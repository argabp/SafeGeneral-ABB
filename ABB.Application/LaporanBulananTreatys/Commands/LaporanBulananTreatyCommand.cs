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

namespace ABB.Application.LaporanBulananTreatys.Commands
{
    public class LaporanBulananTreatyCommand : IRequest<string>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string jns_tr { get; set; }
    }

    public class LaporanBulananTreatyCommandHandler : IRequestHandler<LaporanBulananTreatyCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public LaporanBulananTreatyCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionPst = connectionPst;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(LaporanBulananTreatyCommand request, CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<LaporanBulananTreatyModel>("spr_prod_tty", 
                new
                {
                    input_str = $"{request.kd_cob}, {request.tgl_mul}, {request.tgl_akh}, {request.kd_grp_pas}, {request.kd_rk_pas}, {request.jns_tr}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanBulananTreaty.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );

            var reportConfig = _reportConfig.GetReportData(request.kd_cb);
            
            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            var header = request.jns_tr == "2" ? "Treaty Masuk" : "Treaty Keluar";

            var groups = datas.Select(s => s.ceding).Distinct();

            foreach (var group in groups)
            {
                var firstGroupData = datas.First(w => w.ceding == group);
                var ceding = group == "ZA" ? "Total All Ceding" : group;
                stringBuilder.Append(@$"
                                        <div style='page-break-before: always;'>
                                            <div class='container'>
                                                <div class='section'>
                                                    <p style='font-size: 14px; margin: auto; text-align: left; padding-bottom: 3em'><strong>{reportConfig.Title.Title1}</strong></p>
                                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>Buku Produksi {header}</strong></p>
                                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>PRODUKSI {request.tgl_mul:dd MMMM yyyy} - {request.tgl_akh:dd MMMM yyyy}</strong></p>

                                                    <p style='font-size: 14px; margin: auto; text-align: left; padding-top: 3em'><strong>CEDING : {ceding}</strong></p>
                                                    <table class='table'>
                                                        <tr>
                                                            <td style='text-align: center; border: 1px solid'>No</td>
                                                            <td style='text-align: center; border: 1px solid'>COB</td>
                                                            <td style='text-align: center; border: 1px solid'>TYPE TREATY</td>
                                                            <td style='text-align: center; border: 1px solid'>NO NOTA</td>
                                                            <td style='text-align: center; border: 1px solid'>NO REFF</td>
                                                            <td style='width: 2%; text-align: center; border: 1px solid'>MTU</td>
                                                            <td style='width: 7%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                                            <td style='width: 7%; text-align: center; border: 1px solid'>KOMISI</td>
                                                            <td style='width: 7%; text-align: center; border: 1px solid'>KLAIM</td>
                                                            <td style='width: 7%; text-align: center; border: 1px solid'>NETTO</td>   
                                                        </tr>");
            
                var sequence = 1;
                foreach (var data in datas.Where(w => w.ceding == group))
                {
                    var gross_premi = ReportHelper.ConvertToReportFormat(data.gross_premi);
                    var komisi = ReportHelper.ConvertToReportFormat(data.komisi);
                    var klaim = ReportHelper.ConvertToReportFormat(data.klaim);
                    var netto = ReportHelper.ConvertToReportFormat(data.netto);

                    var sequenceText = string.IsNullOrWhiteSpace(data.cob) ? string.Empty : sequence.ToString();
                    stringBuilder.Append(@$"<tr>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{sequenceText}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.cob}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.treaty}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.no_nota}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.no_polis}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.mtu}</td>
                                                <td style='text-align: right; vertical-align: top; border: 1px solid'>{gross_premi}</td>
                                                <td style='text-align: right; vertical-align: top; border: 1px solid'>{komisi}</td>
                                                <td style='text-align: right; vertical-align: top; border: 1px solid'>{klaim}</td>
                                                <td style='text-align: right; vertical-align: top; border: 1px solid'>{netto}</td>
                                            </tr>");
                    sequence++;
                }

                stringBuilder.Append(@$"
                                                </table>
                                            </div>
                                        </div>
                                    </div>");
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                data = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}