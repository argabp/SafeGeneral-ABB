using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakKwitansiKlaim.Queries
{
    public class GetCetakKwitansiKlaimQuery : IRequest<string>
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

    public class GetCetakKwitansiKlaimQueryHandler : IRequestHandler<GetCetakKwitansiKlaimQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public GetCetakKwitansiKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetCetakKwitansiKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<CetakKwitansiKlaimDto>("spr_cl02r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()},"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "CetakKwitansiKlaim.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();

            var draft = string.Empty;

            if (data.flag_posting == "N")
            {
                draft = @"<div class='draft-watermark'>DRAFT</div>";
            }

            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);

            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);
            var resultTemplate = templateProfileResult.Render( new
            {
                nilai_nt, data.no_sert, data.jns_tr, data.jns_nt_msk, data.jns_nt_kel,
                data.almt_ttg, data.almt_kwi, data.nm_ttg, data.kt_kwi, data.ket_kwi,
                data.no_berkas, data.no_pol_ttg, data.no_pol_lama, data.kd_mtu_symbol,
                data.nm_kt_cb, data.tgl_nt_ind, data.ket_nilai_nt, data.no_nota, draft,
                title1 = reportConfig.Title.Title1, data.nm_cb
            } );

            return resultTemplate;
        }
    }
}