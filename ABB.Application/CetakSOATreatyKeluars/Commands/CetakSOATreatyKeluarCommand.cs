using System;
using System.Collections.Generic;
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
            IEnumerable<string> outerGroups;
            
            switch (request.jns_laporan)
            {
                case "1":
                    outerGroups = datas.Select(s => s.kd_rk_pas).Distinct();
                    foreach (var outerData in outerGroups)
                    {
                        var firstDataOuter = datas.First(s => s.kd_rk_pas == outerData);
                        
                        stringBuilder.Append($@"
                        <div style='page-break-before: always;'>
                            <div class='container'>
                                <div class='section'>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>REASURANSI TREATY KELUAR</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>CEDING COMPANY: {firstDataOuter.nm_rk_pas}</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>TECHNICAL ACCOUNT STATEMENT TRIWULAN / TAHUN : {request.kuartal_tr} / {request.thn_tr}</strong></p>

                                    <table class='table'>
                                        <tr>
                                            <td style='text-align: center; border: 1px solid'>NOMOR NOTA</td>
                                            <td style='text-align: center; border: 1px solid'>TIPE BISNIS / TREATY</td>
                                            <td style='width: 3%; text-align: center; border: 1px solid'>KURS</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                            <td style='width: 10%;  text-align: center; border: 1px solid'>KOMISI R/I</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>KLAIM</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>BAGIAN PAS</td>
                                        </tr>
                                    ");
                        foreach (var data in datas.Where(s => s.kd_rk_pas == outerData))
                        {
                            var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                            var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                            var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                            var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                            var borderStyle = string.IsNullOrWhiteSpace(data.no_nt)
                                ? "border: 1px solid"
                                : "border-right: 1px solid; border-left: 1px solid;";
                
                            stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.no_nt}</td>
                                            <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.desk_tty}</td>
                                            <td style='width: 3%; text-align: center; vertical-align: top; {borderStyle}'>{data.symbol}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_share_bgu}</td>
                                        </tr>");
                        }
                
                        stringBuilder.Append($@"</table>
                                    <table class='table' style='margin-top: 50px; width: 100%;'>
                                        <tr>
                                            <td style='width: 60%;'></td>
                                            <td style='text-align: center; width: 40%; font-size: 12px;'>Jakarta {firstDataOuter.tgl_soa:dd MMMM yyyy}</td>
                                        </tr>
                                        <tr>
                                            <td style='width: 60%;'></td>
                                            <td style='text-align: center; width: 40%; font-size: 12px;'>S.E & O</td>
                                        </tr>
                                        <tr style='height: 75px;'>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style='text-align: center; line-height: 1.3;'>
                                                <strong><u>{firstDataOuter.nm_bag}</u></strong><br>
                                                <span style='font-size: 11px; text-decoration: underline;'>{firstDataOuter.nm_kpl_bag}</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>");
                    }
                    break;
                case "2":
                    outerGroups = datas.Select(s => s.kd_rk_pas).Distinct();
                    foreach (var outerData in outerGroups)
                    {
                        var firstDataOuter = datas.First(s => s.kd_rk_pas == outerData);
                        
                        stringBuilder.Append($@"
                        <div style='page-break-before: always;'>
                            <div class='container'>
                                <div class='section'>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>REASURANSI TREATY KELUAR</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>CEDING COMPANY: {firstDataOuter.nm_rk_pas}</strong></p>
                                    <p style='font-size: 14px; margin: auto; text-align: center;'><strong>TECHNICAL ACCOUNT STATEMENT TRIWULAN / TAHUN : {request.kuartal_tr} / {request.thn_tr}</strong></p>

                                    <table class='table'>
                                        <tr>
                                            <td style='text-align: center; border: 1px solid'>NOMOR NOTA</td>
                                            <td style='text-align: center; border: 1px solid'>TIPE BISNIS / TREATY</td>
                                            <td style='width: 3%; text-align: center; border: 1px solid'>KURS</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>GROSS PREMI</td>
                                            <td style='width: 10%;  text-align: center; border: 1px solid'>KOMISI R/I</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>KLAIM</td>
                                            <td style='width: 10%; text-align: center; border: 1px solid'>BAGIAN PAS</td>
                                        </tr>
                                    ");
                        
                        var innerGroups = datas.Where(s => s.kd_rk_pas == outerData)
                            .Select(s => s.thn_uw).Distinct();

                        foreach (var innerData in innerGroups)
                        {
                            var firstDataInner = datas.First(s => s.kd_rk_pas == outerData && s.thn_uw == innerData);
                            stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid;' colspan=7>U/Y {firstDataInner.thn_uw}</td>
                                        </tr>");
                            
                            foreach (var data in datas.Where(s => s.kd_rk_pas == outerData && s.thn_uw == innerData))
                            {
                                var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                                var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                                var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
                                var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                                var borderStyle = string.IsNullOrWhiteSpace(data.no_nt)
                                    ? "border: 1px solid"
                                    : "border-right: 1px solid; border-left: 1px solid;";
                
                                stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.no_nt}</td>
                                            <td style='text-align: left; vertical-align: top; {borderStyle}'>{data.desk_tty}</td>
                                            <td style='width: 3%; text-align: center; vertical-align: top; {borderStyle}'>{data.symbol}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_prm}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kms}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_kl}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; {borderStyle}'>{nilai_share_bgu}</td>
                                        </tr>");
                            }
                        }
                
                        stringBuilder.Append($@"</table>
                                    <table class='table' style='margin-top: 50px; width: 100%;'>
                                        <tr>
                                            <td style='width: 60%;'></td>
                                            <td style='text-align: center; width: 40%; font-size: 12px;'>Jakarta {firstDataOuter.tgl_soa:dd MMMM yyyy}</td>
                                        </tr>
                                        <tr>
                                            <td style='width: 60%;'></td>
                                            <td style='text-align: center; width: 40%; font-size: 12px;'>S.E & O</td>
                                        </tr>
                                        <tr style='height: 75px;'>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style='text-align: center; line-height: 1.3;'>
                                                <strong><u>{firstDataOuter.nm_bag}</u></strong><br>
                                                <span style='font-size: 11px; text-decoration: underline;'>{firstDataOuter.nm_kpl_bag}</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>");
                    }
                    break;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}