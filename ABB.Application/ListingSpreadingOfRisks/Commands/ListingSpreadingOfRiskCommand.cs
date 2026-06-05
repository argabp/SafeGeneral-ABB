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

namespace ABB.Application.ListingSpreadingOfRisks.Commands
{
    public class ListingSpreadingOfRiskCommand : IRequest<string>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }

        public string no_pol_ttg { get; set; }
    }

    public class ListingSpreadingOfRiskCommandHandler : IRequestHandler<ListingSpreadingOfRiskCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public ListingSpreadingOfRiskCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(ListingSpreadingOfRiskCommand request, CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<ListingSpreadingOfRiskModel>("spr_ri32r_01", 
                new
                {
                    input_str = $"{request.kd_cb}, {request.kd_cob}, {request.tgl_mul:yyyy/MM/dd}, {request.tgl_akh:yyyy/MM/dd}, {request.no_pol_ttg},"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "ListingSpreadingOfRisk.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            var outerGroups = datas.Select(w => w.kd_cob).Distinct();

            foreach (var outerGroup in outerGroups)
            {
                var firstData = datas.First(w => w.kd_cob == outerGroup);
                stringBuilder.Append($@"
                    <div style='page-break-before: always;'>
                        <div class='container'>
                            <div class='section'>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>LISTING ALOKASI REASURANSI</strong></p>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>CLASS OF BUSINESS {firstData.nm_cob}</strong></p>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>Periode {request.tgl_mul:dd MMM yyyy} s/d {request.tgl_akh:dd MMM yyyy}</strong></p>
                                <br>

                                <table class='table'>
                                    <tr>
                                        <td style=''><strong>Nomor Polis</strong></td>
                                        <td>:</td>
                                        <td style='' colspan='6'><strong>{firstData.no_pol_ttg}</strong></td>
                                    </tr>
                                    <tr>
                                        <td style='width: 10%'>Nama Tertanggung</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 46%'>{firstData.nm_ttg}</td>
                                        <td style='width: 10%'>Sub COB</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 10%'>{firstData.nm_scob}</td>
                                        <td style='width: 10%'>Tanggal Closing</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 10%'>{firstData.tgl_closing_id}</td>
                                    </tr>
                                </table>");

                var innerGroup = datas.Where(w => w.kd_cob == outerGroup)
                    .Select(s => s.no_updt_reas.Trim() + s.no_pol_ttg.Trim()).Distinct();
                
                foreach (var innerData in innerGroup)
                {
                    decimal total_nilai_ttl_ptg_reas = 0;
                    decimal total_pst_share_reas = 0;
                    decimal total_nilai_prm_reas = 0;
                    decimal total_pst_adj_reas = 0;
                    decimal total_nilai_adj_reas = 0;
                    decimal total_pst_kms_reas = 0;
                    decimal total_nilai_kms_reas = 0;

                    var firstInnerGroupData = datas.First(w =>
                        w.kd_cob == outerGroup && w.no_updt_reas.Trim() + w.no_pol_ttg.Trim() == innerData);

                    stringBuilder.Append($@"
                                <table>
                                    <tr>
                                        <td style='width: 10%'>Nomor Resiko</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 47%'>{firstInnerGroupData.no_rsk} / Kode Endorsment : {firstInnerGroupData.kd_endt}</td>
                                        <td style='width: 10%'>Mata Uang</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 10%'>{firstInnerGroupData.nm_mtu}</td>
                                        <td style='width: 10%'>Premi</td>
                                        <td style='width: 1%'>:</td>
                                        <td style='width: 10%'>{ReportHelper.ConvertToReportFormat(firstInnerGroupData.nilai_prm_rsk)}</td>
                                    </tr>
                                    <tr>
                                        <td style=''>Coverage</td>
                                        <td>:</td>
                                        <td style='vertical-align: top;' rowspan='2'>{firstInnerGroupData.pst_cvrg}</td>
                                        <td style=''>Total Pertanggungan</td>
                                        <td>:</td>
                                        <td style=''>{ReportHelper.ConvertToReportFormat(firstInnerGroupData.nilai_ttl_ptg)}</td>
                                        <td style=''>Diskon</td>
                                        <td>:</td>
                                        <td style=''>{ReportHelper.ConvertToReportFormat(firstInnerGroupData.nilai_dis_rsk)}</td>
                                    </tr>
                                    <tr>
                                        <td style=''></td>
                                        <td></td>
                                        <td style=''>Rate Premi</td>
                                        <td>:</td>
                                        <td style=''>{ReportHelper.ConvertToReportFormat(firstInnerGroupData.pst_rate_prm_rsk)} {ReportHelper.ConvertSatuanType(firstInnerGroupData.stn_rate_prm_rsk)}</td>
                                        <td style=''>Nett Premi</td>
                                        <td>:</td>
                                        <td style=''>{ReportHelper.ConvertToReportFormat(firstInnerGroupData.nilai_prm_rsk_net)}</td>
                                    </tr>
                                    <tr>
                                        <td style=''>Alamat Resiko</td>
                                        <td>:</td>
                                        <td style='vertical-align: top;' rowspan='2'>{firstInnerGroupData.almt_rsk}</td>
                                        <td style=''>Jangka Waktu Pertanggungan</td>
                                        <td>:</td>
                                        <td style='width: 15%'>{firstInnerGroupData.tgl_mul_ptg_ind} s/d {firstInnerGroupData.tgl_akh_ptg_ind}</td>
                                        <td style='' colspan='3'>{firstInnerGroupData.ket_rsk}</td>
                                    </tr>
                                    <tr>
                                        <td style=''></td>
                                        <td></td>
                                        <td style=''>Kode Okupasi</td>
                                        <td>:</td>
                                        <td style=''>{firstInnerGroupData.kd_okup}</td>
                                        <td style=''></td>
                                        <td></td>
                                        <td style=''></td>
                                    </tr>
                                    <tr>
                                        <td style=''>Table Of Limit</td>
                                        <td>:</td>
                                        <td style='vertical-align: top;' rowspan='2'>{firstInnerGroupData.nm_tol}</td>
                                        <td style=''>Kelas Konstruksi</td>
                                        <td>:</td>
                                        <td style=''>{firstInnerGroupData.kd_kls_konstr}</td>
                                        <td style='' colspan='3'></td>
                                    </tr>
                                    <tr>
                                        <td style=''></td>
                                        <td></td>
                                        <td style=''></td>
                                        <td>:</td>
                                        <td style=''></td>
                                        <td style=''></td>
                                        <td></td>
                                        <td style=''></td>
                                    </tr>
                                </table>
                                <table class='table'>
                                    <tr>
                                        <td style='width: 10%; text-align: center; border: 1px solid'>Kode Jenis SOR</td>
                                        <td style='text-align: center; border: 1px solid'>S O R</td>
                                        <td style='width: 12%; text-align: center; border: 1px solid' colspan='2'>T S I</td>
                                        <td style='width: 8%; text-align: center; border: 1px solid'>Share</td>
                                        <td style='width: 12%;  text-align: center; border: 1px solid'>Premi</td>
                                        <td style='width: 8%; text-align: center; border: 1px solid'>Pst %</td>
                                        <td style='width: 12%; text-align: center; border: 1px solid'>Adjustment</td>
                                        <td style='width: 8%; text-align: center; border: 1px solid'>Pst %</td>
                                        <td style='width: 12%; text-align: center; border: 1px solid'>Komisi</td>
                                    </tr>");
                        
                    foreach (var data in datas.Where(w => w.kd_cob == outerGroup && w.no_updt_reas.Trim() + w.no_pol_ttg.Trim() == innerData))
                    {
                        var nilai_ttl_ptg_reas = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg_reas);
                        var pst_share_reas = ReportHelper.ConvertToReportFormat(data.pst_share_reas);
                        var nilai_prm_reas = ReportHelper.ConvertToReportFormat(data.nilai_prm_reas);
                        var pst_adj_reas = ReportHelper.ConvertToReportFormat(data.pst_adj_reas);
                        var nilai_adj_reas = ReportHelper.ConvertToReportFormat(data.nilai_adj_reas);
                        var pst_kms_reas = ReportHelper.ConvertToReportFormat(data.pst_kms_reas);
                        var nilai_kms_reas = ReportHelper.ConvertToReportFormat(data.nilai_kms_reas);
                        
                        stringBuilder.Append(@$"<tr>
                                                    <td style='width: 10%; text-align: left; vertical-align: top;'>{data.kd_jns_sor}</td>
                                                    <td style='text-align: left; vertical-align: top;'>{data.nm_tty_pps}</td>
                                                    <td style='text-align: left; vertical-align: top;'>{data.symbol}</td>
                                                    <td style='width: 12%; text-align: right; vertical-align: top;'>{nilai_ttl_ptg_reas}</td>
                                                    <td style='width: 8%; text-align: right; vertical-align: top;'>{pst_share_reas}</td>
                                                    <td style='width: 12%; text-align: right; vertical-align: top;'>{nilai_prm_reas}</td>
                                                    <td style='width: 8%;  text-align: right; vertical-align: top;'>{pst_adj_reas}</td>
                                                    <td style='width: 12%; text-align: right; vertical-align: top;'>{nilai_adj_reas}</td>
                                                    <td style='width: 8%;  text-align: right; vertical-align: top;'>{pst_kms_reas}</td>
                                                    <td style='width: 12%; text-align: right; vertical-align: top;'>{nilai_kms_reas}</td>
                                                </tr>");
                        
                        
                        total_nilai_ttl_ptg_reas += ReportHelper.ConvertToDecimalFormat(nilai_ttl_ptg_reas);
                        total_pst_share_reas += ReportHelper.ConvertToDecimalFormat(pst_share_reas);
                        total_nilai_prm_reas += ReportHelper.ConvertToDecimalFormat(nilai_prm_reas);
                        total_pst_adj_reas += ReportHelper.ConvertToDecimalFormat(pst_adj_reas);
                        total_nilai_adj_reas += ReportHelper.ConvertToDecimalFormat(nilai_adj_reas);
                        total_pst_kms_reas += ReportHelper.ConvertToDecimalFormat(pst_kms_reas);
                        total_nilai_kms_reas += ReportHelper.ConvertToDecimalFormat(nilai_kms_reas);
                    }

                    stringBuilder.Append(@$"<tr>
                                                <td style='width: 10%; text-align: left; vertical-align: top;'></td>
                                                <td style='text-align: left; vertical-align: top;'></td>
                                                <td style='text-align: left; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{firstData.symbol}</td>
                                                <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_nilai_ttl_ptg_reas)}</td>
                                                <td style='width: 8%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_pst_share_reas)}</td>
                                                <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_nilai_prm_reas)}</td>
                                                <td style='width: 8%;  text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_pst_adj_reas)}</td>
                                                <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_nilai_adj_reas)}</td>
                                                <td style='width: 8%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_pst_kms_reas)}</td>
                                                <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_nilai_kms_reas)}</td>
                                            </tr>");
                    
                    stringBuilder.Append("</table>");
                }

                stringBuilder.Append($@"

                                <table class='table' style='margin-top: 7rem'>
                                    <tr>
                                        <td style='width:80%'></td>
                                        <td style='text-align: center'><strong>{firstData.nm_bag}</strong></td>
                                    </tr>
                                    <tr>
                                        <td style='width:80%'></td>
                                        <td style='text-align: center'><strong>{firstData.nm_kpl_bag}</strong></td>
                                    </tr>
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