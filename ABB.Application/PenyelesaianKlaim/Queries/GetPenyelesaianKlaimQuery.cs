using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.PenyelesaianKlaim.Queries
{
    public class GetPenyelesaianKlaimQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }
    }

    public class GetPenyelesaianKlaimQueryHandler : IRequestHandler<GetPenyelesaianKlaimQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetPenyelesaianKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetPenyelesaianKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var penyelesaianKlaimDatas = (await _connectionFactory.QueryProc<PenyelesaianKlaimDto>("spr_cl07r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.kd_mul.ToShortDateString()},{request.kd_akh.ToShortDateString()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "PenyelesaianKlaim.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (penyelesaianKlaimDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            var penyelesaianKlaimData = penyelesaianKlaimDatas.FirstOrDefault();

            StringBuilder stringBuilder = new StringBuilder();
            var sequence = 0;
            foreach (var data in penyelesaianKlaimDatas)
            {
                sequence++;
                var nilai_tsi_pst = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu / data.pst_share_bgu);
                var nilai_tsi = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                var pst_share_bgu = ReportHelper.ConvertToReportFormat(data.pst_share_bgu, true);
                stringBuilder.Append(@$"<tr>
                                            <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_tsi_pst}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_tsi}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{pst_share_bgu}</td>
                                        </tr>");
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(),
                penyelesaianKlaimData.no_pol_ttg, penyelesaianKlaimData.nm_mtu, penyelesaianKlaimData.nm_cb,
                penyelesaianKlaimData.nm_cob, tgl_mul = penyelesaianKlaimData.tgl_mul.Value.ToString("dd MMMM yyyy"),
                tgl_akh = penyelesaianKlaimData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}