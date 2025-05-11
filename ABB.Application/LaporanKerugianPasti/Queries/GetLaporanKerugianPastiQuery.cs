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

namespace ABB.Application.LaporanKerugianPasti.Queries
{
    public class GetLaporanKerugianPastiQuery : IRequest<string>
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

        public string tipe_mts { get; set; }
        public string ket_jns { get; set; }
    }

    public class GetLaporanKerugianPastiQueryHandler : IRequestHandler<GetLaporanKerugianPastiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLaporanKerugianPastiQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanKerugianPastiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            
            var datas = (await _connectionFactory.QueryProc<LaporanKerugianPastiDto>("spr_cl11r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_kl.Trim()},{request.no_mts},{request.laporan.Trim()}," +
                                $"{request.jabatan.Trim()},{request.tanda_tangan.Trim()}"
                })).ToList();

            var report_name = request.tipe_mts.Trim() == "B" ? "LaporanKerugianPastiBeban.html" : "LaporanKerugianPasti.html";
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", report_name );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
            var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
            var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl);
            var nilai_estimasi = ReportHelper.ConvertToReportFormat(data.nilai_estimasi);
            var nilai_kl_1 = ReportHelper.ConvertToReportFormat(data.nilai_kl_1);
            var nilai_kl_2 = ReportHelper.ConvertToReportFormat(data.nilai_kl_2);
            var nilai_kl_3 = ReportHelper.ConvertToReportFormat(data.nilai_kl_3);
            var nilai_kl_4 = ReportHelper.ConvertToReportFormat(data.nilai_kl_4);
            var nilai_kl_5 = ReportHelper.ConvertToReportFormat(data.nilai_kl_5);
            var nilai_kl_6 = ReportHelper.ConvertToReportFormat(data.nilai_kl_6);
            var resultTemplate = templateProfileResult.Render( new
            {
                    
                data.no_berkas, data.nm_ttg, data.no_pol_ttg, data.nm_scob, 
                tgl_kej = ReportHelper.ConvertDateTime(data.tgl_kej, "dd/MM/yyyy"),
                data.nm_oby, data.symbol_ptg, nilai_ttl_ptg, nilai_share_bgu,
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"), 
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"), 
                data.tempat_kej, data.kond_ptg, data.ket_oby, data.symbol_kl,
                data.sebab_kerugian, data.nm_sifat_kerugian, nilai_ttl_kl,
                tgl_lns_prm = ReportHelper.ConvertDateTime(data.tgl_lns_prm, "dd/MM/yyyy"),
                data.no_bukti_lns, data.validitas, data.kt_cb, data.tgl_closing_ind,
                request.tanda_tangan, request.jabatan, data.no_berkas_pla,
                nilai_estimasi, data.nm_oby_1, nilai_kl_1, data.nm_oby_2, nilai_kl_2,
                data.nm_oby_3, nilai_kl_3, data.nm_oby_4, nilai_kl_4, data.ket_kl,
                data.nm_oby_5, nilai_kl_5, data.nm_oby_6, nilai_kl_6, data.ket_jns,
                tgl_closing = ReportHelper.ConvertDateTime(data.tgl_closing, "dd/MM/yyyy"),
            } );

            return resultTemplate;
        }
    }
}