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
            "LampiranPolisFireDaftarIsi.html"
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
                    input_str = "JK50,P,0552,24,00001,0,PT. BPR. DHAHA EKONOMI"
                    // input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                    //             $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt},{request.nm_ttg?.Trim()}"
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
                foreach (var data in cetakSchedulePolisData)
                {
                    stringBuilder.Append(GenerateDetailReport(reportTemplateName, data));
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
                    sub_total_kebakaran, detail = stringBuilder.ToString()
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
                    sub_total_kebakaran
                } );
            }

            

            return resultTemplate;
        }
    
        private string GenerateDetailReport(string reportType, CetakSchedulePolisDto data)
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
                            cetakSchedulePolis.jml_tempat_ddk, cetakSchedulePolis.nm_jns_kend,
                            cetakSchedulePolis.desk_aksesoris, cetakSchedulePolis.nm_utk,
                            cetakSchedulePolis.no_oby, cetakSchedulePolis.desk_oby,
                            cetakSchedulePolis.ket_okup, cetakSchedulePolis.kd_kls_konstr,
                            cetakSchedulePolis.kd_penerangan, cetakSchedulePolis.symbol,
                            cetakSchedulePolis.ket_rsk, cetakSchedulePolis.nm_mtu,
                            cetakSchedulePolis.nm_grp_oby, cetakSchedulePolis.nm_grp_oby_1,
                            sub_total_kebakaran
                        }));
                    }

                    break;
            }

            stringBuilder.Append(Constant.FooterReport);
            return stringBuilder.ToString();
        }
    }
}