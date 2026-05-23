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

namespace ABB.Application.CetakRekapitulasiProduksiTreatyMasuks.Commands
{
    public class CetakRekapitulasiProduksiTreatyMasukCommand : IRequest<string>
    {
        public DateTime periode { get; set; }
        
        public string jns_lap { get; set; }
    }

    public class
        CetakRekapitulasiProduksiTreatyMasukCommandHandler : IRequestHandler<CetakRekapitulasiProduksiTreatyMasukCommand
        , string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakRekapitulasiProduksiTreatyMasukCommandHandler(IDbConnectionPst connectionPst,
            IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakRekapitulasiProduksiTreatyMasukCommand request,
            CancellationToken cancellationToken)
        {
            var spName = string.Empty;
            switch (request.jns_lap)
            {
                case "1":
                    spName = "spr_ri07r_01";
                    break;
                case "2":
                    spName = "spr_ri07r_02";
                    break;
            }

            var datas = (await _connectionPst.QueryProc<CetakRekapitulasiProduksiTreatyMasukModel>(spName,
                new
                {
                    input_str = $"{request.periode:yyyy/MM/dd}"
                })).ToList();

            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates",
                "RekapitulasiProduksiTreatyMasuk.html");

            string templateReportHtml = await File.ReadAllTextAsync(reportPath);

            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }

            Template templateProfileResult = Template.Parse(templateReportHtml);

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            decimal total_nilai_prm = 0;
            decimal total_nilai_prm_bl = 0;
            decimal total_nilai_kms = 0;
            decimal total_nilai_kms_bl = 0;
            decimal total_net_prm = 0;
            decimal total_net_prm_bl = 0;
            decimal total_nilai_kl = 0;
            decimal total_nilai_kl_bl = 0;

            var sequence = 1;

            stringBuilder.Append("<table class='table'>");
            
            switch (request.jns_lap)
            {
                case "1":
                    stringBuilder.Append(@"
                            <tr>
                                <td style='width: 3%; text-align: center; border: 1px solid' rowspan='2'>NO</td>
                                <td style='text-align: center; border: 1px solid' rowspan='2'>BISNIS</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>GROSS PREMI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>KOMISI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>NET PREMI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>KLAIM</td>
                            </tr>
                            <tr>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                            </tr>");

                    foreach (var data in datas)
                    {
                        var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                        var nilai_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl);
                        var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                        var nilai_kms_bl = ReportHelper.ConvertToReportFormat(data.nilai_kms_bl);
                        var net_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm - data.nilai_kms);
                        var net_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl - data.nilai_kms_bl);
                        var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                        var nilai_kl_bl = ReportHelper.ConvertToReportFormat(data.nilai_kl_bl);

                        stringBuilder.Append(@$"<tr>
                                            <td style='text-align: center; vertical-align: top; border: 1px solid;'>{sequence}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>{data.nm_cob_ing}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kms}</td>
                                            <td style='width: 10%;  text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kms_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{net_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{net_prm_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kl_bl}</td>
                                        </tr>");

                        sequence++;
                        total_nilai_prm += ReportHelper.ConvertToDecimalFormat(nilai_prm);
                        total_nilai_prm_bl += ReportHelper.ConvertToDecimalFormat(nilai_prm_bl);
                        total_nilai_kms += ReportHelper.ConvertToDecimalFormat(nilai_kms);
                        total_nilai_kms_bl += ReportHelper.ConvertToDecimalFormat(nilai_kms_bl);
                        total_net_prm += ReportHelper.ConvertToDecimalFormat(net_prm);
                        total_net_prm_bl += ReportHelper.ConvertToDecimalFormat(net_prm_bl);
                        total_nilai_kl += ReportHelper.ConvertToDecimalFormat(nilai_kl);
                        total_nilai_kl_bl += ReportHelper.ConvertToDecimalFormat(nilai_kl_bl);
                    }

                    stringBuilder.Append(@$"<tr>
                                                    <td style='text-align: left; vertical-align: top; border: 1px solid;'></td>
                                                    <td style='text-align: left; vertical-align: top; border: 1px solid;'>Total</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm_bl}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms}</td>
                                                    <td style='width: 10%;  text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms_bl}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm_bl}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kl}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kl_bl}</td>
                                                </tr>");
                    break;
                case "2":

                    stringBuilder.Append(@"
                            <tr>
                                <td style='width: 3%; text-align: center; border: 1px solid' rowspan='2'>NO</td>
                                <td style='text-align: center; border: 1px solid' rowspan='2'>BISNIS</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>GROSS PREMI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>KOMISI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>NET PREMI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid' colspan='2'>kl</td>
                            </tr>
                            <tr>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>BULAN INI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                            </tr>");

                    var nm_rks = datas.Select(s => s.nm_rk).Distinct().ToList();
                    
                    foreach (var nm_rk in nm_rks)
                    {
                        stringBuilder.Append($@"<tr>
                                    <td style='text-align: left; border: 1px solid' colspan='10'>{nm_rk}</td>
                                </tr>");

                        total_nilai_prm = 0;
                        total_nilai_prm_bl = 0;
                        total_nilai_kms = 0;
                        total_nilai_kms_bl = 0;
                        total_net_prm = 0;
                        total_net_prm_bl = 0;
                        total_nilai_kl = 0;
                        total_nilai_kl_bl = 0;

                        sequence = 1;

                        foreach (var data in datas)
                        {
                            var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                            var nilai_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl);
                            var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                            var nilai_kms_bl = ReportHelper.ConvertToReportFormat(data.nilai_kms_bl);
                            var net_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm - data.nilai_kms);
                            var net_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl - data.nilai_kms_bl);
                            var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                            var nilai_kl_bl = ReportHelper.ConvertToReportFormat(data.nilai_kl_bl);

                            stringBuilder.Append(@$"<tr>
                                                <td style='text-align: center; vertical-align: top; border: 1px solid;'>{sequence}</td>
                                                <td style='text-align: left; vertical-align: top; border: 1px solid;'>{data.nm_cob_ing}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_bl}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kms}</td>
                                                <td style='width: 10%;  text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kms_bl}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{net_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{net_prm_bl}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kl}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kl_bl}</td>
                                            </tr>");

                            sequence++;
                            total_nilai_prm += ReportHelper.ConvertToDecimalFormat(nilai_prm);
                            total_nilai_prm_bl += ReportHelper.ConvertToDecimalFormat(nilai_prm_bl);
                            total_nilai_kms += ReportHelper.ConvertToDecimalFormat(nilai_kms);
                            total_nilai_kms_bl += ReportHelper.ConvertToDecimalFormat(nilai_kms_bl);
                            total_net_prm += ReportHelper.ConvertToDecimalFormat(net_prm);
                            total_net_prm_bl += ReportHelper.ConvertToDecimalFormat(net_prm_bl);
                            total_nilai_kl += ReportHelper.ConvertToDecimalFormat(nilai_kl);
                            total_nilai_kl_bl += ReportHelper.ConvertToDecimalFormat(nilai_kl_bl);
                        }

                        stringBuilder.Append(@$"<tr>
                                                        <td style='text-align: left; vertical-align: top; border: 1px solid;'></td>
                                                        <td style='text-align: left; vertical-align: top; border: 1px solid;'>Total</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm_bl}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms}</td>
                                                        <td style='width: 10%;  text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms_bl}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm_bl}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kl}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kl_bl}</td>
                                                    </tr>");
                    }

                    break;
            }

            stringBuilder.Append("</table>");

            resultTemplate = templateProfileResult.Render(new
            {
                details = stringBuilder.ToString(),
                periode = request.periode.ToString("MMMM yyyy")
            });

            return resultTemplate;
        }
    }
}