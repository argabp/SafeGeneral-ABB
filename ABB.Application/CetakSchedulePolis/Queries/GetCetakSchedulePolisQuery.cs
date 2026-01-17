using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Scriban;

namespace ABB.Application.CetakSchedulePolis.Queries
{
    public class GetCetakSchedulePolisQuery : IRequest<string>
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
        public string bahasa { get; set; }
        public string jenisLampiran { get; set; }
    }

    public class GetCetakSchedulePolisQueryHandler : IRequestHandler<GetCetakSchedulePolisQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly IHostEnvironment _root;

        private List<string> ReportHaveDetails = new List<string>()
        {
            "LampiranPolisFireDaftarIsi.html",
            "LampiranPolisPASiramaObyek.html",
            "LampiranPolisPABiasaDaftarisi.html",
            "LampiranPolisCargoDaftarisi.html",
            "LampiranPolisMotorListing.html",
            "LampiranPolisMotorDetil.html"
        };

        private List<string> MultipleReport = new List<string>()
        {
            "PolisPASiramaMulti.html"
        };

        public GetCetakSchedulePolisQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig, IHostEnvironment root)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
            _root = root;
        }

        public async Task<string> Handle(GetCetakSchedulePolisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var templateName = (await _connectionFactory.QueryProc<string>("spi_uw01r_01", 
                new
                {
                    request.kd_cob, request.kd_scob, kd_lap = request.jenisLaporan, 
                    kd_bond = request.jenisLampiran, kd_bhs = request.bahasa, 
                    kd_perhit_prm = string.Empty, 
                })).FirstOrDefault();

            if (templateName == null || !Constant.ReportMapping.Keys.Contains(templateName))
                throw new Exception("Template Not Found");

            var reportTemplateName = Constant.ReportMapping[templateName];
            
            if (!Constant.ReportStoreProcedureMapping.Keys.Contains(reportTemplateName))
                throw new Exception("Store Procedure Not Found");
            
            var storeProcedureName = Constant.ReportStoreProcedureMapping[reportTemplateName];
            
            var cetakSchedulePolisData = (await _connectionFactory.QueryProc<CetakSchedulePolisDto>(storeProcedureName, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt},{request.nm_ttg?.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTemplateName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakSchedulePolisData.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate = string.Empty;;

            if (MultipleReport.Contains(reportTemplateName))
                return GenerateMultipleReport(reportTemplateName, cetakSchedulePolisData, templateReportHtml);
            
            var cetakSchedulePolis = cetakSchedulePolisData.FirstOrDefault();
            var sub_total_kebakaran = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ttl -
                                      (cetakSchedulePolis.nilai_bia_mat + cetakSchedulePolis.nilai_bia_pol));

            var nilai_ptg_a = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ptg_A);
            var tgl_mul_ptg = DateTime.TryParse(cetakSchedulePolis.tgl_mul_ptg, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(cetakSchedulePolis.tgl_mul_ptg), "dd-MMM-yyyy")
                : cetakSchedulePolis.tgl_mul_ptg;
            var tgl_akh_ptg = DateTime.TryParse(cetakSchedulePolis.tgl_akh_ptg, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(cetakSchedulePolis.tgl_akh_ptg), "dd-MMM-yyyy")
                : cetakSchedulePolis.tgl_akh_ptg;
            var nilai_prm_pkk = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_pkk);
            var nilai_prm_tbh_01 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_01);
            var nilai_bia_pol = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_bia_pol);
            var nilai_bia_mat = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_bia_mat);
            var nilai_ttl = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ttl);
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ttl_ptg);
            var nilai_kendaraan_bermotor = string.Empty;

            if (cetakSchedulePolis.nilai_casco != null && cetakSchedulePolis.nilai_aksesories != null)
            {
                nilai_kendaraan_bermotor = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_casco -
                                                                              cetakSchedulePolis.nilai_aksesories);
            }
            
            var nilai_tjh = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_tjh);
            var nilai_aksesories = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_aksesories);
            var nilai_pad = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_pad);
            var nilai_pap = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_pap);
            var nilai_tjp = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_tjp);
            var nilai_rsk_sendiri = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_rsk_sendiri);
            var suku_premi = string.Empty;
            if (cetakSchedulePolis.pst_rate_prm != null && cetakSchedulePolis.stn_rate_prm != null)
            {
                var sp_stringBuilder = new StringBuilder();
                sp_stringBuilder.Append(cetakSchedulePolis.pst_rate_prm.Value.ToString("#0.0000"));
                sp_stringBuilder.Append(cetakSchedulePolis.stn_rate_prm == "1" ? " %" : " %o");

                if (cetakSchedulePolis.pst_tjh != null && cetakSchedulePolis.pst_tjh > 0)
                {
                    sp_stringBuilder.Append($" (Pokok) {cetakSchedulePolis.pst_tjh:#0.0000} % (TJH)");
                }

                if (cetakSchedulePolis.pst_rate_hh != null && cetakSchedulePolis.pst_rate_hh > 0 &&
                    cetakSchedulePolis.stn_rate_hh != null)
                {
                    var stn_rate_hh = cetakSchedulePolis.stn_rate_hh == 1 ? "%" : "%o";
                    sp_stringBuilder.Append($",  {cetakSchedulePolis.pst_rate_hh:#0.0000} {stn_rate_hh} (SRCC)");
                }

                if (cetakSchedulePolis.pst_rate_aog != null && cetakSchedulePolis.pst_rate_aog > 0 &&
                    cetakSchedulePolis.stn_rate_aog != null)
                {
                    var stn_rate_aog = cetakSchedulePolis.stn_rate_aog == 1 ? "%" : "%o";
                    sp_stringBuilder.Append($",  {cetakSchedulePolis.pst_rate_aog:#0.0000} {stn_rate_aog} (AOG)");
                }

                if (cetakSchedulePolis.pst_rate_banjir != null && cetakSchedulePolis.pst_rate_banjir > 0 &&
                    cetakSchedulePolis.stn_rate_banjir != null)
                {
                    var stn_rate_banjir = cetakSchedulePolis.stn_rate_banjir == 1 ? "%" : "%o";
                    sp_stringBuilder.Append($",  {cetakSchedulePolis.pst_rate_banjir:#0.0000} {stn_rate_banjir} (Banjir)");
                }

                if (cetakSchedulePolis.pst_rate_trs != null && cetakSchedulePolis.pst_rate_trs > 0 &&
                    cetakSchedulePolis.stn_rate_trs != null)
                {
                    var stn_rate_trs = cetakSchedulePolis.stn_rate_trs == 1 ? "%" : "%o";
                    sp_stringBuilder.Append($",  {cetakSchedulePolis.pst_rate_trs:#0.0000} {stn_rate_trs} (TRS)");
                }

                if (cetakSchedulePolis.nilai_prm_pad != null && cetakSchedulePolis.nilai_prm_pad > 0)
                {
                    sp_stringBuilder.Append(", (0.5%(PAD)");
                }

                if (cetakSchedulePolis.nilai_prm_pap != null && cetakSchedulePolis.nilai_prm_pap > 0)
                {
                    sp_stringBuilder.Append(", (0.1%(PAP)");
                }

                suku_premi = sp_stringBuilder.ToString();
            }
            
            var nilai_prm_casco = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_casco);
            var nilai_prm_tjh = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tjh);
            var nilai_prm_pa = string.Empty;

            if (cetakSchedulePolis.nilai_prm_pad != null && cetakSchedulePolis.nilai_prm_pad != null)
            {
                nilai_prm_pa = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_pad +
                                                                  cetakSchedulePolis.nilai_prm_pad);
            }
            var nilai_dis = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_dis);
            
            var nilai_prm_banjir = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_banjir);
            var nilai_prm_aog = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_aog);
            var nilai_prm_hh = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_hh);
            var nilai_prm_trs = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_trs);
            var nilai_prm_tjp = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tjp);
            var percentage_diskon =  string.Empty;

            if (cetakSchedulePolis.pst_dis != null)
            {
                percentage_diskon = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_dis) + "%";
            }
            var pst_rate_prm = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm, true);
            var nilai_prm = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm);
            var nilai_bond = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_bond);
            var charge = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.charge);
            var nilai_ptg_b = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ptg_B);
            var nilai_ptg_c = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ptg_C);
            var nilai_ptg_d = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ptg_D);
            
            var stn_rate_a = cetakSchedulePolis.stn_rate_A == 1 ? "%" : "%o";
            var pst_a = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_A, true);
            var view_pst_stn_a = ReportHelper.ConvertToDecimalFormat(pst_a) > 0
                ? $"<td style='width: 20%;'>A. {pst_a} {stn_rate_a}</td>"
                : string.Empty;
            var stn_rate_b = cetakSchedulePolis.stn_rate_B == 1 ? "%" : "%o";
            var pst_b = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_B, true);
            var view_pst_stn_b = ReportHelper.ConvertToDecimalFormat(pst_b) > 0
                ? $"<td>B. {pst_b} {stn_rate_b}</td>"
                : string.Empty;
            var stn_rate_c = cetakSchedulePolis.stn_rate_C == 1 ? "%" : "%o";
            var pst_c = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_B, true);
            var view_pst_stn_c = ReportHelper.ConvertToDecimalFormat(pst_c) > 0
                ? $"<td>C. {pst_c} {stn_rate_c}</td>"
                : string.Empty;
            var stn_rate_d = cetakSchedulePolis.stn_rate_D == 1 ? "%" : "%o";
            var pst_d = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_B, true);
            var view_pst_stn_d = ReportHelper.ConvertToDecimalFormat(pst_d) > 0
                ? $"<td>D. {pst_d} {stn_rate_d}</td>"
                : string.Empty;
            var nilai_oby_01 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_01);
            var nilai_oby_02 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_02);
            var nilai_oby_03 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_03);
            var nilai_oby_04 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_04);
            var nilai_oby_05 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_05);
            var nilai_prm_a = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_A);
            var nilai_prm_b = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_B);
            var nilai_prm_c = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_C);
            var nilai_prm_d = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_D);
            var view_jns_nilai_ptg_a = ReportHelper.ConvertToDecimalFormat(nilai_ptg_a) > 0
                ? $@"
                    <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>A. Kematian karena kecelakaan</td>
                    <td>:</td>
                    <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='width: 20%;'>{nilai_ptg_a}</td>
                    </tr>"
                : string.Empty;
            var view_jns_nilai_ptg_b = ReportHelper.ConvertToDecimalFormat(nilai_ptg_b) > 0
                ? $@"
                    <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>B. Cacat tetap keseluruhan / Cacat tetap sebagian</td>
                    <td>:</td>
                    <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='width: 20%;'>{nilai_ptg_b}</td>
                    </tr>"
                : string.Empty;
            var view_jns_nilai_ptg_c = ReportHelper.ConvertToDecimalFormat(nilai_ptg_c) > 0
                ? $@"
                    <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>C. Biaya-biaya perawatan/pengobatan akibat Kecelakaan</td>
                    <td>:</td>
                    <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='width: 20%;'>{nilai_ptg_c}</td>
                    </tr>"
                : string.Empty;
            var view_jns_nilai_ptg_d = ReportHelper.ConvertToDecimalFormat(nilai_ptg_d) > 0
                ? $@"
                    <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>D. Sentuhan meninggal dunia alami</td>
                    <td>:</td>
                    <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='width: 20%;'>{nilai_ptg_d}</td>
                    </tr>"
                : string.Empty;

            var view_prm_tbh_01 =
                cetakSchedulePolis.nilai_prm_tbh_01 == null || cetakSchedulePolis.nilai_prm_tbh_01.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_01)}</td>";
            var view_prm_tbh_02 =
                cetakSchedulePolis.nilai_prm_tbh_02 == null || cetakSchedulePolis.nilai_prm_tbh_02.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_02}</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_02)}</td>";
            var view_prm_tbh_03 =
                cetakSchedulePolis.nilai_prm_tbh_03 == null || cetakSchedulePolis.nilai_prm_tbh_03.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_03}</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_03)}</td>";
            var view_prm_tbh_04 =
                cetakSchedulePolis.nilai_prm_tbh_04 == null || cetakSchedulePolis.nilai_prm_tbh_04.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_04}</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_04)}</td>";
            var view_prm_tbh_05 =
                cetakSchedulePolis.nilai_prm_tbh_05 == null || cetakSchedulePolis.nilai_prm_tbh_05.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_05}</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_05)}</td>";
            var view_nilai_dis =
                cetakSchedulePolis.nilai_dis == null || cetakSchedulePolis.nilai_dis.Value == 0
                    ? string.Empty
                    : $@"<td style='width: 25%;'>- Discount</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                <td style='width: 15%; text-align: right;'>{ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_dis, true)}</td>";
            var view_row_desk_oby_01 =
                cetakSchedulePolis.nilai_oby_01 == null || cetakSchedulePolis.nilai_oby_01.Value == 0
                    ? string.Empty
                    : $@"<tr style='height: 20px;'>
                            <td style='width: 1%;'>1.</td>
                            <td style='width: 68%;' colspan='2'>{cetakSchedulePolis.desk_oby_01}</td>
                            <td></td>
                            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_01)}</td>
                        </tr>";
            var view_row_desk_oby_02 =
                cetakSchedulePolis.nilai_oby_02 == null || cetakSchedulePolis.nilai_oby_02.Value == 0
                    ? string.Empty
                    : $@"<tr style='height: 20px;'>
                            <td style='width: 1%;'>1.</td>
                            <td style='width: 68%;' colspan='2'>{cetakSchedulePolis.desk_oby_02}</td>
                            <td></td>
                            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_02)}</td>
                        </tr>";
            var view_row_desk_oby_03 =
                cetakSchedulePolis.nilai_oby_03 == null || cetakSchedulePolis.nilai_oby_03.Value == 0
                    ? string.Empty
                    : $@"<tr style='height: 20px;'>
                            <td style='width: 1%;'>1.</td>
                            <td style='width: 68%;' colspan='2'>{cetakSchedulePolis.desk_oby_01}</td>
                            <td></td>
                            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_03)}</td>
                        </tr>";
            var view_row_desk_oby_04 =
                cetakSchedulePolis.nilai_oby_04 == null || cetakSchedulePolis.nilai_oby_04.Value == 0
                    ? string.Empty
                    : $@"<tr style='height: 20px;'>
                            <td style='width: 1%;'>1.</td>
                            <td style='width: 68%;' colspan='2'>{cetakSchedulePolis.desk_oby_04}</td>
                            <td></td>
                            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_04)}</td>
                        </tr>";
            var view_row_desk_oby_05 =
                cetakSchedulePolis.nilai_oby_05 == null || cetakSchedulePolis.nilai_oby_05.Value == 0
                    ? string.Empty
                    : $@"<tr style='height: 20px;'>
                            <td style='width: 1%;'>1.</td>
                            <td style='width: 68%;' colspan='2'>{cetakSchedulePolis.desk_oby_05}</td>
                            <td></td>
                            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol} {ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_oby_05)}</td>
                        </tr>";
            
            var view_perhitungan_prm_a = ReportHelper.ConvertToDecimalFormat(nilai_ptg_a) > 0
                                                ? $@"<td style='width: 1%;'>:</td>
                                                <td style='width: 1%;'>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_ptg_a}</td>
                                                <td style='width: 1%;'>x</td>
                                                <td>{pst_a}</td>
                                                <td>{stn_rate_a}</td>
                                                <td>=</td>
                                                <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_prm_a}</td>"
                                                : string.Empty;
            var view_perhitungan_prm_b = ReportHelper.ConvertToDecimalFormat(nilai_ptg_b) > 0
                                                ? $@"<td style='width: 1%;'>:</td>
                                                <td style='width: 1%;'>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_ptg_b}</td>
                                                <td style='width: 1%;'>x</td>
                                                <td>{pst_b}</td>
                                                <td>{stn_rate_b}</td>
                                                <td>=</td>
                                                <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_prm_b}</td>"
                                                : string.Empty;
            var view_perhitungan_prm_c = ReportHelper.ConvertToDecimalFormat(nilai_ptg_c) > 0
                                                ? $@"<td style='width: 1%;'>:</td>
                                                <td style='width: 1%;'>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_ptg_c}</td>
                                                <td style='width: 1%;'>x</td>
                                                <td>{pst_c}</td>
                                                <td>{stn_rate_c}</td>
                                                <td>=</td>
                                                <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_prm_c}</td>"
                                                : string.Empty;
            var view_perhitungan_prm_d = ReportHelper.ConvertToDecimalFormat(nilai_ptg_d) > 0
                                                ? $@"<td style='width: 1%;'>:</td>
                                                <td style='width: 1%;'>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_ptg_d}</td>
                                                <td style='width: 1%;'>x</td>
                                                <td>{pst_a}</td>
                                                <td>{stn_rate_d}</td>
                                                <td>=</td>
                                                <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                                                <td>{nilai_prm_d}</td>"
                                                : string.Empty;
            
            var nilai_ttl_prm = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ttl_prm);
            var stn_rate_prm = cetakSchedulePolis.stn_rate_prm == "1" ? "%" : "%o";
            
            var view_desk_oby_2 = ReportHelper.ConvertToDecimalFormat(nilai_oby_02) > 0
            ? $@"<tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_02}</td>
                <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                <td style='vertical-align: top;'>{nilai_oby_02}</td>
                </tr>"
                : string.Empty;
            var view_desk_oby_3 = ReportHelper.ConvertToDecimalFormat(nilai_oby_03) > 0
            ? $@"<tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_03}</td>
                <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                <td style='vertical-align: top;'>{nilai_oby_03}</td>
                </tr>"
                : string.Empty;
            var view_desk_oby_4 = ReportHelper.ConvertToDecimalFormat(nilai_oby_04) > 0
            ? $@"<tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_04}</td>
                <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                <td style='vertical-align: top;'>{nilai_oby_04}</td>
                </tr>"
                : string.Empty;
            var view_desk_oby_5 = ReportHelper.ConvertToDecimalFormat(nilai_oby_05) > 0
            ? $@"<tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_05}</td>
                <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                <td style='vertical-align: top;'>{nilai_oby_05}</td>
                </tr>"
                : string.Empty;
            var pst_rate_prm_pkk = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_pkk, true);
            var stn_rate_prm_pkk = cetakSchedulePolis.stn_rate_prm_pkk == 1 ? "%" : "%o";
            var nilai_prk_pkk = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prk_pkk);
            var pst_rate_prm_01 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_01, true);
            var stn_rate_prm_01 = cetakSchedulePolis.stn_rate_prm_01 == 1 ? "%" : "%o";
            var pst_rate_prm_02 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_02, true);
            var stn_rate_prm_02 = cetakSchedulePolis.stn_rate_prm_02 == 1 ? "%" : "%o";
            var nilai_prm_tbh_02 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_02);
            var pst_rate_prm_03 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_03, true);
            var stn_rate_prm_03 = cetakSchedulePolis.stn_rate_prm_03 == 1 ? "%" : "%o";
            var nilai_prm_tbh_03 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_03);
            var pst_rate_prm_04 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_04, true);
            var stn_rate_prm_04 = cetakSchedulePolis.stn_rate_prm_04 == 1 ? "%" : "%o";
            var nilai_prm_tbh_04 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_04);
            var pst_rate_prm_05 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.pst_rate_prm_05, true);
            var stn_rate_prm_05 = cetakSchedulePolis.stn_rate_prm_05 == 1 ? "%" : "%o";
            var nilai_prm_tbh_05 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_05);
            var nilai_prm_pad = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_pad);

            #region FireSingle

            var fire_single_nm_cvrg_01_td = string.Empty;

            if (Convert.ToDecimal(nilai_prm_tbh_01) > 0)
            {
                fire_single_nm_cvrg_01_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>b. {cetakSchedulePolis.nm_cvrg_01}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_01}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_01}</td>
		                <td style='width: 15%;'>{stn_rate_prm_01}</td>
		                <td style='width: 1%'>:</td>
		                <td style='padding: 0; margin: 0; width: 17.5%'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>{nilai_prm_tbh_01}
		                </td>
	                </tr>";
            }
            else
            {
                fire_single_nm_cvrg_01_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>b. {cetakSchedulePolis.nm_cvrg_01}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_01}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_01}</td>
		                <td style='width: 15%;'>{stn_rate_prm_01}</td>
		                <td style='width: 1%'></td>
		                <td style='padding: 0; margin: 0; width: 17.5%'></td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>
                        </td>
	                </tr>";
            }

            var fire_single_nm_cvrg_02_td = string.Empty;

            if (Convert.ToDecimal(nilai_prm_tbh_02) > 0)
            {
                fire_single_nm_cvrg_02_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>b. {cetakSchedulePolis.nm_cvrg_02}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_02}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_02}</td>
		                <td style='width: 15%;'>{stn_rate_prm_02}</td>
		                <td style='width: 1%'>:</td>
		                <td style='padding: 0; margin: 0; width: 17.5%'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>{nilai_prm_tbh_02}
		                </td>
	                </tr>";
            }
            else
            {
                fire_single_nm_cvrg_02_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>b. {cetakSchedulePolis.nm_cvrg_02}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_02}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_02}</td>
		                <td style='width: 15%;'>{stn_rate_prm_02}</td>
		                <td style='width: 1%'></td>
		                <td style='padding: 0; margin: 0; width: 17.5%'></td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>
                        </td>
	                </tr>";
            }

            var fire_single_nm_cvrg_03_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.nm_cvrg_03) && Convert.ToDecimal(nilai_prm_tbh_03) > 0)
            {
                fire_single_nm_cvrg_03_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>{cetakSchedulePolis.nm_cvrg_03}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_03}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_03}</td>
		                <td style='width: 15%;'>{stn_rate_prm_03}</td>
		                <td style='width: 1%'>:</td>
		                <td style='padding: 0; margin: 0; width: 17.5%'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>{nilai_prm_tbh_03}
		                </td>
	                </tr>";
            }

            var fire_single_nm_cvrg_04_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.nm_cvrg_04) && Convert.ToDecimal(nilai_prm_tbh_04) > 0)
            {
                fire_single_nm_cvrg_04_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>{cetakSchedulePolis.nm_cvrg_03}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_03}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_03}</td>
		                <td style='width: 15%;'>{stn_rate_prm_03}</td>
		                <td style='width: 1%'>:</td>
		                <td style='padding: 0; margin: 0; width: 17.5%'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>{nilai_prm_tbh_03}
		                </td>
	                </tr>";
            }

            var fire_single_nm_cvrg_05_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.nm_cvrg_03) && Convert.ToDecimal(nilai_prm_tbh_05) > 0)
            {
                fire_single_nm_cvrg_05_td = @$"
	                <tr style='height: 20px;'>
		                <td style='width: 1%;'></td>
		                <td style='width: 12%;'>{cetakSchedulePolis.nm_cvrg_03}</td>
		                <td style='width: 10%;'>{cetakSchedulePolis.kd_cvrg_03}</td>
		                <td style='width: 15%;text-align: right'>{pst_rate_prm_03}</td>
		                <td style='width: 15%;'>{stn_rate_prm_03}</td>
		                <td style='width: 1%'>:</td>
		                <td style='padding: 0; margin: 0; width: 17.5%'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		                <td style='padding: 0; margin: 0; text-align: right; width: 5%'>{nilai_prm_tbh_03}
		                </td>
	                </tr>";
            }

            var fire_single_desk_oby_3_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.desk_oby_03) && Convert.ToDecimal(nilai_oby_03) > 0)
            {
                fire_single_desk_oby_3_td = @$"
	            <tr style='height: 20px;'>
		            <td style='width: 1%;'>3.</td>
		            <td style='width: 62%;' colspan=3>{cetakSchedulePolis.desk_oby_03}</td>
		            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		            <td style='padding: 0; margin: 0; width: 5%; text-align: right'>{nilai_oby_03}
		            </td>
	            </tr>";
            }

            var fire_single_desk_oby_4_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.desk_oby_04) && Convert.ToDecimal(nilai_oby_04) > 0)
            {
                fire_single_desk_oby_4_td = @$"
	            <tr style='height: 20px;'>
		            <td style='width: 1%;'>3.</td>
		            <td style='width: 62%;' colspan=3>{cetakSchedulePolis.desk_oby_04}</td>
		            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		            <td style='padding: 0; margin: 0; width: 5%; text-align: right'>{nilai_oby_04}
		            </td>
	            </tr>";
            }

            var fire_single_desk_oby_5_td = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.desk_oby_05) && Convert.ToDecimal(nilai_oby_05) > 0)
            {
                fire_single_desk_oby_5_td = @$"
	            <tr style='height: 20px;'>
		            <td style='width: 1%;'>3.</td>
		            <td style='width: 62%;' colspan=3>{cetakSchedulePolis.desk_oby_05}</td>
		            <td style='padding: 0; margin: 0; width: 17.5%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
		            <td style='padding: 0; margin: 0; width: 5%; text-align: right'>{nilai_oby_05}
		            </td>
	            </tr>";
            }

            #endregion
            
            var no_renew = cetakSchedulePolis.no_renew == "0" ? "(Baru)" : "(Perpanjangan)";

            #region Fire Multi
            
            var div_no_pol_lama = string.Empty;

            if (!string.IsNullOrWhiteSpace(cetakSchedulePolis.no_pol_lama))
            {
                div_no_pol_lama = $@"
                        <td style='width: 15%;'>Ex Polis</td>
                        <td style='width: 2.5%'>:</td>
                        <td style='width: 30%'>{cetakSchedulePolis.no_pol_lama}</td>";
            }

            var subTotal = ReportHelper.ConvertToReportFormat(Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_pkk ?? 0)  + 
                Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_tbh_01 ?? 0)  + Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_tbh_02 ?? 0) 
                                                              + Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_tbh_03 ?? 0)  + Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_tbh_04 ?? 0) 
                                                              + Convert.ToDecimal(cetakSchedulePolis?.nilai_prm_tbh_05 ?? 0)  - Convert.ToDecimal(cetakSchedulePolis?.nilai_dis ?? 0));
            
            var fire_multi_cvrg_01 = Convert.ToDecimal(nilai_prm_tbh_01) > 0 ? $@"
                    <td>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                    <td>:</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_prm_tbh_01}</td>" : $@"
                    <td></td>
                    <td></td>
                    <td></td>
                    <td</td>";
            
            var fire_multi_cvrg_02 = Convert.ToDecimal(nilai_prm_tbh_02) > 0 ? $@"
                    <td>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                    <td>:</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_prm_tbh_02}</td>" : $@"
                    <td></td>
                    <td></td>
                    <td></td>
                    <td</td>";
            
            var fire_multi_cvrg_03 = Convert.ToDecimal(nilai_prm_tbh_03) > 0 ? $@"
                    <td>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                    <td>:</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_prm_tbh_03}</td>" : $@"
                    <td></td>
                    <td></td>
                    <td></td>
                    <td</td>";
            
            var fire_multi_cvrg_04 = Convert.ToDecimal(nilai_prm_tbh_04) > 0 ? $@"
                    <td>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                    <td>:</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_prm_tbh_04}</td>" : $@"
                    <td></td>
                    <td></td>
                    <td></td>
                    <td</td>";
            
            var fire_multi_cvrg_05 = Convert.ToDecimal(nilai_prm_tbh_05) > 0 ? $@"
                    <td>- Jaminan Tambahan {cetakSchedulePolis.kd_cvrg_01}</td>
                    <td>:</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_prm_tbh_05}</td>" : $@"
                    <td></td>
                    <td></td>
                    <td></td>
                    <td</td>";
            
            var fire_multi_pst_rate_01 = Convert.ToDecimal(pst_rate_prm_01) > 0 ? $@"
                    <td style='width: 18%;'>{pst_rate_prm_01} {stn_rate_prm_01}</td>" : $@"
                    <td style='width: 18%;'></td>";
            
            var fire_multi_pst_rate_02 = Convert.ToDecimal(pst_rate_prm_02) > 0 ? $@"
                    <td style='width: 18%;'>{pst_rate_prm_02} {stn_rate_prm_02}</td>" : $@"
                    <td style='width: 18%;'></td>";
            
            var fire_multi_pst_rate_03 = Convert.ToDecimal(pst_rate_prm_03) > 0 ? $@"
                    <td style='width: 18%;'>{pst_rate_prm_03} {stn_rate_prm_03}</td>" : $@"
                    <td style='width: 18%;'></td>";
            
            var fire_multi_pst_rate_04 = Convert.ToDecimal(pst_rate_prm_04) > 0 ? $@"
                    <td style='width: 18%;'>{pst_rate_prm_04} {stn_rate_prm_04}</td>" : $@"
                    <td style='width: 18%;'></td>";
            
            var fire_multi_pst_rate_05 = Convert.ToDecimal(pst_rate_prm_05) > 0 ? $@"
                    <td style='width: 18%;'>{pst_rate_prm_05} {stn_rate_prm_05}</td>" : $@"
                    <td style='width: 18%;'></td>";
            
            var fire_multi_desk_oby_3 = string.Empty;

            if (Convert.ToDecimal(nilai_oby_03) > 0)
            {
                fire_multi_desk_oby_3 = @$"
                    <tr style='height: 20px;'>
                        <td>3.</td>
                        <td colspan='2'>{cetakSchedulePolis.desk_oby_03}</td>
                        <td></td>
                        <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                        <td style='width: 18%; text-align: right;'>{nilai_oby_03}</td>
                    </tr>";
            }
            
            var fire_multi_desk_oby_4 = string.Empty;

            if (Convert.ToDecimal(nilai_oby_04) > 0)
            {
                fire_multi_desk_oby_4 = @$"
                    <tr style='height: 20px;'>
                        <td>3.</td>
                        <td colspan='2'>{cetakSchedulePolis.desk_oby_04}</td>
                        <td></td>
                        <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                        <td style='width: 18%; text-align: right;'>{nilai_oby_04}</td>
                    </tr>";
            }
            
            var fire_multi_desk_oby_5 = string.Empty;

            if (Convert.ToDecimal(nilai_oby_05) > 0)
            {
                fire_multi_desk_oby_5 = @$"
                    <tr style='height: 20px;'>
                        <td>3.</td>
                        <td colspan='2'>{cetakSchedulePolis.desk_oby_05}</td>
                        <td></td>
                        <td style='width: 1%;'>{cetakSchedulePolis.kd_mtu_symbol}</td>
                        <td style='width: 18%; text-align: right;'>{nilai_oby_05}</td>
                    </tr>";
            }
            
            #endregion

            #region Cargo Single

            var cargo_single_diskon = string.Empty;

            if (Convert.ToDecimal(nilai_dis) > 0)
            {
                cargo_single_diskon = @$"
                <tr>
                    <td></td>
                    <td></td>
                    <td>Diskon</td>
                    <td></td>
                    <td></td>
                    <td>=</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right;'>{nilai_dis}</td>
                </tr>";
            }

            #endregion

            #region Cargo Multi

            var cargo_multi_diskon = string.Empty;

            if (Convert.ToDecimal(nilai_dis) > 0)
            {
                cargo_multi_diskon = @$"
                <tr>
                    <td></td>
                    <td></td>
                    <td>Diskon</td>
                    <td></td>
                    <td>=</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol} {nilai_dis}</td>
                </tr>";
            }

            #endregion

            var polis_pa_no_pol_lama = string.Empty;

            if (string.IsNullOrWhiteSpace(cetakSchedulePolis.no_pol_lama))
            {
                polis_pa_no_pol_lama = $@"
                    <td style='width: 15%;'></td>
                <td></td>
                <td></td>";
            }
            else
            {
                polis_pa_no_pol_lama = $@"
                    <td style='width: 15%;'>Ex Polis No</td>
                <td>:</td>
                <td>{cetakSchedulePolis.no_pol_lama}</td>";
            }
            
            #region Polis PA Biasa Single

            var polis_pa_single_suku_premi_dasar_first_row = new StringBuilder();

            polis_pa_single_suku_premi_dasar_first_row.Append(
                Convert.ToDouble(pst_a) > 0
                    ? $@"<td style='width: 20%;'>A. {pst_a} {stn_rate_a}</td>"
                    : "<td style='width: 20%;'></td>");

            polis_pa_single_suku_premi_dasar_first_row.Append(
                Convert.ToDouble(pst_c) > 0 ? $@"<td>C. {pst_c} {stn_rate_c}</td>" : "<td></td>");

            var polis_pa_single_suku_premi_dasar_second_row = new StringBuilder();

            polis_pa_single_suku_premi_dasar_second_row.Append(
                Convert.ToDouble(pst_b) > 0 ? $@"<td>B. {pst_b} {stn_rate_b}</td>" : "<td></td>");
            
            polis_pa_single_suku_premi_dasar_second_row.Append(
                Convert.ToDouble(pst_d) > 0 ? $@"<td>D. {pst_d} {stn_rate_d}</td>" : "<td></td>");
            
            var polis_pa_single_nilai_ptg_c = string.Empty;

            if (Convert.ToDouble(nilai_ptg_c) > 0)
            {
                polis_pa_single_nilai_ptg_c = $@"
                <tr>
                    <td style='width: 3.4%; vertical-align: top;'></td>
                    <td style='width: 20%; vertical-align: top;'></td>
                    <td style='width: 1%;'>:</td>
                    <td>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right'>{nilai_ptg_c}</td>
                    <td>x</td>
                    <td style='text-align: right'>{pst_c}</td>
                    <td>{stn_rate_c}</td>
                    <td>=</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right'>{nilai_prm_c}</td>
                </tr>";
            }
            
            var polis_pa_single_nilai_ptg_d = string.Empty;

            if (Convert.ToDouble(nilai_ptg_d) > 0)
            {
                polis_pa_single_nilai_ptg_d = $@"
                <tr>
                    <td style='width: 3.4%; vertical-align: top;'></td>
                    <td style='width: 20%; vertical-align: top;'></td>
                    <td style='width: 1%;'>:</td>
                    <td>- {cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right'>{nilai_ptg_d}</td>
                    <td>x</td>
                    <td style='text-align: right'>{pst_d}</td>
                    <td>{stn_rate_d}</td>
                    <td>=</td>
                    <td>{cetakSchedulePolis.kd_mtu_symbol}</td>
                    <td style='text-align: right'>{nilai_prm_d}</td>
                </tr>";
            }
            
            #endregion

            #region Hole In One Single

            var polis_hole_in_one_single_nilai_oby_02 = string.Empty;

            if (Convert.ToDouble(nilai_oby_02) > 0)
            {
                polis_hole_in_one_single_nilai_oby_02 = $@"
                <tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_02}</td>
                    <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                    <td style='vertical-align: top; text-align: right'>{nilai_oby_02}</td>
                </tr>";
            }

            var polis_hole_in_one_single_nilai_oby_03 = string.Empty;

            if (Convert.ToDouble(nilai_oby_03) > 0)
            {
                polis_hole_in_one_single_nilai_oby_03 = $@"
                <tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_03}</td>
                    <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                    <td style='vertical-align: top; text-align: right'>{nilai_oby_03}</td>
                </tr>";
            }

            var polis_hole_in_one_single_nilai_oby_04 = string.Empty;

            if (Convert.ToDouble(nilai_oby_04) > 0)
            {
                polis_hole_in_one_single_nilai_oby_04 = $@"
                <tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_04}</td>
                    <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                    <td style='vertical-align: top; text-align: right'>{nilai_oby_04}</td>
                </tr>";
            }

            var polis_hole_in_one_single_nilai_oby_05 = string.Empty;

            if (Convert.ToDouble(nilai_oby_05) > 0)
            {
                polis_hole_in_one_single_nilai_oby_05 = $@"
                <tr>
                    <td style='width: 1%; vertical-align: top;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 1%;'></td>
                    <td style='vertical-align: top; text-align: justify; width: 25%;'>{cetakSchedulePolis.desk_oby_05}</td>
                    <td style='width: 1%; vertical-align: top;'>{cetakSchedulePolis.kd_mtu}</td>
                    <td style='vertical-align: top; text-align: right'>{nilai_oby_05}</td>
                </tr>";
            }

            #endregion
            
            var total_nilai_prm_pad = ReportHelper.ConvertToReportFormat((cetakSchedulePolis.nilai_prm_pad ?? 0) 
                                      + (cetakSchedulePolis.nilai_prm_pap ?? 0));
            
            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);
            
            decimal total_sub_nilai_ttl_ptg = 0;
            decimal total_nilai_ttl_ptg = 0;
            decimal total_nitai_ptg = 0;
            decimal total_prm = 0;
            decimal total_jup = 0;
            decimal total_premi = 0;
            decimal total_uang_pertanggungan = 0;
            decimal total_premi_tambahan = 0;
            decimal total_premi_sirama_obyek = 0;
            decimal total_tbh = 0;
            decimal total_prm_tjh = 0;
            decimal total_tjh = 0;
            decimal total_ttg = 0;

            var footer_template = string.Empty;

            if (cetakSchedulePolis.kd_cb.Length >= 4 && cetakSchedulePolis.kd_cb.Substring(3, 1) != "0")
            {
                var ttdImageBase64 = string.Empty;
                var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot", "ttd");
                var imageFile = Path.Combine(wwwroot, reportConfig.NamaTTDFile);
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
                footer_template = $@"<div>
                    $""<img src='data:${mimeType};base64,${ttdImageBase64}' style='max-width: 200px;' />"");
                    <p><strong><u>${reportConfig.NamaPejabat}</u></strong></p>
                    <p><strong>${reportConfig.Jabatan}<strong></p>
                    </div>";
            }
            
            if (ReportHaveDetails.Contains(reportTemplateName))
            {
                StringBuilder stringBuilder = new StringBuilder();
                var sequence = 0;
                foreach (var data in cetakSchedulePolisData)
                {
                    sequence++;
                    stringBuilder.Append(GenerateDetailReport(reportTemplateName, data, sequence));

                    if (data.nilai_ttl_ptg != null && data.nilai_ttl_ptg > 0)
                    {
                        total_sub_nilai_ttl_ptg += data.nilai_ttl_ptg.Value;
                    }
                    
                    if (data.nilai_casco != null && data.nilai_casco > 0)
                    {
                        total_nitai_ptg += data.nilai_casco.Value;
                    }
                    
                    if(data.nilai_prm_casco != null && data.nilai_prm_banjir != null && data.nilai_prm_aog != null &&
                       data.nilai_prm_hh != null && data.nilai_prm_trs != null && data.nilai_prm_pap != null &&
                       data.nilai_prm_pad != null && data.nilai_prm_tjh != null)
                    {
                        total_prm += data.nilai_prm_casco.Value + data.nilai_prm_banjir.Value +
                                     data.nilai_prm_aog.Value + data.nilai_prm_hh.Value + data.nilai_prm_trs.Value + 
                                     data.nilai_prm_pap.Value + data.nilai_prm_pad.Value + data.nilai_prm_tjh.Value;
                    }
                    
                    if(data.nilai_casco != null && data.nilai_tjh != null && data.nilai_tjp != null &&
                       data.nilai_pap != null && data.nilai_pad != null && data.nilai_pap_med != null &&
                       data.nilai_pad_med != null)
                    {
                        total_ttg += data.nilai_casco.Value + data.nilai_tjh.Value + data.nilai_tjp.Value + data.nilai_pap.Value +
                                     data.nilai_pad.Value + data.nilai_pap_med.Value + data.nilai_pad_med.Value;
                    }
                    
                    if (data.jup != null && data.jup > 0)
                    {
                        total_jup += data.jup.Value;
                    }
                    
                    if(data.nilai_prm_A != null && data.nilai_prm_B != null && 
                       data.nilai_prm_C != null && data.nilai_prm_D != null)
                    {
                        total_premi += data.nilai_prm_A.Value + data.nilai_prm_B.Value +
                                     data.nilai_prm_C.Value + data.nilai_prm_D.Value;
                    }
                    
                    if(data.nilai_ptg_A != null && data.nilai_ptg_B != null && 
                       data.nilai_ptg_C != null && data.nilai_ptg_D != null &&
                       data.nilai_ptg_phk != null)
                    {
                        total_uang_pertanggungan += data.nilai_ptg_A.Value + data.nilai_ptg_B.Value +
                                                    data.nilai_ptg_C.Value + data.nilai_ptg_D.Value +
                                                    data.nilai_ptg_phk.Value;
                    }
                    
                    if (data.nilai_ptg_phk != null && data.nilai_ptg_phk > 0)
                    {
                        total_premi_tambahan += data.nilai_ptg_phk.Value;
                    }
                    
                    if(data.nilai_prm_A != null && data.nilai_prm_B != null && 
                       data.nilai_prm_C != null && data.nilai_prm_D != null && 
                       data.nilai_prm_phk != null)
                    {
                        total_premi_sirama_obyek += data.nilai_prm_A.Value + data.nilai_prm_B.Value +
                                                    data.nilai_prm_C.Value + data.nilai_prm_D.Value +
                                                    data.nilai_prm_phk.Value;
                    }
                    
                    if (data.nilai_tjh != null && data.nilai_tjh > 0)
                    {
                        total_tjh += data.nilai_tjh.Value;
                    }
                    
                    if (data.nilai_prm_tjh != null && data.nilai_prm_tjh > 0)
                    {
                        total_prm_tjh += data.nilai_prm_tjh.Value;
                    }
                }
                
                StringBuilder summary = new StringBuilder();
                switch (reportTemplateName)
                {
                    case "LampiranPolisFireDaftarIsi.html":
                        summary.Append($@"
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td style='font-weight: bold; text-align: center;' colspan='2'>TOTAL : </td>
                                            <td style='font-weight: bold;'>{ReportHelper.ConvertToReportFormat(total_sub_nilai_ttl_ptg)}</td>
                                        </tr>");
                        total_nilai_ttl_ptg += total_sub_nilai_ttl_ptg;
                        break;
                    case "LampiranPolisPABiasaDaftarisi.html":
                        summary.Append($@"
                                        <tr>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid' colspan=5><strong>TOTAL</strong></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'><strong>{ReportHelper.ConvertToReportFormat(total_jup)}</strong></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'><strong></strong></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'><strong></strong></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'><strong>{ReportHelper.ConvertToReportFormat(total_premi)}</strong></td>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid'><strong></strong></td>
                                        </tr>");
                        break;
                    case "LampiranPolisPASiramaObyek.html":
                        summary.Append($@"<tr>
                                            <td style='vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid' colspan=7><strong>TOTAL</strong></td>
                                            <td style='width: 10%; vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid; text-align: right'><strong>{ReportHelper.ConvertToReportFormat(total_uang_pertanggungan)}</strong></td>
                                            <td style='width: 10%; vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid; text-align: right'><strong>{ReportHelper.ConvertToReportFormat(total_premi)}</strong></td>
                                            <td style='width: 10%; vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid; text-align: right'><strong>{ReportHelper.ConvertToReportFormat(total_premi_tambahan)}</strong></td>
                                            <td style='width: 15%; vertical-align: top; text-align: left; border-top: 1px solid;border-bottom: 1px solid; text-align: right'><strong>{ReportHelper.ConvertToReportFormat(total_premi_sirama_obyek)}</strong></td>
                                        </tr>");
                        break;
                    case "LampiranPolisMotorListing.html":
                        summary.Append($@"
                            <tr>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-left: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-top: 1px solid;border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_nitai_ptg)}</td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-top: 1px solid;border-bottom: 1px solid;'>{ReportHelper.ConvertToReportFormat(total_prm)}</td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-right: 1px solid;border-bottom: 1px solid;'></td>
                            </tr>");
                        break;
                    case "LampiranPolisMotorDetil.html":
                        summary.Append($@"
                            <hr class='s1'>
                            <!-- Total -->
                            <h3>TOTAL PREMIUM :</h3>
                            <table style='border-collapse: collapse;'>
                              <tr>
                                <td style='width: 32.5%'>HP CASCO</td>
                                <td>: </td>
                                <td style='width: 1px'>{cetakSchedulePolis.symbol}</td>
                                <td style='text-align: right'>{ReportHelper.ConvertToReportFormat(total_ttg - total_tjh)}</td>
                                <td>=</td>
                                <td style='width: 1px'>{cetakSchedulePolis.symbol}</td>
                                <td style='text-align: right'>{ReportHelper.ConvertToReportFormat(total_prm - total_prm_tjh)}</td>
                                <td style='width: 28%'></td>
                              </tr>
                              <tr>
                                <td>HP TJH</td>
                                <td>: </td>
                                <td>{cetakSchedulePolis.symbol}</td>
                                <td style='text-align: right'>{ReportHelper.ConvertToReportFormat(total_tjh)}</td>
                                <td>=</td>
                                <td>{cetakSchedulePolis.symbol}</td>
                                <td style='text-align: right'>{ReportHelper.ConvertToReportFormat(total_prm_tjh)}</td>
                                <td></td>
                              </tr>
                              <tr>
                                <td>PREMI</td>
                                <td>: </td>
                                <td></td>
                                <td></td>
                                <td>=</td>
                                <td>{cetakSchedulePolis.symbol}</td>
                                <td style='text-align: right; border-top: 1px solid; border-bottom: 1px double'>{ReportHelper.ConvertToReportFormat(total_prm - total_prm_tjh + total_prm_tjh)}</td>
                                <td></td>
                              </tr>
                            </table>");
                        break;
                }

                if (cetakSchedulePolis != null)
                    resultTemplate = templateProfileResult.Render(new
                    {
                        cetakSchedulePolis.no_pol_ttg, cetakSchedulePolis.nm_ttg,
                        cetakSchedulePolis.almt_ttg, cetakSchedulePolis.kd_pos,
                        cetakSchedulePolis.almt_rsk, cetakSchedulePolis.kd_pos_rsk,
                        cetakSchedulePolis.jk_wkt_ptg, cetakSchedulePolis.tgl_mul_ptg_ind,
                        cetakSchedulePolis.tgl_akh_ptg_ind, cetakSchedulePolis.nm_okup,
                        cetakSchedulePolis.kd_okup, cetakSchedulePolis.nm_kls_konstr,
                        pst_rate_prm_pkk, stn_rate_prm_pkk,
                        cetakSchedulePolis.kd_mtu_symbol, nilai_prk_pkk,
                        cetakSchedulePolis.nm_cvrg_01, cetakSchedulePolis.kd_cvrg_01,
                        pst_rate_prm_01, stn_rate_prm_01,
                        nilai_prm_tbh_01,cetakSchedulePolis.nm_cvrg_02, 
                        cetakSchedulePolis.kd_cvrg_02, pst_rate_prm_02, 
                        stn_rate_prm_02, nilai_prm_tbh_02,
                        cetakSchedulePolis.ket_dis, nilai_dis,
                        nilai_bia_pol, nilai_bia_mat, nilai_prm_pa,
                        cetakSchedulePolis.ket_klausula, nilai_ttl,
                        cetakSchedulePolis.desk_oby_01, nilai_oby_01,
                        cetakSchedulePolis.desk_oby_02, nilai_oby_02,
                        cetakSchedulePolis.desk_oby_03, nilai_oby_03,
                        cetakSchedulePolis.desk_oby_04, nilai_oby_04,
                        cetakSchedulePolis.desk_oby_05, nilai_oby_05,
                        nilai_ttl_ptg, cetakSchedulePolis.tgl_closing_ind,
                        cetakSchedulePolis.stnc, cetakSchedulePolis.ctt_pol,
                        cetakSchedulePolis.kt_cb, cetakSchedulePolis.nm_user,
                        cetakSchedulePolis.no_msn, cetakSchedulePolis.no_rangka,
                        cetakSchedulePolis.no_pls, cetakSchedulePolis.nilai_prm_pkm,
                        cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
                        cetakSchedulePolis.desk_aksesories, cetakSchedulePolis.nm_utk,
                        cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                        cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                        cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                        cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                        cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                        sub_total_kebakaran, cetakSchedulePolis.cover, cetakSchedulePolis.tgl_closing,
                        cetakSchedulePolis.no_pol_lama,cetakSchedulePolis.st_pas,
                        nilai_prm_pkk,nilai_prm_tbh_03, footer_template,
                        nilai_prm_tbh_04, nilai_prm_tbh_05,
                        cetakSchedulePolis.kd_cvrg_03, cetakSchedulePolis.kd_cvrg_04,
                        cetakSchedulePolis.kd_cvrg_05,cetakSchedulePolis.nm_cvrg_03,
                        cetakSchedulePolis.nm_cvrg_04, cetakSchedulePolis.nm_cvrg_05,
                        pst_rate_prm_03, pst_rate_prm_04,
                        pst_rate_prm_05,stn_rate_prm_03,
                        stn_rate_prm_04,stn_rate_prm_05,
                        cetakSchedulePolis.deduct,cetakSchedulePolis.lamp_pol,
                        cetakSchedulePolis.nm_merk_kend,cetakSchedulePolis.tipe_kend,
                        cetakSchedulePolis.thn_buat,cetakSchedulePolis.nilai_casco, 
                        nilai_aksesories,nilai_tjh, suku_premi,
                        cetakSchedulePolis.kd_jns_ptg,nilai_pad,
                        nilai_pap, nilai_tjp, nilai_rsk_sendiri,cetakSchedulePolis.desk_deduct,
                        nilai_prm_casco,nilai_prm_tjh,
                        nilai_prm_pad,nilai_prm_banjir,
                        nilai_prm_aog,nilai_prm_hh, percentage_diskon,
                        nilai_prm_trs,nilai_prm_tjp,
                        cetakSchedulePolis.no_pol,cetakSchedulePolis.consignee,
                        cetakSchedulePolis.no_po,cetakSchedulePolis.no_lc,
                        cetakSchedulePolis.tempat_brkt,cetakSchedulePolis.tempat_tiba,
                        cetakSchedulePolis.nm_kapal,cetakSchedulePolis.tgl_brkt,
                        cetakSchedulePolis.no_inv,cetakSchedulePolis.no_bl,
                        cetakSchedulePolis.desk_kond,pst_rate_prm,
                        stn_rate_prm,nilai_prm, cetakSchedulePolis.ket_rincian_kontr,
                        nilai_bond,cetakSchedulePolis.nm_obl,
                        cetakSchedulePolis.almt_obl,cetakSchedulePolis.ket_nilai_bond,
                        cetakSchedulePolis.ba_srh_trm,cetakSchedulePolis.tgl_kontrak,
                        cetakSchedulePolis.ket_rinc_kontr,cetakSchedulePolis.jml_hari,
                        cetakSchedulePolis.nm_principal,cetakSchedulePolis.jbt_principal,
                        cetakSchedulePolis.nm_surety,cetakSchedulePolis.jbt_surety,
                        charge, cetakSchedulePolis.footer,
                        cetakSchedulePolis.nm_polis,cetakSchedulePolis.almt_polis,
                        cetakSchedulePolis.tgl_lahir,cetakSchedulePolis.nm_waris_1,
                        cetakSchedulePolis.nm_waris_2,cetakSchedulePolis.nm_waris_3,
                        cetakSchedulePolis.hub_waris_1,cetakSchedulePolis.hub_waris_2,
                        cetakSchedulePolis.hub_waris_3,cetakSchedulePolis.jk_wkt_ptg_ind,
                        nilai_ptg_a,nilai_ptg_b, nilai_ptg_c,pst_a,
                        pst_b,pst_c, pst_d,cetakSchedulePolis.pst_E,
                        stn_rate_a, stn_rate_b, stn_rate_c,stn_rate_d,
                        cetakSchedulePolis.stn_rate_E, nilai_ptg_d,
                        nilai_prm_a,nilai_prm_b, nilai_prm_c,nilai_prm_d,
                        cetakSchedulePolis.obyek_ptg,cetakSchedulePolis.ket_event,
                        cetakSchedulePolis.lokasi,cetakSchedulePolis.jml_peserta,
                        tgl_mul_ptg,tgl_akh_ptg,
                        cetakSchedulePolis.spek_hole,cetakSchedulePolis.ket_hadiah,
                        cetakSchedulePolis.own_risk,cetakSchedulePolis.kd_mtu,
                        cetakSchedulePolis.kota_cab,nilai_ttl_prm,
                        cetakSchedulePolis.no_rsk,cetakSchedulePolis.nm_deb,
                        cetakSchedulePolis.alm_lok_ptg,cetakSchedulePolis.kd_usr,
                        cetakSchedulePolis.tmp_lahir, cetakSchedulePolis.no_pol_induk,
                        view_prm_tbh_01, view_prm_tbh_02, view_prm_tbh_03, view_prm_tbh_04,
                        view_prm_tbh_05, view_nilai_dis, view_row_desk_oby_01, view_row_desk_oby_02,
                        view_row_desk_oby_03, view_row_desk_oby_04, view_row_desk_oby_05, nilai_kendaraan_bermotor,
                        view_pst_stn_a, view_pst_stn_b, view_pst_stn_c, view_pst_stn_d, view_jns_nilai_ptg_a,
                        view_jns_nilai_ptg_b, view_jns_nilai_ptg_c, view_jns_nilai_ptg_d, view_perhitungan_prm_a,
                        view_perhitungan_prm_b, view_perhitungan_prm_c, view_perhitungan_prm_d, view_desk_oby_2,
                        view_desk_oby_3, view_desk_oby_4, view_desk_oby_5, detail = stringBuilder.ToString(),
                        summary, cetakSchedulePolis.nm_cb, total_nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(total_nilai_ttl_ptg),
                    });
            }
            else
            {
                resultTemplate = templateProfileResult.Render( new
                {
                        cetakSchedulePolis.no_pol_ttg, cetakSchedulePolis.nm_ttg,
                        cetakSchedulePolis.almt_ttg, cetakSchedulePolis.kd_pos,
                        cetakSchedulePolis.almt_rsk, cetakSchedulePolis.kd_pos_rsk,
                        cetakSchedulePolis.jk_wkt_ptg, cetakSchedulePolis.tgl_mul_ptg_ind,
                        cetakSchedulePolis.tgl_akh_ptg_ind, cetakSchedulePolis.nm_okup,
                        cetakSchedulePolis.kd_okup, cetakSchedulePolis.nm_kls_konstr,
                        pst_rate_prm_pkk, stn_rate_prm_pkk, div_no_pol_lama, cargo_single_diskon,
                        cetakSchedulePolis.kd_mtu_symbol, nilai_prk_pkk, cargo_multi_diskon,
                        cetakSchedulePolis.nm_cvrg_01, cetakSchedulePolis.kd_cvrg_01,
                        pst_rate_prm_01, stn_rate_prm_01, no_renew, polis_pa_no_pol_lama,
                        nilai_prm_tbh_01,cetakSchedulePolis.nm_cvrg_02,
                        cetakSchedulePolis.kd_cvrg_02, pst_rate_prm_02, 
                        stn_rate_prm_02, nilai_prm_tbh_02, subTotal, polis_hole_in_one_single_nilai_oby_05,
                        cetakSchedulePolis.ket_dis, nilai_dis, total_nilai_prm_pad,
                        nilai_bia_pol, nilai_bia_mat, nilai_prm_pa, polis_hole_in_one_single_nilai_oby_04,
                        cetakSchedulePolis.ket_klausula, nilai_ttl, polis_hole_in_one_single_nilai_oby_03,
                        cetakSchedulePolis.desk_oby_01, nilai_oby_01, polis_hole_in_one_single_nilai_oby_02,
                        cetakSchedulePolis.desk_oby_02, nilai_oby_02,
                        cetakSchedulePolis.desk_oby_03, nilai_oby_03,
                        cetakSchedulePolis.desk_oby_04, nilai_oby_04,
                        cetakSchedulePolis.desk_oby_05, nilai_oby_05,
                        nilai_ttl_ptg, cetakSchedulePolis.tgl_closing_ind,
                        cetakSchedulePolis.stnc, cetakSchedulePolis.ctt_pol,
                        cetakSchedulePolis.kt_cb, cetakSchedulePolis.nm_user,
                        cetakSchedulePolis.no_msn, cetakSchedulePolis.no_rangka,
                        cetakSchedulePolis.no_pls, cetakSchedulePolis.nilai_prm_pkm,
                        cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
                        cetakSchedulePolis.desk_aksesories, cetakSchedulePolis.nm_utk,
                        cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                        cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                        cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                        cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                        cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                        sub_total_kebakaran, cetakSchedulePolis.cover, cetakSchedulePolis.tgl_closing,
                        cetakSchedulePolis.no_pol_lama,cetakSchedulePolis.st_pas,
                        nilai_prm_pkk,nilai_prm_tbh_03, polis_pa_single_suku_premi_dasar_first_row = polis_pa_single_suku_premi_dasar_first_row.ToString(),
                        nilai_prm_tbh_04, nilai_prm_tbh_05, polis_pa_single_suku_premi_dasar_second_row = polis_pa_single_suku_premi_dasar_second_row.ToString(),
                        cetakSchedulePolis.kd_cvrg_03, cetakSchedulePolis.kd_cvrg_04,
                        cetakSchedulePolis.kd_cvrg_05,cetakSchedulePolis.nm_cvrg_03,
                        cetakSchedulePolis.nm_cvrg_04, cetakSchedulePolis.nm_cvrg_05,
                        pst_rate_prm_03, pst_rate_prm_04, polis_pa_single_nilai_ptg_c,
                        pst_rate_prm_05,stn_rate_prm_03, polis_pa_single_nilai_ptg_d,
                        stn_rate_prm_04,stn_rate_prm_05,
                        cetakSchedulePolis.deduct,cetakSchedulePolis.lamp_pol,
                        cetakSchedulePolis.nm_merk_kend,cetakSchedulePolis.tipe_kend,
                        cetakSchedulePolis.thn_buat,cetakSchedulePolis.nilai_casco, 
                        nilai_aksesories,nilai_tjh, suku_premi,
                        cetakSchedulePolis.kd_jns_ptg,nilai_pad,
                        nilai_pap, nilai_tjp, nilai_rsk_sendiri,cetakSchedulePolis.desk_deduct,
                        nilai_prm_casco,nilai_prm_tjh, footer_template,
                        nilai_prm_pad,nilai_prm_banjir,
                        nilai_prm_aog,nilai_prm_hh, percentage_diskon,
                        nilai_prm_trs,nilai_prm_tjp,
                        cetakSchedulePolis.no_pol,cetakSchedulePolis.consignee,
                        cetakSchedulePolis.no_po,cetakSchedulePolis.no_lc,
                        cetakSchedulePolis.tempat_brkt,cetakSchedulePolis.tempat_tiba,
                        cetakSchedulePolis.nm_kapal,cetakSchedulePolis.tgl_brkt,
                        cetakSchedulePolis.no_inv,cetakSchedulePolis.no_bl,
                        cetakSchedulePolis.desk_kond,pst_rate_prm,
                        stn_rate_prm,nilai_prm, cetakSchedulePolis.no_kontr,
                        nilai_bond,cetakSchedulePolis.nm_obl, cetakSchedulePolis.ket_rincian_kontr,
                        cetakSchedulePolis.almt_obl,cetakSchedulePolis.ket_nilai_bond,
                        cetakSchedulePolis.ba_srh_trm,cetakSchedulePolis.tgl_kontrak,
                        cetakSchedulePolis.ket_rinc_kontr, cetakSchedulePolis.jml_hari,
                        cetakSchedulePolis.nm_principal,cetakSchedulePolis.jbt_principal,
                        cetakSchedulePolis.nm_surety,cetakSchedulePolis.jbt_surety,
                        charge, cetakSchedulePolis.footer,
                        cetakSchedulePolis.nm_polis,cetakSchedulePolis.almt_polis,
                        cetakSchedulePolis.tgl_lahir,cetakSchedulePolis.nm_waris_1,
                        cetakSchedulePolis.nm_waris_2,cetakSchedulePolis.nm_waris_3,
                        cetakSchedulePolis.hub_waris_1,cetakSchedulePolis.hub_waris_2,
                        cetakSchedulePolis.hub_waris_3,cetakSchedulePolis.jk_wkt_ptg_ind,
                        nilai_ptg_a,nilai_ptg_b, nilai_ptg_c,pst_a,
                        pst_b,pst_c, pst_d,cetakSchedulePolis.pst_E,
                        stn_rate_a, stn_rate_b, stn_rate_c,stn_rate_d,
                        cetakSchedulePolis.stn_rate_E, nilai_ptg_d,
                        nilai_prm_a,nilai_prm_b, nilai_prm_c,nilai_prm_d,
                        cetakSchedulePolis.obyek_ptg,cetakSchedulePolis.ket_event,
                        cetakSchedulePolis.lokasi,cetakSchedulePolis.jml_peserta,
                        tgl_mul_ptg,tgl_akh_ptg,
                        cetakSchedulePolis.spek_hole,cetakSchedulePolis.ket_hadiah,
                        cetakSchedulePolis.own_risk,cetakSchedulePolis.kd_mtu,
                        cetakSchedulePolis.kota_cab,nilai_ttl_prm,
                        cetakSchedulePolis.no_rsk,cetakSchedulePolis.nm_deb,
                        cetakSchedulePolis.alm_lok_ptg,cetakSchedulePolis.kd_usr,
                        cetakSchedulePolis.tmp_lahir,
                        view_prm_tbh_01, view_prm_tbh_02, view_prm_tbh_03, view_prm_tbh_04,
                        view_prm_tbh_05, view_nilai_dis, view_row_desk_oby_01, view_row_desk_oby_02,
                        view_row_desk_oby_03, view_row_desk_oby_04, view_row_desk_oby_05, nilai_kendaraan_bermotor,
                        view_pst_stn_a, view_pst_stn_b, view_pst_stn_c, view_pst_stn_d, view_jns_nilai_ptg_a,
                        view_jns_nilai_ptg_b, view_jns_nilai_ptg_c, view_jns_nilai_ptg_d, view_perhitungan_prm_a,
                        view_perhitungan_prm_b, view_perhitungan_prm_c, view_perhitungan_prm_d, view_desk_oby_2,
                        view_desk_oby_3, view_desk_oby_4, view_desk_oby_5, total_nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(total_nilai_ttl_ptg),
                        fire_single_nm_cvrg_01_td, fire_single_nm_cvrg_02_td, fire_single_nm_cvrg_03_td, fire_single_nm_cvrg_04_td, fire_single_nm_cvrg_05_td,
                        fire_single_desk_oby_3_td, fire_single_desk_oby_4_td, fire_single_desk_oby_5_td, title5 = reportConfig.Title.Title5,
                        fire_multi_cvrg_01, fire_multi_cvrg_02, fire_multi_cvrg_03, fire_multi_cvrg_04, fire_multi_cvrg_05,
                        fire_multi_pst_rate_01, fire_multi_pst_rate_02, fire_multi_pst_rate_03, fire_multi_pst_rate_04, fire_multi_pst_rate_05,
                        fire_multi_desk_oby_3, fire_multi_desk_oby_4, fire_multi_desk_oby_5
                } );
            }
            
            return resultTemplate;
        }
        
        private string GenerateDetailReport(string reportType, CetakSchedulePolisDto data, int sequence)
        {
            var stn_rate_A = data.stn_rate_A == 1 ? "%" : "%o";
            var pst_rate = ReportHelper.ConvertToReportFormat(data.pst_rate, true);
            var stn_rate_prm = data.stn_rate_prm == "1" ? "%" : "%o";
            var pst_rate_prm = ReportHelper.ConvertToReportFormat(data.pst_rate_prm, true);
            var stn_rate_banjir = data.stn_rate_banjir == 1 ? "%" : "%o";
            var pst_rate_banjir = ReportHelper.ConvertToReportFormat(data.pst_rate_banjir, true);
            var stn_rate_aog = data.stn_rate_aog == 1 ? "%" : "%o";
            var pst_rate_aog = ReportHelper.ConvertToReportFormat(data.pst_rate_aog, true);
            var stn_rate_hh = data.stn_rate_hh == 1 ? "%" : "%o";
            var pst_rate_hh = ReportHelper.ConvertToReportFormat(data.pst_rate_hh, true);
            var stn_rate_trs = data.stn_rate_trs == 1 ? "%" : "%o";
            var pst_rate_trs = ReportHelper.ConvertToReportFormat(data.pst_rate_trs, true);
            var tgl_mul_ptg = DateTime.TryParse(data.tgl_mul_ptg, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(data.tgl_mul_ptg), "dd/MM/yyyy")
                : data.tgl_mul_ptg;
            var tgl_akh_ptg = DateTime.TryParse(data.tgl_akh_ptg, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(data.tgl_akh_ptg), "dd/MM/yyyy")
                : data.tgl_akh_ptg;
            var tgl_brkt = DateTime.TryParse(data.tgl_brkt, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(data.tgl_brkt), "dd/MM/yyyy")
                : data.tgl_brkt;
            var tgl_mul_ptg_ind = DateTime.TryParse(data.tgl_mul_ptg_ind, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(data.tgl_mul_ptg_ind), "dd/MM/yyyy")
                : data.tgl_mul_ptg_ind;
            var tgl_akh_ptg_ind = DateTime.TryParse(data.tgl_akh_ptg_ind, out _)
                ? ReportHelper.ConvertDateTime(DateTime.Parse(data.tgl_akh_ptg_ind), "dd/MM/yyyy")
                : data.tgl_akh_ptg_ind;
            var pst_deduct = ReportHelper.ConvertToReportFormat(data.pst_deduct, true);
            switch (reportType)
            {
                case "LampiranPolisFireDaftarIsi.html":
                    var kd_penerangan = data.kd_penerangan == "1" ? "Listrik" : "Lain Lain";
                    var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
                    return @$"<tr style='height: 50px;'>
                        <td style='font-weight: bold;' colspan='8'>{data.nm_grp_oby}</td>
                        </tr>
                        <tr>
                            <td style='width: 3%;  text-align: right; vertical-align: top;' rowspan='2'>{data.no_oby}</td>
                            <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.desk_oby}</td>
                            <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.almt_rsk}</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.ket_okup}</td>
                            <td style='width: 5%;  text-align: center; vertical-align: top;'>{data.kd_kls_konstr}</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{kd_penerangan}</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.symbol}</td>
                            <td style='width: 10%; text-align: center; vertical-align: top;' rowspan='2'>{nilai_ttl_ptg}</td>
                        </tr>
                        <tr>
                            <td style='vertical-align: top; text-align: center;'>{data.kd_zona}</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style='font-weight: bold; text-align: center;' colspan='2'>{data.nm_grp_oby} : </td>
                            <td style='font-weight: bold; text-align: right;'>{nilai_ttl_ptg}</td>
                        </tr>";
                case "LampiranPolisPASiramaObyek.html":
                    return @$"
                                <tr>
                                    <td style='vertical-align: top; text-align: center;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: left;'>{data.nm_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{ReportHelper.ConvertDateTime(data.tgl_lahir, "dd/MM/yyyy")}</td>
                                    <td style='vertical-align: top; text-align: center;'>{tgl_mul_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{tgl_akh_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.jk_wkt}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.usia}</td>
                                    <td style='width: 10%; vertical-align: top; text-align: right;'>{ReportHelper.ConvertToReportFormat(data.nilai_ptg_A + data.nilai_ptg_B + data.nilai_ptg_C + data.nilai_ptg_D + data.nilai_ptg_phk)}</td>
                                    <td style='width: 10%; vertical-align: top; text-align: right;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_A + data.nilai_prm_B + data.nilai_prm_C + data.nilai_prm_D )}</td>
                                    <td style='width: 10%; vertical-align: top; text-align: right;'>{ReportHelper.ConvertToReportFormat(data.nilai_ptg_phk)}</td>
                                    <td style='width: 15%; vertical-align: top; text-align: right;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_A + data.nilai_prm_B + data.nilai_prm_C + data.nilai_prm_D + data.nilai_prm_phk)}</td>
                                </tr>";
                case "LampiranPolisPABiasaDaftarisi.html":
                    return @$"
                                <tr>
                                    <td style='vertical-align: top; text-align: center;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.nm_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.almt_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_lahir}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.usia}</td>
                                    <td style='vertical-align: top; text-align: center;'>{ReportHelper.ConvertToReportFormat(data.jup)}</td>
                                    <td style='vertical-align: top; text-align: center;'>{tgl_mul_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{tgl_akh_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{pst_rate} {stn_rate_A}</td>
                                    <td style='vertical-align: top; text-align: center;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_A + data.nilai_prm_B + data.nilai_prm_C + data.nilai_prm_D)}</td>
                                    <td style='vertical-align: top; text-align: center;'></td>
                                </tr>";
                case "LampiranPolisCargoDaftarisi.html":
                    return @$"<tr>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.jns_brg}</td>
                                    <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.penerima_brg}<br>{data.tempat_brkg} / {data.tempate_tiba}</td>
                                    <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.nm_kapal}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{tgl_brkt}<br>{data.desk_kond}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm)}<br>{pst_rate_prm} {stn_rate_prm}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{pst_deduct}</td>
                                </tr>";
                case "LampiranPolisMotorListing.html":
                    
                    return @$"<tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{sequence}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.nm_jns_kend}/{data.nm_merk_kend}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.thn_buat}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.no_rangka}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{pst_rate_prm}{stn_rate_prm}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_casco)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_rsk_sendiri)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_pap)}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.no_msn}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.nm_utk}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_tjh)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_pad)}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.tipe_kend}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.jml_tempat_ddk}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.perlengkapan}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>Banjir</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{pst_rate_banjir} {stn_rate_banjir}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_banjir)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_tjp)}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.kd_jns_ptg}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.nm_pemilik}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{data.no_pls}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>Gempa Bumi</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{pst_rate_aog} {stn_rate_aog}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_aog)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'>{tgl_mul_ptg} s/d {tgl_akh_ptg}</td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>Huru-Hara</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{pst_rate_hh} {stn_rate_hh}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_hh)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>TRS</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{pst_rate_trs} {stn_rate_trs}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_trs)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>Kec. Diri Pap.</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>0,500 %</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pap)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>Kec. Diri Png.</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>1,000 %</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pad)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: left;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>TJH</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_tjh)}</td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: right;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>";
                case "LampiranPolisMotorDetil.html":
                    return @$"
