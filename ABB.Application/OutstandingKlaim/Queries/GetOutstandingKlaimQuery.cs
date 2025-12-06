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
using Microsoft.Extensions.Primitives;
using Scriban;

namespace ABB.Application.OutstandingKlaim.Queries
{
    public class GetOutstandingKlaimQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }
    }

    public class GetOutstandingKlaimQueryHandler : IRequestHandler<GetOutstandingKlaimQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public GetOutstandingKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetOutstandingKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var sp_name = string.Empty;

            switch (request.jenis_laporan)
            {
                case "P":
                    sp_name = "spr_cl06r_01_bgu";
                    break;
                case "R":
                    sp_name = "spr_cl06r_02";
                    break;
            }
            
            var outstandingKlaimDatas = (await _connectionFactory.QueryProc<OutstandingKlaimDto>(sp_name, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.tgl_akh.ToShortDateString()}"
                })).ToList();

            var report_name = string.Empty;

            switch (request.jenis_laporan)
            {
                case "P":
                    report_name = "OutstandingKlaimRincian.html";
                    break;
                case "R":
                    report_name = "OutstandingKlaimRekap.html";
                    break;
            }
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", report_name );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (outstandingKlaimDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            var outstandingKlaimData = outstandingKlaimDatas.FirstOrDefault();
            
            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);

            int sequence;
            switch (request.jenis_laporan)
            {
                case "P":
                    var groupedData = outstandingKlaimDatas
                        .GroupBy(x => new { x.kd_cb, x.kd_cob, x.kd_scob }) // Outer group
                        .ToList();
                    
                    foreach (var outerGroup in groupedData)
                    {
                        // 💡 Now group by kd_mkt INSIDE the outer group
                        var innerGroups = outerGroup.GroupBy(x => new { x.kd_mtu_symbol, x.kd_mtu_symbol_tsi }).ToList();
                        
                        foreach (var innerGroup in innerGroups)
                        {
                            sequence = 0;
                            var innerFirst = innerGroup.FirstOrDefault();

                            stringBuilder.Append($@"<div style='page-break-before: always;'>
                                <table class='table'>
                                    <tr>
                                        <td style='text-transform: uppercase;'>{reportConfig.Title.Title1}</td>
                                    </tr>
                                </table>
                                <div class='h1'>LAPORAN OUTSTANDING KLAIM</div>
                                    <div class='section'>
                                        <table class='table'>
                                            <tr>
                                                <td style='width: 40%;'></td>
                                                <td style='width: 8%; vertical-align: top; font-weight: bold'>KANTOR</td>
                                                <td style='vertical-align: top; text-align: justify; width: 1%; font-weight: bold'>:</td>
                                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.nm_cb}</td>
                                            </tr>
                                            <tr>
                                                <td style='width: 40%;'></td>
                                                <td style='width: 8%; vertical-align: top; font-weight: bold'>PERIODE</td>
                                                <td style='vertical-align: top; text-align: justify; font-weight: bold'>:</td>
                                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{ReportHelper.ConvertDateTime(innerFirst.tgl_akh, "dd MMM yyyy")}</td>
                                            </tr>
                                            <tr>
                                                <td style='width: 40%;'></td>
                                                <td style='width: 8%; vertical-align: top; font-weight: bold'>C.O.B</td>
                                                <td style='vertical-align: top; text-align: justify; font-weight: bold'>:</td>
                                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.nm_cob}</td>
                                            </tr>
                                        </table>
                                        <table class='table' border='1'>
                                            <tr>
                                                <td style='width: 1%; text-align: center; vertical-align: top; font-weight: bold'>NO. </td>
                                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>NOMOR BERKAS <br> NOMOR POLIS</td>
                                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>TERTANGGUNG</td>
                                                <td style='width: 20%; text-align: center; vertical-align: top' font-weight: bold>OBJEK PERTANGGUNGAN <br> PENYEBAB KERUGIAN <br> LOKASI KEJADIAN</td>
                                                <td style='width: 20%; text-align: center; vertical-align: top' font-weight: bold>PERIODE PERTANGGUNGAN <br> TANGGAL KEJADIAN <br> TANGGAL LAPOR</td>
                                                <td style='width: 10%;  text-align: center; vertical-align: top; font-weight: bold'>T.S.I</td>
                                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold'>NILAI O/S</td>
                                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold''>KETERANGAN</td>
                                            </tr>
                                            <p style='margin-bottom: 0px;'>Dalam : {innerFirst.nm_mtu}</p>");


                            foreach (var data in innerGroup)
                            {
                                sequence++;
                                var nomor_polis =
                                    string.IsNullOrWhiteSpace(data.kd_scob) && string.IsNullOrWhiteSpace(data.no_sert)
                                        ? data.no_pol_ttg
                                        : $"{data.kd_cb}/{data.kd_cob}{data.kd_scob}/{data.kd_thn}/{data.no_kl} <br> {data.no_pol_ttg}";

                                stringBuilder.Append(@$"
                                <tr>
                                    <td style='vertical-align: top;'>{sequence}.</td>
                                    <td style='vertical-align: top'>{nomor_polis}</td>
                                    <td style='vertical-align: top;'>{data.nm_ttg}</td>
                                    <td style='vertical-align: top'>{data.nm_oby} <br> {data.sebab_kerugian} <br> {data.tempat_kej}</td>
                                    <td style='vertical-align: top'>{ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy")} s/d {ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_kej, "dd/MM/yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_lapor, "dd/MM/yyyy")}</td>
                                    <td style='width: 1%; text-align: left; vertical-align: top'>{data.kd_mtu_symbol_tsi} {ReportHelper.ConvertToReportFormat(data.nilai_share_bgu)}</td>
                                    <td style='text-align: left; vertical-align: top'>{data.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl)}</td>
                                    <td style='vertical-align: top;'>{data.nm_sifat_kerugian}</td>
                                </tr>");
                            }
                            

                            stringBuilder.Append($@"</table>
                                                        </div>
                                                    </div>");
                        }
                    }
                    break;
                case "R":
                    sequence = 0;
                    foreach (var data in outstandingKlaimDatas)
                    {
                        sequence++;
                        stringBuilder.Append(@$"
                            <tr>
                                <td style='vertical-align: top;'>{sequence}</td>
                                <td style='vertical-align: top'>{data.nm_cb}</td>
                                <td style='vertical-align: top;'>{data.nm_cob}</td>
                                <td style='vertical-align: top'>{data.kd_mtu_symbol_tsi}</td>
                                <td style='vertical-align: top'>{data.kd_mtu_symbol}</td>
                                <td style='width: 1%; text-align: right; vertical-align: top'>{ReportHelper.ConvertToReportFormat(data.nilai_share_bgu)}</td>
                                <td style='text-align: right; vertical-align: top'>{ReportHelper.ConvertToReportFormat(data.nilai_ttl_pla)}</td>
                            </tr>");
                    }
                    break;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(),
                outstandingKlaimData.no_pol_ttg, outstandingKlaimData.nm_cb,
                outstandingKlaimData.nm_cob, tgl_akh = outstandingKlaimData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}