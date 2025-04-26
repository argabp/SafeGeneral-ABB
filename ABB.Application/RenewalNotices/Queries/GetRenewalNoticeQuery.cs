using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaDanKwitansiPolis.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.RenewalNotices.Queries
{
    public class GetRenewalNoticeQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_pol { get; set; }
        public string? nm_ttg { get; set; }
        public string? almt_ttg { get; set; }
        public int no_updt { get; set; }
        public string no_surat { get; set; }
    }

    public class GetRenewalNoticeQueryHandler : IRequestHandler<GetRenewalNoticeQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        private List<string> ReportHaveDetails = new List<string>()
        {
        };

        private List<string> MultipleReport = new List<string>()
        {
        };

        public GetRenewalNoticeQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetRenewalNoticeQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            
            var datas = (await _connectionFactory.QueryProc<RenewalNoticeDto>("spr_mkt02r_02", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt},{request.no_surat}," +
                                $"{request.nm_ttg?.Trim()},{request.almt_ttg}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "RenewalNotice.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            var resultTemplate = templateProfileResult.Render( new
            {
                    
                request.no_surat, data.tgl_print, data.nm_ttg, data.almt_ttg,
                data.nm_scob, data.no_pol_ttg, data.ket_oby, data.tgl_akh_ptg_ind,
                data.tgl_renew,
            } );

            return resultTemplate;
        }
    }
}