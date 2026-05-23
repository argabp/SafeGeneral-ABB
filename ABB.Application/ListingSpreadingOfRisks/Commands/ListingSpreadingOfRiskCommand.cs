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

        public string almt_rsk { get; set; }
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
                    input_str = $"{request.kd_cb}, {request.kd_cob}, {request.tgl_mul:yyyy/MM/dd}, {request.tgl_akh:yyyy/MM/dd}, {request.no_pol_ttg}, {request.almt_rsk}"
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
            
            decimal total_nilai_ttl_ptg_reas = 0;
            decimal total_pst_share_reas = 0;
            decimal total_nilai_prm_reas = 0;
            decimal total_pst_adj_reas = 0;
            decimal total_nilai_adj_reas = 0;
            decimal total_pst_kms_reas = 0;
            decimal total_nilai_kms_reas = 0;

            stringBuilder.Append($@"<table class='table'>
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
                
            foreach (var data in datas)
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
            
            var firstData = datas.First();

            stringBuilder.Append(@$"<tr>
                                        <td style='width: 10%; text-align: left; vertical-align: top;'></td>
                                        <td style='text-align: left; vertical-align: top;'></td>
                                        <td style='text-align: left; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{firstData.symbol}</td>
                                        <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_nilai_ttl_ptg_reas}</td>
                                        <td style='width: 8%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_pst_share_reas}</td>
                                        <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_nilai_prm_reas}</td>
                                        <td style='width: 8%;  text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_pst_adj_reas}</td>
                                        <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_nilai_adj_reas}</td>
                                        <td style='width: 8%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_pst_kms_reas}</td>
                                        <td style='width: 12%; text-align: right; vertical-align: top; border-top: 1px solid; border-bottom: 1px solid;'>{total_nilai_kms_reas}</td>
                                    </tr>");
            
            stringBuilder.Append("</table>");

            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(), tgl_mul = request.tgl_mul.ToString("dd MMM yyyy"),
                tgl_akh = request.tgl_akh.ToString("dd MMM yyyy"), firstData.nm_cob, firstData.no_pol_ttg,
                firstData.nm_ttg, firstData.nm_scob, firstData.tgl_closing_id, firstData.no_rsk, firstData.kd_endt,
                firstData.pst_cvrg, firstData.almt_rsk, firstData.nm_tol, firstData.nm_mtu, 
                nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(firstData.nilai_ttl_ptg),
                firstData.pst_rate_prm, firstData.stn_rate_prm_rsk, firstData.tgl_mul_ptg_ind, firstData.tgl_akh_ptg_ind,
                firstData.kd_okup, firstData.kd_kls_konstr, firstData.nm_kpl_bag, firstData.nm_bag,
                nilai_prm_rsk = ReportHelper.ConvertToReportFormat(firstData.nilai_prm_rsk), 
                nilai_dis_rsk = ReportHelper.ConvertToReportFormat(firstData.nilai_dis_rsk),
                nilai_prm_rsk_net = ReportHelper.ConvertToReportFormat(firstData.nilai_prm_rsk_net)
            } );
            
            return resultTemplate;
        }
    }
}