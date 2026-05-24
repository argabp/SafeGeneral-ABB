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

namespace ABB.Application.CetakRekapitulasiProduksiPremiFakultatifKeluars.Commands
{
    public class CetakRekapitulasiProduksiPremiFakultatifKeluarCommand : IRequest<string>
    {
        public DateTime periode { get; set; }
        
        public string jns_lap { get; set; }
    }

    public class CetakRekapitulasiProduksiPremiFakultatifKeluarCommandHandler : IRequestHandler<CetakRekapitulasiProduksiPremiFakultatifKeluarCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakRekapitulasiProduksiPremiFakultatifKeluarCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakRekapitulasiProduksiPremiFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            var spName = string.Empty;
            switch (request.jns_lap)
            {
                case "1":
                    spName = "spr_ri06r_02";
                    break;
                case "2":
                    //spName = "spr_ri06r_02_01";
                    break;
            }
            
            var datas = (await _connectionPst.QueryProc<CetakRekapitulasiProduksiPremiFakultatifKeluarModel>(spName, 
                new
                {
                    input_str = $"{request.periode:yyyy/MM/dd}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "RekapitulasiProduksiPremiFakultatifKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            decimal total_nilai_prm = 0;
            decimal total_nilai_prm_bl = 0;
            decimal total_nilai_kms = 0;
            decimal total_nilai_kms_bl = 0;
            decimal total_net_prm = 0;
            decimal total_net_prm_bl = 0;

            stringBuilder.Append($@"<table class='table'>
                            <tr>
                                <td style='width: 3%; text-align: center; border: 1px solid'>NO</td>
                                <td style='text-align: center; border: 1px solid'>JENIS BISNIS</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 12%;  text-align: center; border: 1px solid'>KOMISI</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>NETT PREMI</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>s/d BULAN INI</td>
                            </tr>");

            var sequence = 1;
                
            foreach (var data in datas)
            {
                var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                var nilai_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl);
                var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                var nilai_kms_bl = ReportHelper.ConvertToReportFormat(data.nilai_kms_bl);
                var net_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm - data.nilai_kms);
                var net_prm_bl = ReportHelper.ConvertToReportFormat(data.nilai_prm_bl - data.nilai_kms_bl);
                
                stringBuilder.Append(@$"<tr>
                                            <td style='text-align: center; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{sequence}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.nm_cob_ing}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_prm_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kms_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{net_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{net_prm_bl}</td>
                                        </tr>");
                
                sequence++;
                total_nilai_prm += ReportHelper.ConvertToDecimalFormat(nilai_prm);
                total_nilai_prm_bl += ReportHelper.ConvertToDecimalFormat(nilai_prm_bl);
                total_nilai_kms += ReportHelper.ConvertToDecimalFormat(nilai_kms);
                total_nilai_kms_bl += ReportHelper.ConvertToDecimalFormat(nilai_kms_bl);
                total_net_prm += ReportHelper.ConvertToDecimalFormat(net_prm);
                total_net_prm_bl += ReportHelper.ConvertToDecimalFormat(net_prm_bl);
            }
                
            stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'></td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>Total</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_prm_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_nilai_kms_bl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{total_net_prm_bl}</td>
                                        </tr>");
            
            stringBuilder.Append("</table>");

            var firstData = datas.FirstOrDefault();
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(),
                periode = request.periode.ToString("MMMM yyyy"),
                firstData?.nm_bag, firstData?.nm_kpl_bag
            } );
            
            return resultTemplate;
        }
    }
}