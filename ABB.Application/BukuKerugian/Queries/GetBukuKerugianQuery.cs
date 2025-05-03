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

        public GetBukuKerugianQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
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

            var bukuKerugianData = bukuKerugianDatas.FirstOrDefault();

            StringBuilder stringBuilder = new StringBuilder();
            var sequence = 0;
            decimal total_nilai_tsi_pst = 0;
            decimal total_nilai_tsi = 0;
            decimal total_nilai_tsi_pst_idr = 0;
            decimal total_nilai_tsi_idr = 0;
            decimal total_nilai_ttl_kl = 0;
            decimal total_nilai_ttl_kl_idr = 0;
            foreach (var data in bukuKerugianDatas)
            {
                sequence++;
                var nilai_tsi_pst = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu / data.pst_share_bgu);
                var nilai_tsi = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                var nilai_tsi_pst_idr = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu_idr / data.pst_share_bgu);
                var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl);
                var nilai_ttl_kl_idr = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl_idr);
                var pst_share_bgu = ReportHelper.ConvertToReportFormat(data.pst_share_bgu, true);
                stringBuilder.Append(@$"<tr>
                                            <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_tsi_pst}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_tsi}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_tsi_pst_idr}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_ttl_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_ttl_kl_idr}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{pst_share_bgu}</td>
                                        </tr>");
                total_nilai_tsi_pst += ReportHelper.ConvertToDecimalFormat(nilai_tsi_pst);
                total_nilai_tsi += ReportHelper.ConvertToDecimalFormat(nilai_tsi);
                total_nilai_tsi_pst_idr += ReportHelper.ConvertToDecimalFormat(nilai_tsi_pst_idr);
                total_nilai_tsi_idr += ReportHelper.ConvertToDecimalFormat(nilai_ttl_kl);
                total_nilai_ttl_kl += ReportHelper.ConvertToDecimalFormat(nilai_ttl_kl_idr);
                total_nilai_ttl_kl_idr += ReportHelper.ConvertToDecimalFormat(pst_share_bgu);
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                total_nilai_tsi_pst = ReportHelper.ConvertToReportFormat(total_nilai_tsi_pst), 
                total_nilai_tsi = ReportHelper.ConvertToReportFormat(total_nilai_tsi), 
                total_nilai_tsi_pst_idr = ReportHelper.ConvertToReportFormat(total_nilai_tsi_pst_idr),
                total_nilai_tsi_idr = ReportHelper.ConvertToReportFormat(total_nilai_tsi_idr), 
                total_nilai_ttl_kl = ReportHelper.ConvertToReportFormat(total_nilai_ttl_kl), 
                total_nilai_ttl_kl_idr = ReportHelper.ConvertToReportFormat(total_nilai_ttl_kl_idr),
                details = stringBuilder.ToString(),
                bukuKerugianData.no_pol_ttg, bukuKerugianData.nm_mtu, bukuKerugianData.nm_cb,
                bukuKerugianData.nm_cob, tgl_mul = bukuKerugianData.tgl_mul.Value.ToString("dd MMMM yyyy"),
                tgl_akh = bukuKerugianData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}