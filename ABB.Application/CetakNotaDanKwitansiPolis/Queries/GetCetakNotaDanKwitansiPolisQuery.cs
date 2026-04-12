using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakSchedulePolis.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaDanKwitansiPolis.Queries
{
    public class GetCetakNotaDanKwitansiPolisQuery : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_pol { get; set; }
        public string? nm_ttg { get; set; }
        public int no_updt { get; set; }
        public string jenisLaporan { get; set; }
        public string mataUang { get; set; }
    }

    public class GetCetakNotaDanKwitansiPolisQueryHandler : IRequestHandler<GetCetakNotaDanKwitansiPolisQuery, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly ReportTTDConfig _reportTtdConfig;

        private List<string> ReportHaveDetails = new List<string>()
        {
        };

        private List<string> MultipleReport = new List<string>()
        {
            "CetakanKwitansiAngsuran.html",
            "CetakanNotaDebetKreditAngsuran.html"
        };

        public GetCetakNotaDanKwitansiPolisQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment, 
            ReportConfig reportConfig, ReportTTDConfig reportTtdConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
            _reportTtdConfig = reportTtdConfig;
        }

        public async Task<(string, string)> Handle(GetCetakNotaDanKwitansiPolisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var templateName = (await _connectionFactory.QueryProc<string>("spi_uw02r_01", 
                new
                {
                    kd_lap = request.jenisLaporan
                })).FirstOrDefault();

            if (templateName == null || !Constant.ReportMapping.Keys.Contains(templateName))
                throw new Exception("Template Not Found");

            var reportTemplateName = Constant.ReportMapping[templateName];
            
            if (!Constant.ReportStoreProcedureMapping.Keys.Contains(reportTemplateName))
                throw new Exception("Store Procedure Not Found");
            
            var storeProcedureName = Constant.ReportStoreProcedureMapping[reportTemplateName];
            
            var cetakNotaDanKwitansiPolisData = (await _connectionFactory.QueryProc<CetakNotaDanKwitansiPolisDto>(storeProcedureName, 
                new
                {
                        input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                    $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt}," +
                                    $"{request.jenisLaporan?.Trim()},{request.mataUang}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTemplateName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakNotaDanKwitansiPolisData.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );
            
            string resultTemplate;

            var footer_template = string.Empty;
            var ttd_image = string.Empty;
            var nm_pejabat = string.Empty;
            var jabatan = string.Empty;

            var cetakNotaDanKwitansiPolis = cetakNotaDanKwitansiPolisData.FirstOrDefault();
            if (cetakNotaDanKwitansiPolis.kd_cb.Length >= 4 && cetakNotaDanKwitansiPolis.kd_cb.Substring(3, 1) != "0")
            {
                var ttdImageBase64 = string.Empty;
                var wwwroot = Path.Combine(_environment.ContentRootPath, "wwwroot", "img");
                var imageFile = Path.Combine(wwwroot, _reportTtdConfig.NamaTTDFile);
                if (File.Exists(imageFile))
                {
                    ttdImageBase64 = Convert.ToBase64String(File.ReadAllBytes(imageFile));
                }
                string extension = Path.GetExtension(imageFile).ToLower();
                string mimeType = extension switch
                {
                    ".png" => "image/png",
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream"
                };
                ttd_image = $"<img src='data:${mimeType};base64,${ttdImageBase64}' style='max-width: 200px;' />";
                nm_pejabat = $"<p style='margin: 0'><strong><u>{_reportTtdConfig.NamaPejabat}</u></strong></p>";
                jabatan = $"<p style='margin: 0'><strong>{_reportTtdConfig.Jabatan}<strong></p>";
                footer_template = $@"<div style='text-align: center;'>{ttd_image}{nm_pejabat}{jabatan}</div>";
            }
            
            var reportConfig = _reportConfig.GetReportData(request.kd_cb);
            
            if (MultipleReport.Contains(reportTemplateName))
                return GenerateMultipleReport(reportTemplateName, cetakNotaDanKwitansiPolisData, templateReportHtml,
                    reportConfig, footer_template, ttd_image, jabatan, nm_pejabat);
            
            var nilai_01 = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_01);
            var nilai_02 = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_02);
            var nilai_03 = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_03);
            var nilai_04 = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_04);
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_ttl_ptg);
            var nilai_nt = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_nt);
            var total_nilai = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_01 + cetakNotaDanKwitansiPolis.nilai_03 + cetakNotaDanKwitansiPolis.nilai_04);
            var pst_kms = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.pst_kms, true);
            var nilai_net_kms = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_net_kms);
            var pst_ppn = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.pst_ppn, true);
            var nilai_ppn = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_ppn);
            var pst_pph = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.pst_pph, true);
            var nilai_pph = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_pph);
            var pst_lain = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.pst_lain, true);
            var nilai_lain = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_lain);
            var nilai_total = ReportHelper.ConvertToReportFormat(cetakNotaDanKwitansiPolis.nilai_net_kms +
                                                                 cetakNotaDanKwitansiPolis.nilai_ppn +
                                                                 cetakNotaDanKwitansiPolis.nilai_pph +
                                                                 cetakNotaDanKwitansiPolis.nilai_lain);
            
            if (ReportHaveDetails.Contains(reportTemplateName))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var data in cetakNotaDanKwitansiPolisData)
                {
                    stringBuilder.Append(GenerateDetailReport(reportTemplateName, data));
                }
                
                resultTemplate = templateProfileResult.Render( new
                {
                    jns_nota = cetakNotaDanKwitansiPolis.jns_tr + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_msk + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_kel,
                    cetakNotaDanKwitansiPolis.no_nota, cetakNotaDanKwitansiPolis.tgl_nt_ind,
                    cetakNotaDanKwitansiPolis.no_reg, cetakNotaDanKwitansiPolis.no_ref,
                    cetakNotaDanKwitansiPolis.uraian_01, kd_mtu_symbol_1 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_01, cetakNotaDanKwitansiPolis.nm_ttg,
                    cetakNotaDanKwitansiPolis.uraian_02, kd_mtu_symbol_5 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_02, cetakNotaDanKwitansiPolis.no_pol_ttg,
                    cetakNotaDanKwitansiPolis.uraian_03, kd_mtu_symbol_7 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_03, cetakNotaDanKwitansiPolis.periode_polis,
                    cetakNotaDanKwitansiPolis.uraian_04, kd_mtu_symbol_8 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_04, cetakNotaDanKwitansiPolis.nm_scob,
                    nilai_ttl_ptg, kd_mtu_symbol_6 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    kd_mtu_symbol_12 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_nt, cetakNotaDanKwitansiPolis.ket_nilai_nt,
                    cetakNotaDanKwitansiPolis.nm_cb, cetakNotaDanKwitansiPolis.nm_rek,
                    cetakNotaDanKwitansiPolis.nm_akun, cetakNotaDanKwitansiPolis.nm_ttj_kms,
                    cetakNotaDanKwitansiPolis.almt_ttj_kms, cetakNotaDanKwitansiPolis.kt_ttj_kms,
                    cetakNotaDanKwitansiPolis.no_npwp, cetakNotaDanKwitansiPolis,
                    kd_mtu_symbol_3 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, kd_mtu_symbol_2 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    kd_mtu_symbol_4 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, cetakNotaDanKwitansiPolis.ket_nt_kms,
                    total_nilai, pst_kms, nilai_lain,
                    nilai_net_kms, kd_mtu_symbol_11 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    pst_ppn, nilai_ppn, footer_template, ttd_image, jabatan, nm_pejabat,
                    pst_pph_title = cetakNotaDanKwitansiPolis.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    pst_pph, nilai_pph, cetakNotaDanKwitansiPolis.almt_nota, cetakNotaDanKwitansiPolis.nm_nota,
                    pst_lain, kd_mtu_symbol_9 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.kt_cb, cetakNotaDanKwitansiPolis.almt_ttg, cetakNotaDanKwitansiPolis.ket_kwi,
                    cetakNotaDanKwitansiPolis.period_polis, cetakNotaDanKwitansiPolis.kd_mtu_symbol, nilai_total,
                    title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
                } );
            }
            else
            {
                resultTemplate = templateProfileResult.Render( new
                {
                    jns_nota = cetakNotaDanKwitansiPolis.jns_tr + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_msk + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_kel,
                    cetakNotaDanKwitansiPolis.no_nota, cetakNotaDanKwitansiPolis.tgl_nt_ind,
                    cetakNotaDanKwitansiPolis.no_reg, cetakNotaDanKwitansiPolis.no_ref,
                    cetakNotaDanKwitansiPolis.uraian_01, kd_mtu_symbol_1 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_01, cetakNotaDanKwitansiPolis.nm_ttg,
                    cetakNotaDanKwitansiPolis.uraian_02, kd_mtu_symbol_5 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_02, cetakNotaDanKwitansiPolis.no_pol_ttg,
                    cetakNotaDanKwitansiPolis.uraian_03, kd_mtu_symbol_7 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_03, cetakNotaDanKwitansiPolis.periode_polis,
                    cetakNotaDanKwitansiPolis.uraian_04, kd_mtu_symbol_8 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_04, cetakNotaDanKwitansiPolis.nm_scob,
                    nilai_ttl_ptg, kd_mtu_symbol_6 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    nilai_nt, cetakNotaDanKwitansiPolis.ket_nilai_nt,
                    cetakNotaDanKwitansiPolis.nm_cb, cetakNotaDanKwitansiPolis.nm_rek,
                    cetakNotaDanKwitansiPolis.nm_akun, cetakNotaDanKwitansiPolis.nm_ttj_kms,
                    cetakNotaDanKwitansiPolis.almt_ttj_kms, cetakNotaDanKwitansiPolis.kt_ttj_kms,
                    cetakNotaDanKwitansiPolis.no_npwp, cetakNotaDanKwitansiPolis,
                    kd_mtu_symbol_3 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, kd_mtu_symbol_2 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    kd_mtu_symbol_4 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, cetakNotaDanKwitansiPolis.ket_nt_kms,
                    total_nilai, pst_kms, nilai_lain,
                    nilai_net_kms, kd_mtu_symbol_11 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    pst_ppn, nilai_ppn, footer_template, ttd_image, jabatan, nm_pejabat,
                    pst_pph_title = cetakNotaDanKwitansiPolis.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    pst_pph, nilai_pph, cetakNotaDanKwitansiPolis.almt_nota, cetakNotaDanKwitansiPolis.nm_nota,
                    pst_lain, kd_mtu_symbol_9 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.kt_cb, cetakNotaDanKwitansiPolis.almt_ttg, cetakNotaDanKwitansiPolis.ket_kwi,
                    cetakNotaDanKwitansiPolis.period_polis, cetakNotaDanKwitansiPolis.kd_mtu_symbol, nilai_total,
                    title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
                } );
            }

            var reportName = reportTemplateName.Split(".")[0];
            return (reportName, resultTemplate);
        }
        
        private string GenerateDetailReport(string reportType, CetakNotaDanKwitansiPolisDto data)
        {
            switch (reportType)
            {
                // case "LampiranPolisFireDaftarIsi.html":
                //     return @$"<tr style='height: 50px;'>
                //         <td style='font-weight: bold;' colspan='8'>{data.nm_grp_oby}</td>
                //         </tr>
                //         <tr>
                //             <td style='width: 3%;  text-align: right; vertical-align: top;' rowspan='2'>{data.no_oby}</td>
                //             <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.desk_oby}</td>
                //             <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.almt_rsk}</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.ket_okup}</td>
                //             <td style='width: 5%;  text-align: center; vertical-align: top;'>{data.kd_kls_konstr}</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>if( {data.kd_penerangan} ='1','Listrik', 'Lain-lain')</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.symbol}</td>
                //             <td style='width: 10%; text-align: center; vertical-align: top;' rowspan='2'>{data.nilai_ttl_ptg}</td>
                //         </tr>
                //         <tr>
                //             <td style='vertical-align: top; text-align: center;'>{data.ket_rsk}</td>
                //         </tr>
                //         <tr>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td style='font-weight: bold; text-align: center;' colspan='2'>{data.nm_grp_oby} : </td>
                //             <td style='font-weight: bold;'>sum( {data.nilai_ttl_ptg} for 1  ) </td>
                //         </tr>";
                default:
                    return string.Empty;
            }
        }
        
        private (string, string) GenerateMultipleReport(string reportType, List<CetakNotaDanKwitansiPolisDto> datas, 
            string template, ReportData reportConfig, string footer_template, string ttd_image, string jabatan,
            string nm_pejabat)
        {
            // 1. Map filenames to Display Titles
            var reportTitles = new Dictionary<string, string>
            {
                { "CetakanKwitansiAngsuran.html", "Cetakan Kwitansi Angsuran" },
                { "CetakanNotaDebetKreditAngsuran.html", "Cetakan Nota Debet/Kredit Angsuran" },
                // Add new report types here easily
            };

            // Fallback to filename if not in dictionary
            string displayTitle = reportTitles.GetValueOrDefault(reportType, "Laporan Polis");

            var templateProfileResult = Template.Parse(template);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Constant.HeaderReportSimple.Replace("{{TITLE}}", displayTitle));

            var totalData = datas.Count;
            var sequence = 1;
            // 2. The Generic Loop
            foreach (var item in datas)
            {
                // Helper to keep the Render call clean
                Func<decimal?, bool, string> fmt = ReportHelper.ConvertToReportFormat;

                stringBuilder.Append(templateProfileResult.Render(new
                {
                    // Concatenated Fields
                    jns_nota = $"{item.jns_tr} . {item.jns_nt_msk} . {item.jns_nt_kel}",
                    pst_pph_title = item.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    
                    // Formatted Values
                    nilai_01 = fmt(item.nilai_01, false),
                    nilai_02 = fmt(item.nilai_02, false),
                    nilai_03 = fmt(item.nilai_03, false),
                    nilai_04 = fmt(item.nilai_04, false),
                    nilai_ttl_ptg = fmt(item.nilai_ttl_ptg, false),
                    nilai_nt = fmt(item.nilai_nt, false),
                    total_nilai = fmt(item.nilai_01 + item.nilai_03 + item.nilai_04, false),
                    nilai_net_kms = fmt(item.nilai_net_kms, false),
                    nilai_ppn = fmt(item.nilai_ppn, false),
                    nilai_pph = fmt(item.nilai_pph, false),
                    nilai_lain = fmt(item.nilai_lain, false),
                    nilai_total = fmt(item.nilai_net_kms + item.nilai_ppn + item.nilai_pph + item.nilai_lain, false),
                    
                    // Percentages/Ratios
                    pst_kms = fmt(item.pst_kms, true),
                    pst_ppn = fmt(item.pst_ppn, true),
                    pst_pph = fmt(item.pst_pph, true),
                    pst_lain = fmt(item.pst_lain, true),

                    // Pass-through data
                    item.no_nota, item.tgl_nt_ind, item.no_reg, item.no_ref, item.uraian_01, item.nm_ttg,
                    item.uraian_02, item.no_pol_ttg, item.uraian_03, item.periode_polis, item.uraian_04, item.nm_scob,
                    item.ket_nilai_nt, item.nm_cb, item.nm_rek, item.nm_akun, item.nm_ttj_kms, item.almt_ttj_kms, 
                    item.kt_ttj_kms, item.no_npwp, item.ket_nt_kms, item.almt_nota, item.nm_nota, item.kt_cb, 
                    item.almt_ttg, item.ket_kwi, item.period_polis, item.kd_mtu_symbol,

                    // Aliases for template compatibility
                    kd_mtu_symbol_1 = item.kd_mtu_symbol, kd_mtu_symbol_2 = item.kd_mtu_symbol,
                    kd_mtu_symbol_3 = item.kd_mtu_symbol, kd_mtu_symbol_4 = item.kd_mtu_symbol,
                    kd_mtu_symbol_5 = item.kd_mtu_symbol, kd_mtu_symbol_6 = item.kd_mtu_symbol,
                    kd_mtu_symbol_7 = item.kd_mtu_symbol, kd_mtu_symbol_8 = item.kd_mtu_symbol,
                    kd_mtu_symbol_9 = item.kd_mtu_symbol, kd_mtu_symbol_11 = item.kd_mtu_symbol,

                    // Config & Parameters
                    footer_template, ttd_image, jabatan, nm_pejabat,
                    title3 = reportConfig.Title.Title3, 
                    title4 = reportConfig.Title.Title4, 
                    title6 = reportConfig.Title.Title6
                }));

                if (sequence < totalData)
                {
                    sequence++;
                    stringBuilder.Append("\n<div class='container'></div>");
                }
            }

            stringBuilder.Append(Constant.FooterReport);

            return (reportType.Replace(".html", ""), stringBuilder.ToString());
        }
    }
}