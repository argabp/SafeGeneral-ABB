using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
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
        private readonly ReportConfig _reportConfig;

        public GetLossRecordQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
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
            
            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);

            
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
            var groups = lossRecordDatas
                            .Select(s => s.kd_cb + s.kd_cob + s.kd_scob + s.nm_ttg + s.kd_mtu_mts).Distinct().ToList();

            var firstData = true;
            
            foreach (var group in groups)
            {
                var sequence = 0;
                decimal total_nilai_kl = 0;
                
                var groupDetail = lossRecordDatas.FirstOrDefault(w =>
                    w.kd_cb + w.kd_cob + w.kd_scob + w.nm_ttg + w.kd_mtu_mts == group);

                var style = firstData ? "" : "style='page-break-before: always;'";
                firstData = false;
                
                stringBuilder.Append($@"<div {style}>
                                            <table class='table'>
                                                <tr>
                                                    <td>{reportConfig.Title.Title1} <br> {reportConfig.Title.Title2}</td>
                                                </tr>
                                            </table>
                                            <div class='h1'>LAPORAN LOSS RECORD</div>
                                            <div class='section'>
                                                <table class='table'>
                                                    <tr>
                                                        <td style='text-align: center;'>Periode {ReportHelper.ConvertDateTime(groupDetail.tgl_mul,"dd MMM yyyy")} s/d {ReportHelper.ConvertDateTime(groupDetail.tgl_akh,"dd MMM yyyy")}</td>
                                                    </tr>
                                                </table>
                                                <p style='margin-bottom: 0px;'><strong>Klaim {groupDetail.nm_cob}</strong> <br> <strong>{groupDetail.nm_ttg}</strong></p>
                                                <table class='table' border='1'>
                                                    <tr>
                                                        <td style='width: 1%; text-align: center; vertical-align: top;'>NO. </td>
                                                        <td style='width: 20%; text-align: center; vertical-align: top;'>No. Klaim</td>
                                                        <td style='width: 10%; text-align: center; vertical-align: top;'>Kejadian</td>
                                                        <td style='width: 1%; text-align: center; vertical-align: top'>Kur</td>
                                                        <td style='width: 20%; text-align: center; vertical-align: top'>Nilai Klaim</td>
                                                        <td style='width: 10%;  text-align: center;'>Status</td>
                                                    </tr>");
                
                foreach (var data in lossRecordDatas.Where(w =>
                             w.kd_cb + w.kd_cob+ w.kd_scob + w.nm_ttg + w.kd_mtu_mts == group))
                {
                    sequence++;
                    var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                    stringBuilder.Append(@$"
                    <tr>
                        <td style='vertical-align: top;'>{sequence}</td>
                        <td style='vertical-align: top'>{data.no_berkas}</td>
                        <td style='vertical-align: top;'>{ReportHelper.ConvertDateTime(data.tgl_kej, "dd MMM yyyy")}</td>
                        <td style='vertical-align: top'>{data.kd_mtu_symbol}</td>
                        <td style='vertical-align: top'>{nilai_kl}</td>
                        <td style='vertical-align: top'>{data.status}</td>
                    </tr>");
                    total_nilai_kl += ReportHelper.ConvertToDecimalFormat(nilai_kl);
                }

                stringBuilder.Append($@"
                                        <tr>
                                            <td style='padding-left: 40px;' colspan='3'>TOTAL</td>
                                            <td>{groupDetail.kd_mtu_symbol}</td>
                                            <td>{total_nilai_kl}</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>");
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(), lossRecordData.nm_cb, lossRecordData.kd_mtu_symbol,
                lossRecordData.nm_cob, tgl_mul = lossRecordData.tgl_mul.Value.ToString("dd MMMM yyyy"),
                tgl_akh = lossRecordData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}