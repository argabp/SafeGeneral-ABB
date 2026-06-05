using System;
using System.Collections.Generic;
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
            var tableTitle = string.Empty;

            var groups = request.jns_lap == "1"
                ? datas.Select(s => s.thn_tty_pps + s.nm_tty_pps)
                : datas.Select(s => s.thn_tty_pps + s.reasuradur);

            foreach (var group in groups)
            {
                var firstData = request.jns_lap == "1"
                    ? datas.First(w => w.thn_tty_pps + w.nm_tty_pps == group)
                    : datas.First(w => w.thn_tty_pps + w.reasuradur == group);
                
                switch (request.jns_lap)
                {
                    case "1":
                        header = $"JENIS PERTANGGUNGAN {firstData.nm_tty_pps}";
                        tableTitle = "REASURADUR";
                        break;
                    case "2":
                        header = $"REASURADUR {firstData.reasuradur}";
                        tableTitle = "JENIS PERTANGGUNGAN";
                        break;
                }
                
                stringBuilder.Append($@"
                    <div style='page-break-before: always;'>
                        <div class='container'>
                            <div class='section'>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>NOTA PREMI TREATY KELUAR</strong></p>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>{header}</strong></p>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>BULAN TAHUN : {firstData.bl_prd:MMMM yyyy}</strong></p>

                                <table class='table'>
                                    <tr>
                                        <td style='text-align: right'><strong>U/Y : {firstData.thn_tty_pps}</strong></td>
                                    </tr>
                                </table>

                                <table class='table'>
                                <tr>
                                    <td style='text-align: center; border: 1px solid'>{tableTitle}</td>
                                    <td style='text-align: center; border: 1px solid'>JENIS TREATY</td>
                                    <td style='text-align: center; border: 1px solid'>NO. NOTA</td>
                                    <td style='text-align: center; border: 1px solid'>KURS</td>
                                    <td style='width: 12%; text-align: center; border: 1px solid'>PREMI<br>100%</td>
                                    <td style='width: 15%; text-align: center; border: 1px solid' colspan='2'>PREMI<br>BGN SOR</td>
                                    <td style='width: 12%; text-align: center; border: 1px solid'>KOMISI<br>REASURANSI</td>
                                    <td style='width: 12%; text-align: center; border: 1px solid'>NETTO</td>
                                </tr>");

                var dataBasedOnGroup = request.jns_lap == "1"
                    ? datas.Where(w => w.thn_tty_pps + w.nm_tty_pps == group)
                    : datas.Where(w => w.thn_tty_pps + w.reasuradur == group);
                
                foreach (var data in dataBasedOnGroup)
                {
                    var prm = ReportHelper.ConvertToReportFormat(data.prm);
                    var prm_bgn = ReportHelper.ConvertToReportFormat(data.prm_bgn);
                    var prm_sdr = ReportHelper.ConvertToReportFormat(data.prm_sdr);
                    var kms = ReportHelper.ConvertToReportFormat(data.kms);
                    var netto = ReportHelper.ConvertToReportFormat(data.netto);

                    var tableData = request.jns_lap == "1" ? data.reasuradur : data.nm_tty_pps;
                    
                    stringBuilder.Append(@$"<tr>
                                        <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{tableData}</td>
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

                stringBuilder.Append(@$"<tr>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid;' colspan='3'>TOTAL</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>IDR</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_prm_idr)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'></td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_prm_sdr_idr)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_kms_idr)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_netto_idr)}</td>
                                        </tr>
                                        <tr>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid;' colspan='3'></td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>USD</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_prm_usd)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'></td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_prm_sdr_usd)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_kms_usd)}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{ReportHelper.ConvertToReportFormat(firstData.tot_netto_usd)}</td>
                                        </tr>");

                stringBuilder.Append(@$"</table>

                                <table class='table' style='margin-top: 5rem'>
                                    <tr>
                                        <td style='width: 80%;'></td>
                                        <td style='text-align: center'><strong>{firstData.nm_kpl_bag}</strong></td>
                                    </tr>
                                    <tr>
                                        <td style='width: 80%;'></td>
                                        <td style='text-align: center'><strong>{firstData.nm_bag}</strong></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>");
            }

            resultTemplate = templateProfileResult.Render(new
            {
                data = stringBuilder.ToString()
            });

            return resultTemplate;
        }
    }
}