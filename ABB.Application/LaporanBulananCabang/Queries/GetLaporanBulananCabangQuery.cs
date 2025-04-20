using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.RekapitulasiProduksi.Quries;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.LaporanBulananCabang.Queries
{
    public class GetLaporanBulananCabangQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public DateTime kd_bln_mul { get; set; }
        public DateTime kd_bln_akh { get; set; }
        
        public string jns_lap { get; set; }
    }

    public class GetLaporanBulananCabangQueryHandler : IRequestHandler<GetLaporanBulananCabangQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLaporanBulananCabangQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanBulananCabangQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var laporanBulananCabangDatas = (await _connectionFactory.QueryProc<LaporanBulananCabangDto>("spr_lap_bulanan", 
                new
                {
                    input_str = $"{request.kd_bln_mul:yyyy/MM/dd},{request.kd_bln_akh:yyyy/MM/dd}," +
                                $"{request.kd_cb.Trim()},{request.jns_lap.Trim()}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanBulananCabang.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (laporanBulananCabangDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            decimal total_premi_01 = 0;
            decimal total_premi_02 = 0;
            decimal total_premi_03 = 0;
            decimal total_premi_04 = 0;
            decimal total_premi_05 = 0;
            decimal total_premi_06 = 0;
            decimal total_premi_07 = 0;
            decimal total_premi_08 = 0;
            decimal total_premi_09 = 0;
            decimal total_premi_10 = 0;
            decimal total_premi_11 = 0;
            decimal total_premi_12 = 0;
            decimal total_nilai_prm_ttl = 0;
            StringBuilder totalText = new StringBuilder();

            var st_pass = laporanBulananCabangDatas.Select(s => s.st_pas).Distinct().ToList();
            
            stringBuilder.Append($@"<table class='table'>
                            <tr>
                                <td style='width: 20%; text-align: center; border: 1px solid'>COB</td>
                                <td style='width: 20%; text-align: center; border: 1px solid'>JANUARI</td>
                                <td style='width: 20%; text-align: center; border: 1px solid'>FEBRUARI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>MARET</td>
                                <td style='width: 5%;  text-align: center; border: 1px solid'>APRIL</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>MEI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>JUNI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>JULI</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>AGUSTUS</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>SEPTEMBER</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>OKTOBER</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>NOVEMBER</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>DESEMBER</td>
                                <td style='width: 10%; text-align: center; border: 1px solid'>TOTAL</td>
                            </tr>");

            var sequence = "I";
            foreach (var st_pas in st_pass)
            {
                stringBuilder.Append($@"<tr>
                                            <td colspan='10' style='border: 1px solid'>{st_pas}</td>
                                        </tr>");
                
                foreach (var data in laporanBulananCabangDatas.Where(w => w.st_pas == st_pas))
                {
                    var nilai_prm_01 = ReportHelper.ConvertToReportFormat(data.nilai_prm_01);
                    var nilai_prm_02 = ReportHelper.ConvertToReportFormat(data.nilai_prm_02);
                    var nilai_prm_03 = ReportHelper.ConvertToReportFormat(data.nilai_prm_03);
                    var nilai_prm_04 = ReportHelper.ConvertToReportFormat(data.nilai_prm_04);
                    var nilai_prm_05 = ReportHelper.ConvertToReportFormat(data.nilai_prm_05);
                    var nilai_prm_06 = ReportHelper.ConvertToReportFormat(data.nilai_prm_06);
                    var nilai_prm_07 = ReportHelper.ConvertToReportFormat(data.nilai_prm_07);
                    var nilai_prm_08 = ReportHelper.ConvertToReportFormat(data.nilai_prm_08);
                    var nilai_prm_09 = ReportHelper.ConvertToReportFormat(data.nilai_prm_09);
                    var nilai_prm_10 = ReportHelper.ConvertToReportFormat(data.nilai_prm_10);
                    var nilai_prm_11 = ReportHelper.ConvertToReportFormat(data.nilai_prm_11);
                    var nilai_prm_12 = ReportHelper.ConvertToReportFormat(data.nilai_prm_12);
                    var nilai_prm_ttl = ReportHelper.ConvertToReportFormat(data.nilai_prm_ttl);
                    
                    stringBuilder.Append(@$"<tr>
                                                <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid;'>{data.nm_cob}</td>
                                                <td style='width: 20%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_01}</td>
                                                <td style='width: 20%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_02}</td>
                                                <td style='width: 20%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_03}</td>
                                                <td style='width: 20%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_04}</td>
                                                <td style='width: 5%;  text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_05}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_06}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_07}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_08}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_09}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_10}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_11}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_12}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm_ttl}</td>
                                            </tr>");
                }
            }
            
            stringBuilder.Append("</table>");
            
            var laporanBulananCabang = laporanBulananCabangDatas.FirstOrDefault();
            resultTemplate = templateProfileResult.Render( new
            {
                laporanBulananCabang?.nm_cab, 
                kd_bln_mul = request.kd_bln_mul.ToString("dd-MM-yyyy"),
                kd_bln_akh = request.kd_bln_akh.ToString("dd-MM-yyyy"),
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}