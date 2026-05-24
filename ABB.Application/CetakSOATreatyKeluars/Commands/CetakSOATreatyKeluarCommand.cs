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

namespace ABB.Application.CetakSOATreatyKeluars.Commands
{
    public class CetakSOATreatyKeluarCommand : IRequest<string>
    {
        public string kd_cb { get; set; }
        
        public string kuartal_tr { get; set; }

        public string thn_tr { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string jns_laporan { get; set; }
    }

    public class CetakSOATreatyKeluarCommandHandler : IRequestHandler<CetakSOATreatyKeluarCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakSOATreatyKeluarCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakSOATreatyKeluarCommand request, CancellationToken cancellationToken)
        {
            var spName = string.Empty;
            
            switch (request.jns_laporan)
            {
                case "1":
                    spName = "spr_ri17r_01";
                    break;
                case "2":
                    spName = "spr_ri17r_02";
                    break;
            }
            
            var datas = (await _connectionPst.QueryProc<CetakSOATreatyKeluarModel>(spName, 
                new
                {
                    input_str = $"{request.kd_cb}, {request.thn_tr}, {request.kuartal_tr}, {request.kd_grp_pas}, {request.kd_rk_pas}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "SOATreatyKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($@"<table class='table'>
                            <tr>
                                <td style='text-align: center; border: 1px solid'>NOMOR NOTA</td>
                                <td style='text-align: center; border: 1px solid'>TIPE BISNIS / TREATY</td>
                                <td style='width: 3%; text-align: center; border: 1px solid'>KURS</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                <td style='width: 12%;  text-align: center; border: 1px solid'>KOMISI R/I</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>KLAIM</td>
                                <td style='width: 12%; text-align: center; border: 1px solid'>BAGIAN PAS</td>
                            </tr>");

            var firstData = datas.First();
            
            switch (request.jns_laporan)
            {
                case "1":
                    foreach (var data in datas)
                    {
                        var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                        var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                        var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                        var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                
                        stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.no_nt}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.desk_tty}</td>
                                            <td style='width: 3%; text-align: center; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.symbol}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_share_bgu}</td>
                                        </tr>");
                    }
                    break;
                case "2":
                    var thn_uws = datas.Select(s => s.thn_uw).Distinct().ToList();
                    
                    foreach (var thn_uw in thn_uws)
                    {
                        stringBuilder.Append($@"<tr>
                                    <td style='text-align: left; border: 1px solid' colspan='7'>U/Y {thn_uw}</td>
                                </tr>");

                        foreach (var data in datas)
                        {
                            var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                            var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                            var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                            var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                
                            stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.no_nt}</td>
                                            <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.desk_tty}</td>
                                            <td style='width: 3%; text-align: center; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{data.symbol}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid;'>{nilai_share_bgu}</td>
                                        </tr>");
                        }
                    }
                    break;
            }
            
            stringBuilder.Append(@$"<tr>
                                        <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='text-align: left; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='width: 3%; text-align: center; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                        <td style='width: 10%; text-align: right; vertical-align: top; border-right: 1px solid; border-left: 1px solid; border-bottom: 1px solid;'></td>
                                    </tr>");
            
            stringBuilder.Append("</table>");
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(), request.thn_tr,
                firstData.nm_rk_pas, request.kuartal_tr
            } );
            
            return resultTemplate;
        }
    }
}