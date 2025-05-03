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

namespace ABB.Application.LossRecord.Queries
{
    public class GetLossRecordQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }

        public string? kd_grp_ttg { get; set; }

        public string? kd_rk_ttg { get; set; }
    }

    public class GetLossRecordQueryHandler : IRequestHandler<GetLossRecordQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLossRecordQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLossRecordQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var lossRecordDatas = (await _connectionFactory.QueryProc<LossRecordDto>("spr_cl14r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.kd_mul.ToShortDateString()},{request.kd_akh.ToShortDateString()}," +
                                $"{request.kd_grp_ttg?.Trim()},{request.kd_rk_ttg?.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LossRecord.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (lossRecordDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            var lossRecordData = lossRecordDatas.FirstOrDefault();

            StringBuilder stringBuilder = new StringBuilder();
            var sequence = 0;
            decimal total_nilai_kl = 0;
            foreach (var data in lossRecordDatas)
            {
                sequence++;
                var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                stringBuilder.Append(@$"<tr>
                                            <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                        </tr>");
                total_nilai_kl += ReportHelper.ConvertToDecimalFormat(nilai_kl);
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                total_nilai_kl = ReportHelper.ConvertToReportFormat(total_nilai_kl),
                details = stringBuilder.ToString(), lossRecordData.nm_cb,
                lossRecordData.nm_cob, tgl_mul = lossRecordData.tgl_mul.Value.ToString("dd MMMM yyyy"),
                tgl_akh = lossRecordData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}