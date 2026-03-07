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

namespace ABB.Application.PenyelesaianKlaimReasuransis.Queries
{
    public class GetPenyelesaianKlaimReasuransiQuery : IRequest<string>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string jenis_laporan { get; set; }
    }

    public class GetPenyelesaianKlaimReasuransiQueryHandler : IRequestHandler<GetPenyelesaianKlaimReasuransiQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly ILogger<GetPenyelesaianKlaimReasuransiQuery> _logger;

        public GetPenyelesaianKlaimReasuransiQueryHandler(IDbConnectionPst dbConnectionPst, IHostEnvironment environment,
            ReportConfig reportConfig, ILogger<GetPenyelesaianKlaimReasuransiQuery> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _environment = environment;
            _reportConfig = reportConfig;
            _logger = logger;
        }

        public async Task<string> Handle(GetPenyelesaianKlaimReasuransiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var sp_name = string.Empty;

                switch (request.jenis_laporan)
                {
                    case "P":
                        sp_name = "spr_cl07r_01_bak";
                        break;
                    case "R":
                        sp_name = "spr_cl07r_02_bak";
                        break;
                }
                
                var penyelesaianKlaimDatas = (await _dbConnectionPst.QueryProc<PenyelesaianKlaimReasuransiDto>(sp_name, 
                    new
                    {
                        input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                    $"{request.tgl_mul.ToShortDateString()},{request.tgl_akh.ToShortDateString()}"
                    })).ToList();

                var report_name = string.Empty;

                switch (request.jenis_laporan)
                {
                    case "P":
                        report_name = "PenyelesaianKlaimReasuransiRincian.html";
                        break;
                    case "R":
                        report_name = "PenyelesaianKlaimReasuransiRekap.html";
                        break;
                }
                
                string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", report_name );
                
                string templateReportHtml = await File.ReadAllTextAsync( reportPath );
                
                if (penyelesaianKlaimDatas.Count == 0)
                {
                    throw new NullReferenceException("Data tidak ditemukan");
                }
                
                Template templateProfileResult = Template.Parse( templateReportHtml );

                string resultTemplate;

                StringBuilder stringBuilder = new StringBuilder();

                var penyelesaianKlaimData = penyelesaianKlaimDatas.FirstOrDefault();
                
                var reportConfig = _reportConfig.GetReportData(request.kd_cb);

                int sequence;
                switch (request.jenis_laporan)
                {
                    case "P":
                        var groupedData = penyelesaianKlaimDatas
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
                                    <div class='h1'>LAPORAN PENYELESAIAN KLAIM REASURANSI</div>
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
                                                    <td style='text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{ReportHelper.ConvertDateTime(request.tgl_mul, "dd MMM yyyy")} S/D {ReportHelper.ConvertDateTime(innerFirst.tgl_akh, "dd MMM yyyy")}</td>
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
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>NOMOR BERKAS <br> NOMOR NOTA <br> NOMOR POLIS</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>TERTANGGUNG</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>TANGGAL PENYELESAIAN <br> TANGGAL KEJADIAN <br> PERIODE PERTANGGUNGAN</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>T.S.I <br> PENYELESAIAN KLAIM</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>MTU</td>
                                                    <td colspan='7' style='width: 30%; text-align: center; font-weight: bold'>KLAIM REASURANSI</td>
                                                    <td rowspan='2' style='width: 10%; text-align: center; font-weight: bold'>JUMLAH</td>
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
                                    var nomor_nota = $"{data.kd_cb}.{data.jns_tr}.{data.jns_nt_msk}.{data.kd_thn}.{data.kd_bln}.{data.no_nt_msk}/{data.jns_nt_kel}/{data.no_nt_kel}";
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
                                        <td style=''>{data.no_berkas} <br> {nomor_nota} <br> {data.no_pol_ttg}</td>
                                        <td style=''>{data.nm_ttg}</td>
                                        <td style=''>{ReportHelper.ConvertDateTime(data.tgl_closing, "dd MMM yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_kej, "dd MMM yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd MMM yyyy")} s/d {ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd MMM yyyy")}</td>
                                        <td style='width: 1%; text-align: left; '>{data.kd_mtu_symbol_tsi} {ReportHelper.ConvertToReportFormat(data.nilai_share_bgu)} <br> {data.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(data.nilai_ttl_dla)}</td>
                                        <td style=''>{data.kd_mtu_symbol}</td>
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
                                

                                stringBuilder.Append($@"</table>
                                                            </div>
                                                        </div>");
                            }
                        }
                        break;
                    case "R":
                        sequence = 0;
                        foreach (var data in penyelesaianKlaimDatas)
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
                    penyelesaianKlaimData.no_pol_ttg, penyelesaianKlaimData.nm_cb,
                    penyelesaianKlaimData.nm_cob, tgl_akh = penyelesaianKlaimData.tgl_akh.Value.ToString("dd MMMM yyyy")
                } );
                
                return resultTemplate;
            }, _logger);
        }
    }
}