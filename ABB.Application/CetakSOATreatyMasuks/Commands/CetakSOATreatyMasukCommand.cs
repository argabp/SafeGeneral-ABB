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

namespace ABB.Application.CetakSOATreatyMasuks.Commands
{
    public class CetakSOATreatyMasukCommand : IRequest<string>
    {
        public string kd_cb { get; set; }
        
        public string kuartal_tr { get; set; }

        public string thn_tr { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }
    }

    public class CetakSOATreatyMasukCommandHandler : IRequestHandler<CetakSOATreatyMasukCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakSOATreatyMasukCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakSOATreatyMasukCommand request, CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<CetakSOATreatyMasukModel>("spr_ri16r_01", 
                new
                {
                    input_str = $"{request.kd_cb}, {request.thn_tr}, {request.kuartal_tr}, {request.kd_grp_pas}, {request.kd_rk_pas}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "SOATreatyMasuk.html" );
            
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
                                <td style='width: 15%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                <td style='width: 15%;  text-align: center; border: 1px solid'>KOMISI R/I</td>
                                <td style='width: 15%; text-align: center; border: 1px solid'>KLAIM</td>
                                <td style='width: 15%; text-align: center; border: 1px solid'>BAGIAN PAS</td>
                            </tr>");
                
            foreach (var data in datas)
            {
                var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                
                stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>{data.no_nota}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;'>{data.desk_tty}</td>
                                            <td style='width: 3%; text-align: center; vertical-align: top; border: 1px solid;'>{data.symbol}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kms}</td>
                                            <td style='width: 10%;  text-align: right; vertical-align: top; border: 1px solid;'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid;'>{nilai_share_bgu}</td>
                                        </tr>");
            }
            
            stringBuilder.Append("</table>");

            var firstData = datas.FirstOrDefault();
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(), request.thn_tr,
                firstData?.nm_rk_pas, request.kuartal_tr
            } );
            
            return resultTemplate;
        }
    }
}