using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKlaim.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakLampiranEndorsments.Queries
{
    public class GetCetakLampiranEndorsmentQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_pol { get; set; }
        public int no_updt { get; set; }
    }

    public class GetCetakLampiranEndorsmentQueryHandler : IRequestHandler<GetCetakLampiranEndorsmentQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetCetakLampiranEndorsmentQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetCetakLampiranEndorsmentQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<CetakLampiranEndorsmentModel>("spr_uw11r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "CetakLampiranEndorsment.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();

            string nm_cob = string.Empty;

            switch (request.kd_cob.Trim())
            {
                case "M":
                    nm_cob = "Jenis Kendaraan";
                    break;
                case "F":
                    nm_cob = "Letak Resiko";
                    break;
                case "C":
                    nm_cob = "Nama Kapal";
                    break;
                default:
                    nm_cob = "Nama obyek";
                    break;
            }

            var ket_endt = data.ket_endt.Replace("\r\n", "<br>");
            
            var resultTemplate = templateProfileResult.Render( new
            {
                data.almt_ttg, data.no_endt, data.nm_ttg, nm_cob, data.no_updt,
                data.kd_jns_ptg, data.no_pol_ttg, data.kt_ttg, ket_endt, data.other,
                data.kd_cob, tsi = ReportHelper.ConvertToReportFormat(data.tsi),
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"),
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"),
                data.symbol, data.kd_cob_1, 
            } );

            return resultTemplate;
        }
    }
}