<h3>BENTUK PERTANGGUNGAN : [ E ] ALL RISK</h3>
                            <table style='border-collapse: collapse;'>
                              <tr>
                                <td style='width: 5%;'>{sequence}</td>
                                <td style='width: 27.5%;'>MERK / JENIS / TAHUN / WARNA</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 60%;' colspan='7'></td>
                                <td colspan='4' style='text-align: right'>Rate : {pst_rate_prm} {stn_rate_prm}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>NOMOR POLISI</td>
                                <td>:</td>
                                <td colspan='10'>{data.no_pls}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PENGGUNAAN KENDARAAN</td>
                                <td>:</td>
                                <td colspan='10'>{data.nm_utk}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>NO. RANGKA / MESIN</td>
                                <td>:</td>
                                <td colspan='10'>{data.no_rangka} / {data.no_msn}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>JUMLAH TEMPAT DUDUK</td>
                                <td>:</td>
                                <td colspan='10'>{data.jml_tempat_ddk} Orang</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>RESIKO SENDIRI</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='1' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_rsk_sendiri)}</td>
                                <td colspan='8'></td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>MASA PERTANGGUNGAN</td>
                                <td>:</td>
                                <td colspan='19'>{data.jk_wkt_ptg} hari</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>HARGA PERTANGGUNGAN</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 12.5%;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>x</td>
                                <td style='width: 10%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.pst_rate_prm, true)} %</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_casco)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>TJH PIHAK KETIGA</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_tjh)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td style='width: 1%;'></td>
                                <td style='width: 10%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_tjh)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Huru-hara</td>
                                <td>:</td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 12.5%;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>x</td>
                                <td style='width: 10%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.pst_rate_hh, true)} %</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_hh_1)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Bencana Alam</td>
                                <td>:</td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 12.5%;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>x</td>
                                <td style='width: 10%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.pst_rate_aog, true)} %</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_aog)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Banjir</td>
                                <td>:</td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 12.5%;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>x</td>
                                <td style='width: 10%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.pst_rate_banjir, true)} %</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_banjir)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi TRS</td>
                                <td>:</td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 12.5%;'>{ReportHelper.ConvertToReportFormat(data.nilai_casco)}</td>
                                <td style='width: 1%;'>x</td>
                                <td style='width: 10%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.pst_rate_trs, true)} %</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_trs)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PA Penumpang</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_pap)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pap)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PA Pengemudi</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_pad)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pad)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>TJH Penumpang Pihak Ketiga</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_tjp)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_tkp)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>ME Penumpang</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_pap_med)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pap_med)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>ME Pengemudi</td>
                                <td>:</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td style='width: 15%; text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_pad_med)}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 12.5%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 1%;'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_pad_med)}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%; border-top: 1px solid; border-bottom: 1px double'>{data.symbol}</td>
                                <td style='width: 12.5%; text-align: right; border-top: 1px solid; border-bottom: 1px double'>{ReportHelper.ConvertToReportFormat(data.nilai_casco + data.nilai_tjh + data.nilai_tjp + data.nilai_pap + data.nilai_pad + data.nilai_pap_med + data.nilai_pad_med)}</td>
                                <td style='width: 1%;'></td>
                                <td></td>
                                <td></td>
                                <td style='width: 1%;'></td>
                                <td style='width: 1%; border-top: 1px solid; border-bottom: 1px double'>{data.symbol}</td>
                                <td colspan='2' style='text-align: right; border-top: 1px solid; border-bottom: 1px double'>{ReportHelper.ConvertToReportFormat(data.nilai_prm_casco + data.nilai_prm_tjh + data.nilai_prm_tjp + data.nilai_prm_pap + data.nilai_prm_pad + data.nilai_prm_pap_med + data.nilai_prm_pad_med + data.nilai_prm_hh + data.nilai_prm_banjir + data.nilai_prm_trs + data.nilai_prm_aog)}</td>
                              </tr>
                            </table>";
                default:
                    return string.Empty;
            }
        }

        private string GenerateMultipleReport(string reportType, List<CetakSchedulePolisDto> datas, string template)
        {
            Template templateProfileResult = Template.Parse( template );
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Constant.HeaderReport);
            switch (reportType)
            {
                case "PolisPASiramaMulti.html":
                    foreach (var cetakSchedulePolis in datas)
                    {
                        var nilai_ptg_a = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ptg_A);
                        var tgl_mul_ptg = DateTime.TryParse(cetakSchedulePolis.tgl_mul_ptg, out _)
                            ? ReportHelper.ConvertDateTime(DateTime.Parse(cetakSchedulePolis.tgl_mul_ptg), "dd-MMMM-yyyy")
                            : cetakSchedulePolis.tgl_mul_ptg;
                        var tgl_akh_ptg = DateTime.TryParse(cetakSchedulePolis.tgl_akh_ptg, out _)
                            ? ReportHelper.ConvertDateTime(DateTime.Parse(cetakSchedulePolis.tgl_akh_ptg), "dd-MMMM-yyyy")
                            : cetakSchedulePolis.tgl_akh_ptg;
                        var nilai_prm_pkk = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_pkk);
                        var nilai_prm_tbh_01 = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_prm_tbh_01);
                        var nilai_bia_pol = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_bia_pol);
                        var nilai_bia_mat = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_bia_mat);
                        var nilai_ttl = ReportHelper.ConvertToReportFormat(cetakSchedulePolis.nilai_ttl);
                        
                        stringBuilder.Append(templateProfileResult.Render(new
                        {
                            cetakSchedulePolis.no_pol_ttg, cetakSchedulePolis.nm_ttg,
                            cetakSchedulePolis.almt_ttg, cetakSchedulePolis.kd_pos,
                            cetakSchedulePolis.almt_rsk, cetakSchedulePolis.kd_pos_rsk,
                            cetakSchedulePolis.jk_wkt_ptg, cetakSchedulePolis.tgl_mul_ptg_ind,
                            cetakSchedulePolis.tgl_akh_ptg_ind, cetakSchedulePolis.nm_okup,
                            cetakSchedulePolis.kd_okup, cetakSchedulePolis.nm_kls_konstr,
                            cetakSchedulePolis.pst_rate_prm_pkk, cetakSchedulePolis.stn_rate_prm_pkk,
                            cetakSchedulePolis.kd_mtu_symbol, cetakSchedulePolis.nilai_prk_pkk,
                            cetakSchedulePolis.nm_cvrg_01, cetakSchedulePolis.kd_cvrg_01,
                            cetakSchedulePolis.pst_rate_prm_01, cetakSchedulePolis.stn_rate_prm_01,
                            nilai_prm_tbh_01, cetakSchedulePolis.nm_cvrg_02,
                            cetakSchedulePolis.kd_cvrg_02, cetakSchedulePolis.pst_rate_prm_02,
                            cetakSchedulePolis.stn_rate_prm_02, cetakSchedulePolis.nilai_prm_tbh_02,
                            cetakSchedulePolis.ket_dis, cetakSchedulePolis.nilai_dis,
                            nilai_bia_pol, nilai_bia_mat,
                            nilai_ttl, cetakSchedulePolis.ket_klausula,
                            cetakSchedulePolis.desk_oby_01, cetakSchedulePolis.nilai_oby_01,
                            cetakSchedulePolis.desk_oby_02, cetakSchedulePolis.nilai_oby_02,
                            cetakSchedulePolis.desk_oby_03, cetakSchedulePolis.nilai_oby_03,
                            cetakSchedulePolis.desk_oby_04, cetakSchedulePolis.nilai_oby_04,
                            cetakSchedulePolis.desk_oby_05, cetakSchedulePolis.nilai_oby_05,
                            cetakSchedulePolis.nilai_ttl_ptg, cetakSchedulePolis.tgl_closing_ind,
                            cetakSchedulePolis.stnc, cetakSchedulePolis.ctt_pol,
                            cetakSchedulePolis.kt_cb, cetakSchedulePolis.nm_user,
                            cetakSchedulePolis.no_msn, cetakSchedulePolis.no_rangka,
                            cetakSchedulePolis.no_pls, cetakSchedulePolis.nilai_prm_pkm,
                            cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_merk_kend,
                            cetakSchedulePolis.desk_aksesories, cetakSchedulePolis.nm_utk,
                            cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                            cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                            cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                            cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                            cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                            cetakSchedulePolis.thn_buat, cetakSchedulePolis.nm_jns_kend,
                            cetakSchedulePolis.footer, cetakSchedulePolis.desk_deduct,
                            cetakSchedulePolis.nilai_prm_casco, cetakSchedulePolis.tempat_tiba,
                            cetakSchedulePolis.tempat_brkt,nilai_prm_pkk,
                            cetakSchedulePolis.nilai_prm_tbh_03,cetakSchedulePolis.nilai_prm_tbh_04,
                            cetakSchedulePolis.nilai_prm_tbh_05,cetakSchedulePolis.kd_cvrg_03,
                            cetakSchedulePolis.kd_cvrg_04,cetakSchedulePolis.kd_cvrg_05,
                            cetakSchedulePolis.nm_cvrg_03,cetakSchedulePolis.nm_cvrg_04,
                            cetakSchedulePolis.nm_cvrg_05,cetakSchedulePolis.pst_rate_prm_03,
                            cetakSchedulePolis.pst_rate_prm_04,cetakSchedulePolis.pst_rate_prm_05,
                            cetakSchedulePolis.stn_rate_prm_03,cetakSchedulePolis.stn_rate_prm_04,
                            cetakSchedulePolis.stn_rate_prm_05,cetakSchedulePolis.deduct,
                            cetakSchedulePolis.lamp_pol,cetakSchedulePolis.tipe_kend,
                            cetakSchedulePolis.nilai_casco,cetakSchedulePolis.nilai_aksesories,
                            cetakSchedulePolis.nilai_tjh,cetakSchedulePolis.kd_jns_ptg,
                            cetakSchedulePolis.nilai_pad,cetakSchedulePolis.nilai_pap,
                            cetakSchedulePolis.nilai_tjp,cetakSchedulePolis.nilai_rsk_sendiri,
                            cetakSchedulePolis.nilai_prm_tjh,cetakSchedulePolis.nilai_prm_pad,
                            cetakSchedulePolis.nilai_prm_banjir,cetakSchedulePolis.nilai_prm_aog,
                            cetakSchedulePolis.nilai_prm_hh,cetakSchedulePolis.nilai_prm_trs,
                            cetakSchedulePolis.nilai_prm_tjp,cetakSchedulePolis.no_pol,
                            cetakSchedulePolis.consignee,cetakSchedulePolis.no_po,
                            cetakSchedulePolis.no_lc,cetakSchedulePolis.nm_kapal,
                            cetakSchedulePolis.tgl_brkt,cetakSchedulePolis.no_inv,
                            cetakSchedulePolis.no_bl,cetakSchedulePolis.desk_kond,
                            cetakSchedulePolis.pst_rate_prm,cetakSchedulePolis.stn_rate_prm,
                            cetakSchedulePolis.nilai_prm,cetakSchedulePolis.nilai_bond,
                            cetakSchedulePolis.nm_obl,cetakSchedulePolis.almt_obl,
                            cetakSchedulePolis.ket_nilai_bond,cetakSchedulePolis.ba_srh_trm,
                            cetakSchedulePolis.tgl_kontrak,cetakSchedulePolis.ket_rinc_kontr,
                            cetakSchedulePolis.jml_hari,cetakSchedulePolis.nm_principal,
                            cetakSchedulePolis.jbt_principal,cetakSchedulePolis.nm_surety,
                            cetakSchedulePolis.jbt_surety,cetakSchedulePolis.charge,
                            cetakSchedulePolis.no_pol_lama,cetakSchedulePolis.nm_polis,
                            tgl_lahir = ReportHelper.ConvertDateTime(cetakSchedulePolis.tgl_lahir, "dd MMMM yyyy"),cetakSchedulePolis.nm_waris_1,
                            cetakSchedulePolis.nm_waris_2,cetakSchedulePolis.nm_waris_3,
                            cetakSchedulePolis.hub_waris_1,cetakSchedulePolis.hub_waris_2,
                            cetakSchedulePolis.hub_waris_3,cetakSchedulePolis.jk_wkt_ptg_ind,
                            nilai_ptg_a,cetakSchedulePolis.nilai_ptg_B, cetakSchedulePolis.ket_rincian_kontr,
                            cetakSchedulePolis.nilai_ptg_C,cetakSchedulePolis.pst_A,
                            cetakSchedulePolis.pst_B,cetakSchedulePolis.pst_C,
                            cetakSchedulePolis.pst_D,cetakSchedulePolis.pst_E,
                            cetakSchedulePolis.stn_rate_A,cetakSchedulePolis.stn_rate_B,
                            cetakSchedulePolis.stn_rate_C,cetakSchedulePolis.stn_rate_D,
                            cetakSchedulePolis.stn_rate_E,cetakSchedulePolis.nilai_ptg_D,
                            cetakSchedulePolis.nilai_prm_A,cetakSchedulePolis.nilai_prm_B,
                            cetakSchedulePolis.nilai_prm_C,cetakSchedulePolis.nilai_prm_D,
                            cetakSchedulePolis.almt_polis,cetakSchedulePolis.obyek_ptg,
                            cetakSchedulePolis.ket_event,cetakSchedulePolis.lokasi,
                            cetakSchedulePolis.jml_peserta,tgl_mul_ptg,
                            tgl_akh_ptg,cetakSchedulePolis.spek_hole,
                            cetakSchedulePolis.ket_hadiah,cetakSchedulePolis.own_risk,
                            cetakSchedulePolis.kd_mtu,cetakSchedulePolis.kota_cab,
                            cetakSchedulePolis.nilai_ttl_prm,cetakSchedulePolis.no_rsk,
                            cetakSchedulePolis.nm_deb,cetakSchedulePolis.alm_lok_ptg,
                            cetakSchedulePolis.kd_usr,cetakSchedulePolis.tmp_lahir,
                            draft = @"<div class='draft-watermark'>DRAFT</div>"
                        }));
                    }

                    break;
            }

            stringBuilder.Append(Constant.FooterReport);
            return stringBuilder.ToString();
        }
    }
}