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
    public class GetCetakNotaDanKwitansiPolisQuery : IRequest<string>
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

    public class GetCetakNotaDanKwitansiPolisQueryHandler : IRequestHandler<GetCetakNotaDanKwitansiPolisQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        private List<string> ReportHaveDetails = new List<string>()
        {
        };

        private List<string> MultipleReport = new List<string>()
        {
        };

        public GetCetakNotaDanKwitansiPolisQueryHandler(IDbConnectionFactory connectionFactory, 
            IHostEnvironment environment, ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetCetakNotaDanKwitansiPolisQuery request, CancellationToken cancellationToken)
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
                        input_str = $"{request.kd_cb.Trim()}{request.kd_cob.Trim()}{request.kd_scob.Trim()}" +
                                    $"{request.kd_thn}{request.no_pol.Trim()}{request.no_updt}{request.nm_ttg?.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTemplateName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakNotaDanKwitansiPolisData.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );
            
            string resultTemplate;

            if (MultipleReport.Contains(reportTemplateName))
                return GenerateMultipleReport(reportTemplateName, cetakNotaDanKwitansiPolisData, templateReportHtml);
            
            var cetakNotaDanKwitansiPolis = cetakNotaDanKwitansiPolisData.FirstOrDefault();
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

            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);
            
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
                    pst_ppn, nilai_ppn,
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
                    pst_ppn, nilai_ppn,
                    pst_pph_title = cetakNotaDanKwitansiPolis.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    pst_pph, nilai_pph, cetakNotaDanKwitansiPolis.almt_nota, cetakNotaDanKwitansiPolis.nm_nota,
                    pst_lain, kd_mtu_symbol_9 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.kt_cb, cetakNotaDanKwitansiPolis.almt_ttg, cetakNotaDanKwitansiPolis.ket_kwi,
                    cetakNotaDanKwitansiPolis.period_polis, cetakNotaDanKwitansiPolis.kd_mtu_symbol, nilai_total,
                    title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
                } );
            }

            return resultTemplate;
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
        
        private string GenerateMultipleReport(string reportType, List<CetakNotaDanKwitansiPolisDto> datas, string template)
        {
            Template templateProfileResult = Template.Parse( template );
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Constant.HeaderReport);
            // switch (reportType)
            // {
            //     case "PolisPASiramaMulti.html":
            //         foreach (var cetakSchedulePolis in datas)
            //         {
            //             var sub_total_kebakaran = cetakSchedulePolis.nilai_ttl -
            //                                       (cetakSchedulePolis.nilai_bia_mat + cetakSchedulePolis.nilai_bia_pol);
            //             stringBuilder.Append(templateProfileResult.Render(new
            //             {
            //                 cetakSchedulePolis.no_pol_ttg, cetakSchedulePolis.nm_ttg,
            //                 cetakSchedulePolis.almt_ttg, cetakSchedulePolis.kd_pos,
            //                 cetakSchedulePolis.almt_rsk, cetakSchedulePolis.kd_pos_rsk,
            //                 cetakSchedulePolis.jk_wkt_ptg, cetakSchedulePolis.tgl_mul_ptg_ind,
            //                 cetakSchedulePolis.tgl_akh_ptg_ind, cetakSchedulePolis.nm_okup,
            //                 cetakSchedulePolis.kd_okup, cetakSchedulePolis.nm_kls_konstr,
            //                 cetakSchedulePolis.pst_rate_prm_pkk, cetakSchedulePolis.stn_rate_prm_pkk,
            //                 cetakSchedulePolis.kd_mtu_symbol, cetakSchedulePolis.nilai_prk_pkk,
            //                 cetakSchedulePolis.nm_cvrg_01, cetakSchedulePolis.kd_cvrg_01,
            //                 cetakSchedulePolis.pst_rate_prm_01, cetakSchedulePolis.stn_rate_prm_01,
            //                 cetakSchedulePolis.nilai_prm_tbh_01, cetakSchedulePolis.nm_cvrg_02,
            //                 cetakSchedulePolis.kd_cvrg_02, cetakSchedulePolis.pst_rate_prm_02,
            //                 cetakSchedulePolis.stn_rate_prm_02, cetakSchedulePolis.nilai_prm_tbh_02,
            //                 cetakSchedulePolis.ket_dis, cetakSchedulePolis.nilai_dis,
            //                 cetakSchedulePolis.nilai_bia_pol, cetakSchedulePolis.nilai_bia_mat,
            //                 cetakSchedulePolis.nilai_ttl, cetakSchedulePolis.ket_klausula,
            //                 cetakSchedulePolis.desk_oby_01, cetakSchedulePolis.nilai_oby_01,
            //                 cetakSchedulePolis.desk_oby_02, cetakSchedulePolis.nilai_oby_02,
            //                 cetakSchedulePolis.desk_oby_03, cetakSchedulePolis.nilai_oby_03,
            //                 cetakSchedulePolis.desk_oby_04, cetakSchedulePolis.nilai_oby_04,
            //                 cetakSchedulePolis.desk_oby_05, cetakSchedulePolis.nilai_oby_05,
            //                 cetakSchedulePolis.nilai_ttl_ptg, cetakSchedulePolis.tgl_closing_ind,
            //                 cetakSchedulePolis.stnc, cetakSchedulePolis.ctt_pol,
            //                 cetakSchedulePolis.kt_cb, cetakSchedulePolis.nm_user,
            //                 cetakSchedulePolis.no_msn, cetakSchedulePolis.no_rangka,
            //                 cetakSchedulePolis.no_pls, cetakSchedulePolis.nilai_prm_pkm,
            //                 cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
            //                 cetakSchedulePolis.desk_aksesoris, cetakSchedulePolis.nm_utk,
            //                 cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
            //                 cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
            //                 cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
            //                 cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
            //                 cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
            //                 sub_total_kebakaran
            //             }));
            //         }
            //
            //         break;
            // }

            stringBuilder.Append(Constant.FooterReport);
            return stringBuilder.ToString();
        }
    }
}