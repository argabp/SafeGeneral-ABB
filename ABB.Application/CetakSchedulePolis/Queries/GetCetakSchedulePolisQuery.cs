using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Interfaces;
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

        public GetCetakSchedulePolisQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
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

            string resultTemplate;

            if (MultipleReport.Contains(reportTemplateName))
                return GenerateMultipleReport(reportTemplateName, cetakSchedulePolisData, templateReportHtml);
            
            var cetakSchedulePolis = cetakSchedulePolisData.FirstOrDefault();
            var sub_total_kebakaran = cetakSchedulePolis.nilai_ttl -
                                      (cetakSchedulePolis.nilai_bia_mat + cetakSchedulePolis.nilai_bia_pol);
            if (ReportHaveDetails.Contains(reportTemplateName))
            {
                StringBuilder stringBuilder = new StringBuilder();
                var sequence = 0;
                foreach (var data in cetakSchedulePolisData)
                {
                    sequence++;
                    stringBuilder.Append(GenerateDetailReport(reportTemplateName, data, sequence));
                }
                resultTemplate = templateProfileResult.Render( new
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
                    cetakSchedulePolis.nilai_prm_tbh_01,cetakSchedulePolis.nm_cvrg_02, 
                    cetakSchedulePolis.kd_cvrg_02, cetakSchedulePolis.pst_rate_prm_02, 
                    cetakSchedulePolis.stn_rate_prm_02, cetakSchedulePolis.nilai_prm_tbh_02,
                    cetakSchedulePolis.ket_dis, cetakSchedulePolis.nilai_dis,
                    cetakSchedulePolis.nilai_bia_pol, cetakSchedulePolis.nilai_bia_mat,
                    cetakSchedulePolis.nilai_ttl, cetakSchedulePolis.ket_klausula,
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
                    cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
                    cetakSchedulePolis.desk_aksesoris, cetakSchedulePolis.nm_utk,
                    cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                    cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                    cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                    cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                    cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                    sub_total_kebakaran, detail = stringBuilder.ToString(),
                    cetakSchedulePolis.deduct, cetakSchedulePolis.nilai_prm_pkk,
                    cetakSchedulePolis.no_pol_lama,cetakSchedulePolis.footer,
                    cetakSchedulePolis.desk_deduct,cetakSchedulePolis.st_pas,
                    cetakSchedulePolis.nilai_prm_tbh_03,cetakSchedulePolis.nilai_prm_tbh_04,
                    cetakSchedulePolis.nilai_prm_tbh_05,cetakSchedulePolis.kd_cvrg_03,
                    cetakSchedulePolis.kd_cvrg_04,cetakSchedulePolis.kd_cvrg_05,
                    cetakSchedulePolis.nm_cvrg_03,cetakSchedulePolis.nm_cvrg_04,
                    cetakSchedulePolis.nm_cvrg_05,cetakSchedulePolis.pst_rate_prm_03,
                    cetakSchedulePolis.pst_rate_prm_04,cetakSchedulePolis.pst_rate_prm_05,
                    cetakSchedulePolis.stn_rate_prm_03,cetakSchedulePolis.stn_rate_prm_04,
                    cetakSchedulePolis.stn_rate_prm_05,cetakSchedulePolis.lamp_pol,
                } );
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
                    cetakSchedulePolis.pst_rate_prm_pkk, cetakSchedulePolis.stn_rate_prm_pkk,
                    cetakSchedulePolis.kd_mtu_symbol, cetakSchedulePolis.nilai_prk_pkk,
                    cetakSchedulePolis.nm_cvrg_01, cetakSchedulePolis.kd_cvrg_01,
                    cetakSchedulePolis.pst_rate_prm_01, cetakSchedulePolis.stn_rate_prm_01,
                    cetakSchedulePolis.nilai_prm_tbh_01,cetakSchedulePolis.nm_cvrg_02, 
                    cetakSchedulePolis.kd_cvrg_02, cetakSchedulePolis.pst_rate_prm_02, 
                    cetakSchedulePolis.stn_rate_prm_02, cetakSchedulePolis.nilai_prm_tbh_02,
                    cetakSchedulePolis.ket_dis, cetakSchedulePolis.nilai_dis,
                    cetakSchedulePolis.nilai_bia_pol, cetakSchedulePolis.nilai_bia_mat,
                    cetakSchedulePolis.nilai_ttl, cetakSchedulePolis.ket_klausula,
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
                    cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
                    cetakSchedulePolis.desk_aksesoris, cetakSchedulePolis.nm_utk,
                    cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                    cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                    cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                    cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                    cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                    sub_total_kebakaran, cetakSchedulePolis.cover, cetakSchedulePolis.tgl_closing,
                    cetakSchedulePolis.no_pol_lama,cetakSchedulePolis.st_pas,
                    cetakSchedulePolis.nilai_prm_pkk,cetakSchedulePolis.nilai_prm_tbh_03,
                    cetakSchedulePolis.nilai_prm_tbh_04, cetakSchedulePolis.nilai_prm_tbh_05,
                    cetakSchedulePolis.kd_cvrg_03, cetakSchedulePolis.kd_cvrg_04,
                    cetakSchedulePolis.kd_cvrg_05,cetakSchedulePolis.nm_cvrg_03,
                    cetakSchedulePolis.nm_cvrg_04, cetakSchedulePolis.nm_cvrg_05,
                    cetakSchedulePolis.pst_rate_prm_03, cetakSchedulePolis.pst_rate_prm_04,
                    cetakSchedulePolis.pst_rate_prm_05,cetakSchedulePolis.stn_rate_prm_03,
                    cetakSchedulePolis.stn_rate_prm_04,cetakSchedulePolis.stn_rate_prm_05,
                    cetakSchedulePolis.deduct,cetakSchedulePolis.lamp_pol,
                } );
            }
            
            return resultTemplate;
        }
    
        private string GenerateDetailReport(string reportType, CetakSchedulePolisDto data, int sequence)
        {
            switch (reportType)
            {
                case "LampiranPolisFireDaftarIsi.html":
                    return @$"<tr style='height: 50px;'>
                        <td style='font-weight: bold;' colspan='8'>{data.nm_grp_oby}</td>
                        </tr>
                        <tr>
                            <td style='width: 3%;  text-align: right; vertical-align: top;' rowspan='2'>{data.no_oby}</td>
                            <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.desk_oby}</td>
                            <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.almt_rsk}</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.ket_okup}</td>
                            <td style='width: 5%;  text-align: center; vertical-align: top;'>{data.kd_kls_konstr}</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>if( {data.kd_penerangan} ='1','Listrik', 'Lain-lain')</td>
                            <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.symbol}</td>
                            <td style='width: 10%; text-align: center; vertical-align: top;' rowspan='2'>{data.nilai_ttl_ptg}</td>
                        </tr>
                        <tr>
                            <td style='vertical-align: top; text-align: center;'>{data.ket_rsk}</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style='font-weight: bold; text-align: center;' colspan='2'>{data.nm_grp_oby} : </td>
                            <td style='font-weight: bold;'>sum( {data.nilai_ttl_ptg} for 1  ) </td>
                        </tr>";
                case "LampiranPolisPASiramaObyek.html":
                    
                    //TODO logic here
                    
                    return @$"
                                <tr>
                                    <td style='vertical-align: top; text-align: center;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.nm_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_lahir}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_mul_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_akh_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.jk_wkt}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.usia}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                </tr>";
                case "LampiranPolisPABiasaDaftarisi.html":
                    
                    //TODO logic here
                    
                    return @$"
                                <tr>
                                    <td style='vertical-align: top; text-align: center;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.nm_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.almt_ttg}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_lahir}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.usia}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.jup}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_mul_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{data.tgl_akh_ptg_ind}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                    <td style='vertical-align: top; text-align: center;'>{0}</td>
                                    <td style='vertical-align: top; text-align: center;'>kd_usr</td>
                                </tr>";
                case "LampiranPolisCargoDaftarisi.html":
                    
                    //TODO logic here
                    
                    return @$"<tr>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{sequence}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.jns_brg}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.penerima_brg}<br>{data.tempat_brkg} / {data.tempate_tiba}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nm_kapal}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.tgl_brkg}<br>{data.desk_kond}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm}<br>{data.pst_rate_prm} {data.stn_rate_prm}</td>
                                    <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.pst_deduct}</td>
                                </tr>";
                case "LampiranPolisMotorListing.html":
                    
                    //TODO logic here
                    
                    return @$"<tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{sequence}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nm_jns_kend}/{data.nm_merk_kend}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{0}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.no_rangka}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_casco}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{0}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_casco}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_rsk_sendiri}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_pap}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.no_msn}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nm_utk}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_tjh}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_pad}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.tipe_kend}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.jml_tempat_ddk}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.perlengkapan}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>Banjir</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.pst_rate_banjir} {data.stn_rate_banjir}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_banjir}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_tjp}</td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.kd_jns_ptg}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nm_pemilik}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.no_pls}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>Gempa Bumi</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.pst_rate_aog} {data.stn_rate_aog}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_aog}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.tgl_mul_ptg} s/d {data.tgl_akh_ptg}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>Huru-Hara</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.pst_rate_hh} {data.stn_rate_hh}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_hh}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>TRS</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.pst_rate_trs} {data.stn_rate_trs}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_trs}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>Kec. Diri Pap.</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_pap}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_pap}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>0,500 %</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>Kec. Diri Png.</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_pad}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_pad}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>1,000 %</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>TJH</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'>{data.nilai_prm_tjh}</td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-right: 1px solid;border-left: 1px solid;'></td>
                            </tr>
                            <tr>

                            <tr>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-left: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'>{0}</td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'>{0}</td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-bottom: 1px solid;'></td>
                                <td style='vertical-align: top; text-align: center;border-top: 1px solid;border-right: 1px solid;border-bottom: 1px solid;'></td>
                            </tr>";
                case "LampiranPolisMotorDetil.html":
                    
                    //TODO logic here
                    
                    return @$"<table>
                              <tr>
                                <td style='width: 5%;'>{sequence}</td>
                                <td style='width: 20%;'>MERK / JENIS / TAHUN / WARNA</td>
                                <td style='width: 1%;'>:</td>
                                <td style='width: 60%;' colspan='6'></td>
                                <td>Rate : </td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>NOMOR POLISI</td>
                                <td>:</td>
                                <td colspan='8'>{data.no_pls}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PENGGUNAAN KENDARAAN</td>
                                <td>:</td>
                                <td colspan='8'>{data.nm_utk}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>NO. RANGKA / MESIN</td>
                                <td>:</td>
                                <td colspan='8'>{data.no_rangka} / {data.no_msn}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>JUMLAH TEMPAT DUDUK</td>
                                <td>:</td>
                                <td colspan='8'>{data.jml_tempat_ddk} Orang</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>RESIKO SENDIRI</td>
                                <td>:</td>
                                <td colspan='8'>{data.symbol} {data.nilai_rsk_sendiri}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>MASA PERTANGGUNGAN</td>
                                <td>:</td>
                                <td colspan='8'>{data.jk_wkt_ptg} ha</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>HARGA PERTANGGUNGAN</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_casco_1}</td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 15%;'>{data.nilai_casco_2}</td>
                                <td style='width: 15%;'>x {data.pst_rate} %</td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_casco}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>TJH PIHAK KETIGA</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_tjh}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_tjh}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Huru-hara</td>
                                <td>:</td>
                                <td style='width: 18%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 15%;'>{data.nilai_casco}</td>
                                <td style='width: 15%;'>x {data.pst_rate} %</td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_hh_1}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Bencana Alam</td>
                                <td>:</td>
                                <td style='width: 18%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 15%;'>{data.nilai_casco_3}</td>
                                <td style='width: 15%;'>x {data.pst_rate} %</td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_aog}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi Banjir</td>
                                <td>:</td>
                                <td style='width: 18%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 15%;'>{data.nilai_casco_4}</td>
                                <td style='width: 15%;'>x {data.pst_rate} %</td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_banjir}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>Premi TRS</td>
                                <td>:</td>
                                <td style='width: 18%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td style='width: 15%;'>{data.nilai_casco+5}</td>
                                <td style='width: 15%;'>x {data.pst_rate} %</td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_trs}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PA Penumpang</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_pap}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_pap}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>PA Pengemudi</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_pad}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_pad}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>TJH Penumpang Pihak Ketiga</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_tjp}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_tkp}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>ME Penumpang</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_pap_med}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_pap_med}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td>ME Pengemudi</td>
                                <td>:</td>
                                <td style='width: 18%;'>{data.symbol} {data.nilai_pad_med}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'>=</td>
                                <td colspan='3'>{data.symbol} {data.nilai_prm_pad_med}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style='width: 18%;'>------------------------------------------</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'></td>
                                <td colspan='3'>---------------------------------------------------</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style='width: 18%;'>sum {data.symbol} {data.nilai_casco} + {data.nilai_tjh}</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'></td>
                                <td colspan='3'>sum {data.symbol} {data.nilai_prm_casco} + {data.nilai_prm_tjh}</td>
                              </tr>
                              <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style='width: 18%;'>------------------------------------------</td>
                                <td style='width: 1%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 15%;'></td>
                                <td style='width: 1%;'></td>
                                <td colspan='3'>---------------------------------------------------</td>
                              </tr>
                            </table>
                            <hr class='s1'>
                            <!-- Total -->
                            <h3>TOTAL PREMIUM :</h3>
                            <table>
                              <tr><td>HP CASCO</td><td>: xxxxxxxxxxxxxxxxxxx</td></tr>
                              <tr><td>HP TJH</td><td>: xxxxxxxxxxxxxxxxxxxxxx</td></tr>
                              <tr><td><strong>PREMI</strong></td><td>: <strong>xxxxxxxxxxxxxxxxxxx</strong></td></tr>
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
                        var sub_total_kebakaran = cetakSchedulePolis.nilai_ttl -
                                                  (cetakSchedulePolis.nilai_bia_mat + cetakSchedulePolis.nilai_bia_pol);
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
                            cetakSchedulePolis.nilai_prm_tbh_01, cetakSchedulePolis.nm_cvrg_02,
                            cetakSchedulePolis.kd_cvrg_02, cetakSchedulePolis.pst_rate_prm_02,
                            cetakSchedulePolis.stn_rate_prm_02, cetakSchedulePolis.nilai_prm_tbh_02,
                            cetakSchedulePolis.ket_dis, cetakSchedulePolis.nilai_dis,
                            cetakSchedulePolis.nilai_bia_pol, cetakSchedulePolis.nilai_bia_mat,
                            cetakSchedulePolis.nilai_ttl, cetakSchedulePolis.ket_klausula,
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
                            cetakSchedulePolis.desk_aksesoris, cetakSchedulePolis.nm_utk,
                            cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                            cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                            cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                            cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                            cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                            cetakSchedulePolis.thn_buat, cetakSchedulePolis.nm_jns_kend,
                            cetakSchedulePolis.footer, cetakSchedulePolis.desk_deduct,
                            cetakSchedulePolis.nilai_prm_casco, cetakSchedulePolis.tempat_tiba,
                            cetakSchedulePolis.tempat_brkt,cetakSchedulePolis.nilai_prm_pkk,
                            cetakSchedulePolis.nilai_prm_tbh_03,cetakSchedulePolis.nilai_prm_tbh_04,
                            cetakSchedulePolis.nilai_prm_tbh_05,cetakSchedulePolis.kd_cvrg_03,
                            cetakSchedulePolis.kd_cvrg_04,cetakSchedulePolis.kd_cvrg_05,
                            cetakSchedulePolis.nm_cvrg_03,cetakSchedulePolis.nm_cvrg_04,
                            cetakSchedulePolis.nm_cvrg_05,cetakSchedulePolis.pst_rate_prm_03,
                            cetakSchedulePolis.pst_rate_prm_04,cetakSchedulePolis.pst_rate_prm_05,
                            cetakSchedulePolis.stn_rate_prm_03,cetakSchedulePolis.stn_rate_prm_04,
                            cetakSchedulePolis.stn_rate_prm_05,cetakSchedulePolis.deduct,
                            cetakSchedulePolis.lamp_pol,
                        }));
                    }

                    break;
            }

            stringBuilder.Append(Constant.FooterReport);
            return stringBuilder.ToString();
        }
    }
}