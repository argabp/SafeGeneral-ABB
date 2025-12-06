using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.LaporanKerugianSementara.Queries
{
    public class GetLaporanKerugianSementaraQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public string laporan { get; set; }
        public string jabatan { get; set; }
        public string tanda_tangan { get; set; }
    }

    public class GetLaporanKerugianSementaraQueryHandler : IRequestHandler<GetLaporanKerugianSementaraQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        
        public GetLaporanKerugianSementaraQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanKerugianSementaraQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            
            var datas = (await _connectionFactory.QueryProc<LaporanKerugianSementaraDto>("spr_cl01r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_kl.Trim()},{request.no_mts},{request.jabatan.Trim()}," +
                                $"{request.tanda_tangan.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanKerugianSementara.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
            var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
            var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl);
            
            var view_share = data.pst_share_bgu != 100
                ? 
                $@"
                <tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                    <td style='width: 20%;'>Share Co-as untuk kami</td>
                    <td>:</td>
                    <td style='vertical-align: top;'>{data.symbol_ptg}</td>
                    <td style='vertical-align: top;'>{data.nilai_share_bgu}</td>
                    <td></td>
                </tr>"
                : string.Empty;
            
            var view_validitas = data.kd_cob == "M"
                ? 
                $@"
                <tr>
                    <td style='width: 1%;'>12. </td>
                    <td style='width: 20%;'>Validitas</td>
                    <td>:</td>
                    <td colspan='3'>{data.validitas}</td>
                </tr>"
                : string.Empty;
            
            var resultTemplate = templateProfileResult.Render( new
            {
                    
                data.no_berkas, data.nm_ttg, data.no_pol_ttg, data.nm_scob, 
                data.nm_oby, data.symbol_ptg, nilai_ttl_ptg, nilai_share_bgu,
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"), 
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"), 
                tgl_kej = ReportHelper.ConvertDateTime(data.tgl_kej, "dd/MM/yyyy"),
                data.tempat_kej, data.kond_ptg, data.ket_oby, data.symbol_kl,
                data.sebab_kerugian, data.nm_sifat_kerugian, nilai_ttl_kl,
                tgl_lns_prm = ReportHelper.ConvertDateTime(data.tgl_lns_prm, "dd/MM/yyyy"),
                data.no_bukti_lns, view_validitas, data.kt_cb, data.tgl_closing_ind,
                request.tanda_tangan, request.jabatan, view_share
            } );

            return resultTemplate;
        }
    }
}