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

namespace ABB.Application.BukuKerugian.Queries
{
    public class GetBukuKerugianQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }
    }

    public class GetBukuKerugianQueryHandler : IRequestHandler<GetBukuKerugianQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public GetBukuKerugianQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetBukuKerugianQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var bukuKerugianDatas = (await _connectionFactory.QueryProc<BukuKerugianDto>("spr_cl05r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.kd_mul.ToShortDateString()},{request.kd_akh.ToShortDateString()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "BukuKerugian.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (bukuKerugianDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();
            
            var groupedData = bukuKerugianDatas
                .GroupBy(x => new { x.kd_cb, x.kd_cob, x.kd_scob }) // Outer group
                .ToList();
            
            var lastOuterKey = groupedData.Last().Key;

            var reportConfig = _reportConfig.GetReportData(request.kd_cb);
            
            foreach (var outerGroup in groupedData)
            {
                // 💡 Now group by kd_mkt INSIDE the outer group
                var innerGroups = outerGroup.GroupBy(x => x.nm_mtu).ToList();

                var lastInnerKey = innerGroups.Last().Key;
                
                var sequence = 0;
                foreach (var innerGroup in innerGroups)
                {
                    var innerFirst = innerGroup.FirstOrDefault();

                    stringBuilder.Append($@"<div style='page-break-before: always;'>
                    <table class='table'>
                        <tr>
                            <td style='text-transform: uppercase;'>{reportConfig.Title.Title1}</td>
                        </tr>
                    </table>
                    <div class='h1'>AGENDA KLAIM</div>
                    <div class='section'>
                        <table class='table'>
                            <tr>
                                <td style='width: 40%;'></td>
                                <td style='width: 8%; vertical-align: top; font-weight: bold;'>KANTOR</td>
                                <td style='vertical-align: top; text-align: justify; width: 1%; font-weight: bold'>:</td>
                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.nm_cb}</td>
                            </tr>
                            <tr>
                                <td style='width: 40%;'></td>
                                <td style='width: 8%; vertical-align: top; font-weight: bold'>PERIODE</td>
                                <td style='vertical-align: top; text-align: justify; font-weight: bold'>:</td>
                                <td style='vertical-align: top; text-align: justify; text-transform: uppercase; font-weight: bold' colspan='6'>{innerFirst.tgl_mul.Value:dd MMMM yyyy} s/d {innerFirst.tgl_akh.Value:dd MMMM yyyy}</td>
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
                                <td style='width: 1%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>NO. </td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>NOMOR BERKAS <br> NOMOR POLIS</td>
                                <td style='width: 10%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>TERTANGGUNG</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>OBJEK PERTANGGUNGAN <br> PENYEBAB KERUGIAN <br> LOKASI KEJADIAN</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>PERIODE PERTANGGUNGAN TANGGAL KEJADIAN</td>
                                <td style='width: 20%;  text-align: center; font-weight: bold' colspan='2'>T.S.I</td>
                                <td style='width: 20%; text-align: center; font-weight: bold' colspan='2'>NILAI LKS KERUGIAN</td>
                                <td style='width: 20%; text-align: center; vertical-align: top; font-weight: bold' rowspan='2'>KETERANGAN</td>
                            </tr>
                            <tr>
                                <td style='text-align: center; width: 25%; font-weight: bold'>Original</td>
                                <td style='text-align: center; width: 25%; font-weight: bold'>Rupiah</td>
                                <td style='text-align: center; width: 25%; font-weight: bold'>Original</td>
                                <td style='text-align: center; width: 25%; font-weight: bold'>Rupiah</td>
                            </tr>
                            <p style='margin-bottom: 0px;'>Dalam: {innerFirst.nm_mtu}</p>");
                    
                    
                    decimal total_nilai_tsi_pst = 0;
                    decimal total_nilai_tsi = 0;
                    decimal total_nilai_tsi_pst_idr = 0;
                    decimal total_nilai_ttl_kl = 0;
                    decimal total_nilai_ttl_kl_idr = 0;
                    
                    foreach (var data in innerGroup)
                    {
                        sequence++;
                        var nilai_tsi_pst = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu / data.pst_share_bgu * 100);
                        var nilai_tsi = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                        var nilai_tsi_pst_idr = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu_idr / data.pst_share_bgu * 100);
                        var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl);
                        var nilai_ttl_kl_idr = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl_idr);
                        stringBuilder.Append(@$"
                            <tr>
                                <td style='vertical-align: top'>{sequence}</td>
                                <td style='vertical-align: top'>{data.no_berkas} <br> {data.no_pol_ttg}</td>
                                <td style='vertical-align: top;'>{data.nm_ttg}</td>
                                <td style='vertical-align: top'>{data.nm_oby} <br> {data.sebab_kerugian} <br> {data.tempat_kej}</td>
                                <td style='vertical-align: top'>{ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd MMM yyyy")} s/d {ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd MMM yyyy")} <br> {ReportHelper.ConvertDateTime(data.tgl_kej, "dd MMM yyyy")} </td>
                                <td style='width: 1%; text-align: left; vertical-align: top'>{data.kd_mtu_symbol_tsi} {nilai_tsi_pst}
                                <td style='width: 1%; text-align: left; vertical-align: top;'>{data.kd_mtu_symbol} {nilai_tsi_pst_idr}</td>
                                <td style='width: 1%; vertical-align: top; text-align: left;'>{data.kd_mtu_symbol} {nilai_ttl_kl}</td>
                                <td style='width: 1%; vertical-align: top; text-align: left;'>{data.kd_mtu_symbol} {nilai_ttl_kl_idr}</td>
                                <td style='vertical-align: top;'>{data.nm_sifat_kerugian}</td>
                            </tr>");
                        total_nilai_tsi_pst += ReportHelper.ConvertToDecimalFormat(nilai_tsi_pst);
                        total_nilai_tsi += ReportHelper.ConvertToDecimalFormat(nilai_tsi);
                        total_nilai_tsi_pst_idr += ReportHelper.ConvertToDecimalFormat(nilai_tsi_pst_idr);
                        total_nilai_ttl_kl += ReportHelper.ConvertToDecimalFormat(nilai_ttl_kl);
                        total_nilai_ttl_kl_idr += ReportHelper.ConvertToDecimalFormat(nilai_ttl_kl_idr);
                    }

                    stringBuilder.Append($@"
                    <tr>
                        <td></td>
                        <td style='font-weight: bold; text-align: right; padding-right: 50px;' colspan='4'>TOTAL : </td>
                        <td style='font-weight: bold; text-align: left; vertical-align: top;'>{innerFirst.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(total_nilai_tsi_pst)}</td>
                        <td style='font-weight: bold; text-align: left; vertical-align: top;'>{innerFirst.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(total_nilai_tsi_pst_idr)}</td>
                        <td style='font-weight: bold; text-align: left; vertical-align: top;'>{innerFirst.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(total_nilai_ttl_kl)}</td>
                        <td style='font-weight: bold; text-align: left; vertical-align: top;'>{innerFirst.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(total_nilai_ttl_kl_idr)}</td>
                        <td style='font-weight: bold; text-align: right; vertical-align: top;'></td>
                    </tr>");

                    if (!outerGroup.Key.Equals(lastOuterKey) || !innerGroup.Key.Equals(lastInnerKey))
                    {
                        // append - i.e. if outer isn't last OR inner isn't last
                        stringBuilder.Append("</table></div></div>");
                    }
                }
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}