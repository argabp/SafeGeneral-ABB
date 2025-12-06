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
        private readonly ReportConfig _reportConfig;

        public GetPenyelesaianKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
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
            
            var groupedData = penyelesaianKlaimDatas
                .GroupBy(x => new { x.kd_cb, x.kd_cob, x.kd_scob }) // Outer group
                .ToList();
            
            var lastOuterKey = groupedData.Last().Key;
            
            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);

            foreach (var outerGroup in groupedData)
            {
                // 💡 Now group by kd_mkt INSIDE the outer group
                var innerGroups = outerGroup.GroupBy(x => new { x.kd_mtu_symbol, x.kd_mtu_symbol_tsi }).ToList();
                
                var lastInnerKey = innerGroups.Last().Key;
                
                foreach (var innerGroup in innerGroups)
                {
                    var sequence = 0;
                    var innerFirst = innerGroup.FirstOrDefault();

                    stringBuilder.Append($@"<div style='page-break-before: always;'>
                        
                    <table class='table'>
                        <tr>
                            <td style='text-transform: uppercase;'>{reportConfig.Title.Title1}</td>
                        </tr>
                    </table>
                    <div class='h1'>LAPORAN PENYELESAIAN KLAIM </div>
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
                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{ReportHelper.ConvertDateTime(innerFirst.tgl_mul,"dd MMM yyyy")} s/d {ReportHelper.ConvertDateTime(innerFirst.tgl_akh,"dd MMM yyyy")}</td>
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
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>NOMOR BERKAS <br> NOMOR NOTA <br> NOMOR POLIS</td>
                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold'>TERTANGGUNG</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>OBJEK PERTANGGUNGAN <br> PENYEBAB KERUGIAN <br> LOKASI KEJADIAN</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>TANGGAL PENYELESAIAN <br> TANGGAL KEJADIAN <br>PERIODE PERTANGGUNGAN</td>
                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold'>T.S.I</td>
                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold'>NILAI O/S</td>
                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold'>PENYELESAIAN KLAIM</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold'>KETERANGAN <br> <br> Bia Materai</td>
                            </tr>
                            <p style='margin-bottom: 0px;'>Dalam: {innerFirst.nm_mtu}</p>
                        ");


                    foreach (var data in innerGroup)
                    {
                        sequence++;
                    
                        var nilai_tsi_pst = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu / data.pst_share_bgu);
                    
                        stringBuilder.Append(@$"
                        <tr>
                            <td style='vertical-align: top;'>{sequence}</td>
                            <td style='vertical-align: top'>{data.no_berkas} <br> {data.no_nota} <br> {data.no_pol_ttg}</td>
                            <td style='vertical-align: top;'>{data.nm_ttg}</td>
                            <td style='vertical-align: top'>{data.nm_oby} <br> {data.sebab_kerugian} <br> {data.tempat_kej}</td>
                            <td style='vertical-align: top'>{ReportHelper.ConvertDateTime(data.tgl_closing, "dd MMM yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_kej, "dd MMM yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd MMM yyyy")}</td>
                            <td style='width: 1%; text-align: left; vertical-align: top'>{data.kd_mtu_symbol_tsi} {nilai_tsi_pst}</td>
                            <td style='text-align: left; vertical-align: top'>{data.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(data.nilai_ttl_pla)}</td>
                            <td style='text-align: left; vertical-align: top'>{data.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(data.nilai_ttl_dla)}</td>
                            <td style='vertical-align: top;'>{data.nm_flag_settled}</td>
                        </tr>");
                    }

                    stringBuilder.Append($@"</table>
                                            </div>
                                        </div>");
                }
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