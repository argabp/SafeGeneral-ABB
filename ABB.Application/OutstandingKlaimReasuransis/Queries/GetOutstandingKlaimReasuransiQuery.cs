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
using Microsoft.Extensions.Logging;
using Scriban;

namespace ABB.Application.OutstandingKlaimReasuransis.Queries
{
    public class GetOutstandingKlaimReasuransiQuery : IRequest<string>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }
    }

    public class GetOutstandingKlaimReasuransiQueryHandler : IRequestHandler<GetOutstandingKlaimReasuransiQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly ILogger<GetOutstandingKlaimReasuransiQuery> _logger;

        public GetOutstandingKlaimReasuransiQueryHandler(IDbConnectionPst dbConnectionPst, IHostEnvironment environment,
            ReportConfig reportConfig, ILogger<GetOutstandingKlaimReasuransiQuery> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _environment = environment;
            _reportConfig = reportConfig;
            _logger = logger;
        }

        public async Task<string> Handle(GetOutstandingKlaimReasuransiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var sp_name = string.Empty;

                switch (request.jenis_laporan)
                {
                    case "P":
                        sp_name = "spr_cl18r_01_bak2";
                        break;
                    case "R":
                        sp_name = "spr_cl18r_02_bak2";
                        break;
                }
                
                var outstandingKlaimDatas = (await _dbConnectionPst.QueryProc<OutstandingKlaimReasuransiDto>(sp_name, 
                    new
                    {
                        input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                    $"{request.tgl_akh.ToShortDateString()}"
                    })).ToList();

                var report_name = string.Empty;

                switch (request.jenis_laporan)
                {
                    case "P":
                        report_name = "OutstandingKlaimReasuransiRincian.html";
                        break;
                    case "R":
                        report_name = "OutstandingKlaimReasuransiRekap.html";
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
                
                var reportConfig = _reportConfig.GetReportData(request.kd_cb);

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
                            var innerGroups = outerGroup.GroupBy(x => x.kd_mtu_symbol?.Trim()).ToList();
                            
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
                                    <div class='h1'>LAPORAN OUTSTANDING KLAIM REASURANSI</div>
                                        <div class='section'>
                                            <table class='table'>
                                                <tr>
                                                    <td style='width: 40%;'></td>
                                                    <td style='width: 8%; font-weight: bold'>KANTOR</td>
                                                    <td style='text-align: justify; width: 1%; font-weight: bold'>:</td>
                                                    <td style='text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.nm_cb}</td>
                                                </tr>
                                                <tr>
                                                    <td style='width: 40%;'></td>
                                                    <td style='width: 8%; font-weight: bold'>PERIODE</td>
                                                    <td style='text-align: justify; font-weight: bold'>:</td>
                                                    <td style='text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{ReportHelper.ConvertDateTime(innerFirst.tgl_akh, "dd MMM yyyy")}</td>
                                                </tr>
                                                <tr>
                                                    <td style='width: 40%;'></td>
                                                    <td style='width: 8%; font-weight: bold'>C.O.B</td>
                                                    <td style='text-align: justify; font-weight: bold'>:</td>
                                                    <td style='text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.nm_cob}</td>
                                                </tr>
                                            </table>
                                            <table class='table' border='1'>
                                                <tr>
                                                    <td rowspan='2' style='width: 1%; text-align: center; font-weight: bold'>NO. </td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>NOMOR BERKAS <br> NOMOR POLIS <br> TAHUN U/W</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>TERTANGGUNG</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>PERIODE PERTANGGUNGAN <br> TANGGAL KEJADIAN <br> TANGGAL LAPOR</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>T.S.I <br> TAKSRIAN KERUGIAN</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>MTU</td>
                                                    <td colspan='7' style='width: 30%; text-align: center; font-weight: bold'>KLAIM REASURANSI</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>JUMLAH</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>TANGGAL PLA</td>
                                                </tr>
                                                <tr>
                                                    <td style='text-align: center; font-weight: bold'>QUOTA SHARE</td>
                                                    <td style='text-align: center; font-weight: bold'>SURPLUS</td>
                                                    <td style='text-align: center; font-weight: bold'>CONSORSIUM</td>
                                                    <td style='text-align: center; font-weight: bold'>POOL</td>
                                                    <td style='text-align: center; font-weight: bold'>BPPDAN</td>
                                                    <td style='text-align: center; font-weight: bold'>FACULTATIVE</td>
                                                    <td style='text-align: center; font-weight: bold'>XOL</td>
                                                </tr>
                                                <p style='margin-bottom: 0px;'>Dalam : {innerFirst.nm_mtu}</p>");


                                foreach (var data in innerGroup)
                                {
                                    sequence++;
                                    var nomor_berkas = string.IsNullOrWhiteSpace(data.kd_scob) && string.IsNullOrWhiteSpace(data.no_sert)
                                        ? data.no_pol_ttg
                                        : $"{data.kd_cb}/{data.kd_cob}{data.kd_scob}/{data.kd_thn}/{data.no_kl}";
                                    var nomor_polis =
                                        string.IsNullOrWhiteSpace(data.kd_scob) && string.IsNullOrWhiteSpace(data.no_sert)
                                            ? data.no_pol_ttg
                                            : $"{data.no_pol_ttg}";
                                    var jns_sor_qts = ReportHelper.ConvertToReportFormat(data.jns_sor_qts);
                                    var jns_sor_spl = ReportHelper.ConvertToReportFormat(data.jns_sor_spl);
                                    var jns_sor_con = ReportHelper.ConvertToReportFormat(data.jns_sor_con);
                                    var jns_sor_pol = ReportHelper.ConvertToReportFormat(data.jns_sor_pol);
                                    var jns_sor_bppdan = ReportHelper.ConvertToReportFormat(data.jns_sor_bppdan);
                                    var jns_sor_fac = ReportHelper.ConvertToReportFormat(data.jns_sor_fac);
                                    var jns_sor_xol = ReportHelper.ConvertToReportFormat(data.jns_sor_xol);
                                    var total = ReportHelper.ConvertToReportFormat(data.jns_sor_qts + data.jns_sor_spl + data.jns_sor_con + 
                                                                                        data.jns_sor_pol + data.jns_sor_bppdan + data.jns_sor_fac +
                                                                                        data.jns_sor_xol);
                                    

                                    stringBuilder.Append(@$"
                                    <tr>
                                        <td style=''>{sequence}.</td>
                                        <td style=''>{nomor_berkas} <br> {nomor_polis} <br> {data.thn_uw}</td>
                                        <td style=''>{data.nm_ttg}</td>
                                        <td style=''>{ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy")} s/d {ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_kej, "dd/MM/yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_lapor, "dd/MM/yyyy")}</td>
                                        <td style='width: 1%; text-align: left; '>{data.kd_mtu_symbol_tsi} {ReportHelper.ConvertToReportFormat(data.nilai_share_bgu)} <br> {data.kd_mtu_symbol_tsi} {ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl)}</td>
                                        <td style=''>{data.kd_mtu_symbol}</td>
                                        <td style=''>{jns_sor_qts}</td>
                                        <td style=''>{jns_sor_spl}</td>
                                        <td style=''>{jns_sor_con}</td>
                                        <td style=''>{jns_sor_pol}</td>
                                        <td style=''>{jns_sor_bppdan}</td>
                                        <td style=''>{jns_sor_fac}</td>
                                        <td style=''>{jns_sor_xol}</td>
                                        <td style=''>{total}</td>
                                        <td style=''>{ReportHelper.ConvertDateTime(data.tgl_pla_reas, "dd/MM/yyyy")}</td>
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
                            var jns_sor_qts = ReportHelper.ConvertToReportFormat(data.jns_sor_qts);
                            var jns_sor_spl = ReportHelper.ConvertToReportFormat(data.jns_sor_spl);
                            var jns_sor_con = ReportHelper.ConvertToReportFormat(data.jns_sor_con);
                            var jns_sor_pol = ReportHelper.ConvertToReportFormat(data.jns_sor_pol);
                            var jns_sor_bppdan = ReportHelper.ConvertToReportFormat(data.jns_sor_bppdan);
                            var jns_sor_fac = ReportHelper.ConvertToReportFormat(data.jns_sor_fac);
                            var jns_sor_xol = ReportHelper.ConvertToReportFormat(data.jns_sor_xol);
                            var total = ReportHelper.ConvertToReportFormat(data.jns_sor_qts + data.jns_sor_spl + data.jns_sor_con + 
                                                                           data.jns_sor_pol + data.jns_sor_bppdan + data.jns_sor_fac +
                                                                           data.jns_sor_xol);
                            stringBuilder.Append(@$"
                                <tr>
                                    <td style=''>{sequence}</td>
                                    <td style=''>{data.nm_cb}</td>
                                    <td style=''>{data.nm_cob}</td>
                                    <td style=''>{data.kd_mtu_symbol_tsi}</td>
                                    <td style='width: 1%; text-align: right; '>{ReportHelper.ConvertToReportFormat(data.nilai_share_bgu)}</td>
                                    <td style=''>{data.kd_mtu_symbol}</td>
                                    <td style='text-align: right; '>{ReportHelper.ConvertToReportFormat(data.nilai_ttl_pla)}</td>
                                    <td style=''>{jns_sor_qts}</td>
                                    <td style=''>{jns_sor_spl}</td>
                                    <td style=''>{jns_sor_con}</td>
                                    <td style=''>{jns_sor_pol}</td>
                                    <td style=''>{jns_sor_bppdan}</td>
                                    <td style=''>{jns_sor_fac}</td>
                                    <td style=''>{jns_sor_xol}</td>
                                    <td style=''>{total}</td>
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
            }, _logger);
        }
    }
}