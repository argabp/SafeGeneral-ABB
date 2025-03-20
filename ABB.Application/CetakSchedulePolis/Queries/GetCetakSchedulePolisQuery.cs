using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
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

            var reportTempalteName = Constant.ReportMapping[templateName];
            
            if (!Constant.ReportStoreProcedureMapping.Keys.Contains(reportTempalteName))
                throw new Exception("Store Procedure Not Found");
            
            var storeProcedureName = Constant.ReportStoreProcedureMapping[reportTempalteName];
            
            var cetakSchedulePolis = (await _connectionFactory.QueryProc<CetakSchedulePolisDto>(storeProcedureName, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt},{request.nm_ttg?.Trim()}"
                })).FirstOrDefault();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTempalteName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakSchedulePolis == null)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var sub_total_kebakaran = cetakSchedulePolis.nilai_ttl -
                                      (cetakSchedulePolis.nilai_bia_mat + cetakSchedulePolis.nilai_bia_pol);
            
            string resultTemplate = templateProfileResult.Render( new
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
                sub_total_kebakaran
            } );

            return resultTemplate;
        }
    }
}