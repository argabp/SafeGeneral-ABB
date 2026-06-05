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

            var outerGroups = datas.Select(s => s.nm_rk_pas + s.ket_tr).Distinct();

            foreach (var outerGroup in outerGroups)
            {
                var firstData = datas.First(w => w.nm_rk_pas + w.ket_tr == outerGroup);
                var no_ref = string.IsNullOrWhiteSpace(firstData.ket_tr) ? string.Empty : "No Ref: " + firstData.ket_tr;
                stringBuilder.Append($@"
                        <div style='page-break-before: always;'>
                            <div class='container'>
                                <div class='section'>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>REASURANSI TREATY MASUK</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>CEDING COMPANY: {firstData.nm_rk_pas}</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>TECHNICAL ACCOUNT STATEMENT TRIWULAN / TAHUN : {request.kuartal_tr} / {request.thn_tr}</strong></p>
                                    
                                    <p style='font-size: 14px; margin: auto; text-align: left; padding-top: 3em'><strong>{no_ref}</strong></p>
                                    <table class='table'>
                                        <tr>
                                            <td style='text-align: center; border: 1px solid'>NOMOR NOTA</td>
                                            <td style='text-align: center; border: 1px solid'>TIPE BISNIS / TREATY</td>
                                            <td style='width: 3%; text-align: center; border: 1px solid'>KURS</td>
                                            <td style='width: 15%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                            <td style='width: 15%;  text-align: center; border: 1px solid'>KOMISI R/I</td>
                                            <td style='width: 15%; text-align: center; border: 1px solid'>KLAIM</td>
                                            <td style='width: 15%; text-align: center; border: 1px solid'>BAGIAN PAS</td>
                                        </tr>
                                    ");
                    
                foreach (var data in datas.Where(w => w.nm_rk_pas + w.ket_tr == outerGroup))
                {
                    var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                    var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                    var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                    var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                    var borderStyle = string.IsNullOrWhiteSpace(data.no_nota)
                        ? "border: 1px solid"
                        : "border-right: 1px solid; border-left: 1px solid;";
                    
                    stringBuilder.Append(@$"<tr>
                                                <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.no_nota}</td>
                                                <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.desk_tty}</td>
                                                <td style='width: 3%; text-align: center; vertical-align: top; {borderStyle}'>{data.symbol}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kms}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kl}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_share_bgu}</td>
                                            </tr>");
                }
                
                stringBuilder.Append(@"</table>
                                        <table class='table' style='margin-top: 3rem'>
                                        <tr>
                                            <td style='text-align: right'><strong>Departemen Reasuransi</strong></td>
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