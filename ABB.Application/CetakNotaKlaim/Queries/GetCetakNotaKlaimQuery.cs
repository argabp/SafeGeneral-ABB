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

namespace ABB.Application.CetakNotaKlaim.Queries
{
    public class GetCetakNotaKlaimQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string kd_thn { get; set; }
        public string kd_bln { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
    }

    public class GetCetakNotaKlaimQueryHandler : IRequestHandler<GetCetakNotaKlaimQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetCetakNotaKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetCetakNotaKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<CetakNotaKlaimDto>("spr_cl02r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()},"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "CetakNotaKlaim.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);
            var nilai_01 = ReportHelper.ConvertToReportFormat(data.nilai_01);
            var nilai_02 = ReportHelper.ConvertToReportFormat(data.nilai_02);
            var nilai_03 = ReportHelper.ConvertToReportFormat(data.nilai_03);
            var nilai_04 = ReportHelper.ConvertToReportFormat(data.nilai_04);
            var nilai_05 = ReportHelper.ConvertToReportFormat(data.nilai_05);
            var nilai_total = ReportHelper.ConvertToReportFormat(data.nilai_01 + data.nilai_02 + data.nilai_03 + data.nilai_04 + data.nilai_05);
            var resultTemplate = templateProfileResult.Render( new
            {
                nilai_nt, nilai_01, nilai_02, nilai_03, nilai_04, nilai_05, nilai_total, 
                data.jns_tr, data.jns_nt_msk, data.jns_nt_kel, data.nm_cb, data.almt_ttj,
                data.almt_ttg, data.flag_postr, data.nm_ttg, data.nm_ttj, data.kt_ttj,
                data.no_berkas, data.no_pol_ttg, data.kt_ttg, data.kd_mtu_symbol,
                data.nm_kt_cb, data.tgl_nt_ind, data.no_nota, data.nm_scob, data.nm_scob_ing,
                data.kd_cob, data.kd_scob, data.nilai_share_bgu, data.ket_nt,
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"),
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"),
                data.uraian_01, data.uraian_02, data.uraian_03, data.uraian_04, data.uraian_05
            } );

            return resultTemplate;
        }
    }
}