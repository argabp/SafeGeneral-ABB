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

namespace ABB.Application.CetakNotaPremiTreatyKeluars.Commands
{
    public class CetakNotaPremiTreatyKeluarCommand : IRequest<string>
    {
        public DateTime periode { get; set; }
        
        public string jns_lap { get; set; }
    }

    public class
        CetakNotaPremiTreatyKeluarCommandHandler : IRequestHandler<CetakNotaPremiTreatyKeluarCommand
        , string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakNotaPremiTreatyKeluarCommandHandler(IDbConnectionPst connectionPst,
            IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakNotaPremiTreatyKeluarCommand request,
            CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<CetakNotaPremiTreatyKeluarModel>("spr_ri08r_03",
                new
                {
                    input_str = $"{request.periode:yyyy/MM/dd}"
                })).ToList();

            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates",
                "NotaPremiTreatyKeluar.html");

            string templateReportHtml = await File.ReadAllTextAsync(reportPath);

            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }

            Template templateProfileResult = Template.Parse(templateReportHtml);

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            var header = string.Empty;
            

            stringBuilder.Append("<table class='table'>");

            var firstData = datas.First();
            
            switch (request.jns_lap)
            {
                case "1":
                    header = $"JENIS PERTANGGUNGAN {firstData.reasuradur}";
                    stringBuilder.Append(@"
                            <tr>
                                <td style='text-align: center; border: 1px solid'>REASURADUR</td>
                                <td style='text-align: center; border: 1px solid'>JENIS TREATY</td>
                                <td style='text-align: center; border: 1px solid'>NO. NOTA</td>
                                <td style='text-align: center; border: 1px solid'>KURS</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>PREMI<br>100%</td>
                                <td style='width: 15%; text-align: center; border: 1px solid' colspan='2'>PREMI<br>BGN SOR</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>KOMISI<br>REASURANSI</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>NETTO</td>
                            </tr>");

                    foreach (var data in datas)
                    {
                        var prm = ReportHelper.ConvertToReportFormat(data.prm);
                        var prm_bgn = ReportHelper.ConvertToReportFormat(data.prm_bgn);
                        var prm_sdr = ReportHelper.ConvertToReportFormat(data.prm_sdr);
                        var kms = ReportHelper.ConvertToReportFormat(data.kms);
                        var netto = ReportHelper.ConvertToReportFormat(data.netto);

                        stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.nm_tty_pps}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.kd_jns_sor}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.no_nota}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.symbol_mtu}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm_bgn}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm_sdr}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{netto}</td>
                                        </tr>");
                    }
                    break;
                case "2":
                    header = $"REASURADUR {firstData.reasuradur}";
                    stringBuilder.Append(@"
                            <tr>
                                <td style='text-align: center; border: 1px solid'>JENIS PERTANGGUNGAN</td>
                                <td style='text-align: center; border: 1px solid'>JENIS TREATY</td>
                                <td style='text-align: center; border: 1px solid'>NO. NOTA</td>
                                <td style='text-align: center; border: 1px solid'>KURS</td>
                                <td style='width: 15%; text-align: center; border: 1px solid'>PREMI<br>100%</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>PREMI<br>BGN SOR</td>
                                <td style='width: 15%; text-align: center; border: 1px solid'>KOMISI<br>REASURANSI</td>
                                <td style='width: 15%; text-align: center; border: 1px solid'>NETTO</td>
                            </tr>");

                    foreach (var data in datas)
                    {
                        var prm = ReportHelper.ConvertToReportFormat(data.prm);
                        var prm_bgn = ReportHelper.ConvertToReportFormat(data.prm_bgn);
                        var prm_sdr = ReportHelper.ConvertToReportFormat(data.prm_sdr);
                        var kms = ReportHelper.ConvertToReportFormat(data.kms);
                        var netto = ReportHelper.ConvertToReportFormat(data.netto);

                        stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.nm_tty_pps}</td>
                                            <td style='text-align: left; vertical-align: top;  border-right: 1px solid; border-left: 1px solid;'>{data.kd_jns_sor}</td>
                                            <td style='text-align: left; vertical-align: top;  border-right: 1px solid; border-left: 1px solid;'>{data.no_nota}</td>
                                            <td style='text-align: left; vertical-align: top;  border-right: 1px solid; border-left: 1px solid;'>{data.symbol_mtu}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm_bgn}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{prm_sdr}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{netto}</td>
                                        </tr>");
                    }
                    break;
            }

            stringBuilder.Append(@$"<tr>
                                        <td style='text-align: right; vertical-align: top; border: 1px solid;' colspan='3'>TOTAL</td>
                                        <td style='text-align: left; vertical-align: top; border: 1px solid;'>IDR</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_prm_idr}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_prm_sdr_idr}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_kms_idr}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_netto_idr}</td>
                                    </tr>
                                    <tr>
                                        <td style='text-align: right; vertical-align: top; border: 1px solid;' colspan='3'></td>
                                        <td style='text-align: left; vertical-align: top; border: 1px solid;'>USD</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_prm_usd}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_prm_sdr_usd}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_kms_usd}</td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{firstData.tot_netto_usd}</td>
                                    </tr>");

            stringBuilder.Append("</table>");

            resultTemplate = templateProfileResult.Render(new
            {
                details = stringBuilder.ToString(), header,
                periode = firstData.bl_prd.ToString("MMMM yyyy"),
                firstData.thn_tty_pps
            });

            return resultTemplate;
        }
    }
}