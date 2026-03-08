using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scriban;

namespace ABB.Application.CetakPLAReasuransis.Queries
{
    public class GetCetakPLAReasuransiQuery : IRequest<string>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public Int16 no_pla { get; set; }
    }

    public class GetCetakPLAReasuransiQueryHandler : IRequestHandler<GetCetakPLAReasuransiQuery, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<GetCetakPLAReasuransiQueryHandler> _logger;

        public GetCetakPLAReasuransiQueryHandler(IDbConnectionPst connectionPst, 
            IHostEnvironment environment, ILogger<GetCetakPLAReasuransiQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> Handle(GetCetakPLAReasuransiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var no_pla = request.no_pla == 0 ? 1 : request.no_pla;
                var datas = (await _connectionPst.QueryProc<CetakPLAReasuransiDto>("spr_cl03r_02", 
                    new
                    {
                        input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                    $"{request.kd_thn},{request.no_kl.Trim()},{request.no_mts},{no_pla}"
                    })).ToList();

                string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "PLAReasuransi.html" );
                
                string templateReportHtml = await File.ReadAllTextAsync( reportPath );
                
                if (datas.Count == 0)
                    throw new NullReferenceException("Data tidak ditemukan");

                Template templateProfileResult = Template.Parse( templateReportHtml );

                var data = datas.FirstOrDefault();
                
                
                var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg, true);
                var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
                var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl, true);
                var nilai_share = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl * (data.pst_share / 100));
                var pst_share = ReportHelper.ConvertToReportFormat(data.pst_share);
                var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl * (data.pst_share / 100), true);
                var nilai_jns_sor_01 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_01);
                var nilai_jns_sor_02 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_02);
                var nilai_jns_sor_03 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_03);
                var nilai_jns_sor_04 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_04);
                var nilai_jns_sor_05 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_05);
                var total_nilai_jns_sor = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_01
                                                                             + data.nilai_jns_sor_02
                                                                             + data.nilai_jns_sor_03
                                                                             + data.nilai_jns_sor_04
                                                                             + data.nilai_jns_sor_05);
                var nilai_sor_pas_01 = ReportHelper.ConvertToReportFormat(data.nilai_sor_pas_01);
                var nilai_sor_pas_02 = ReportHelper.ConvertToReportFormat(data.nilai_sor_pas_02);
                var nilai_sor_pas_03 = ReportHelper.ConvertToReportFormat(data.nilai_sor_pas_03);
                var nilai_sor_pas_04 = ReportHelper.ConvertToReportFormat(data.nilai_sor_pas_04);
                var nilai_sor_pas_05 = ReportHelper.ConvertToReportFormat(data.nilai_sor_pas_05);
                var pst_sor_pas_01 = ReportHelper.ConvertToReportFormat(data.pst_sor_pas_01);
                var pst_sor_pas_02 = ReportHelper.ConvertToReportFormat(data.pst_sor_pas_02);
                var pst_sor_pas_03 = ReportHelper.ConvertToReportFormat(data.pst_sor_pas_03);
                var pst_sor_pas_04 = ReportHelper.ConvertToReportFormat(data.pst_sor_pas_04);
                var pst_sor_pas_05 = ReportHelper.ConvertToReportFormat(data.pst_sor_pas_05);
                var resultTemplate = templateProfileResult.Render( new
                {
                        
                    data.no_berkas_reas, data.nm_ttg, data.no_pol_ttg, data.nm_scob, data.nm_jns_sor_05,
                    tgl_kej = ReportHelper.ConvertDateTime(data.tgl_kej, "dd MMMM yyyy"),
                    data.nm_oby, data.symbol, nilai_ttl_ptg, nilai_share_bgu, data.address, data.ket_oby,
                    tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd MMM yyyy"), 
                    tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd MMM yyyy"), 
                    data.tempat_kej, data.ket_dia, data.nm_jns_sor_01, nilai_share, data.kond_ptg,
                    data.sebab_kerugian, data.nm_jns_sor_02, nilai_ttl_kl, data.no_sert, data.sifat_kerugian,
                    data.kt_cb, tgl_closing_ind = ReportHelper.ConvertDateTime(data.tgl_closing_ind, "MMM dd yyyy"),
                    data.kd_cb, data.kd_cob, data.kd_scob, data.ket_pla,
                    data.kd_thn, data.no_kl, data.no_mts, request.no_pla, data.nm_pas, data.nm_jns_sor_03,
                    data.almt_pas, data.kt_pas, pst_share, nilai_kl, total_nilai_jns_sor, data.nm_jns_sor_04,
                    nilai_jns_sor_01, nilai_jns_sor_02, nilai_jns_sor_03, nilai_jns_sor_04, nilai_jns_sor_05,
                    nilai_sor_pas_01, nilai_sor_pas_02, nilai_sor_pas_03, nilai_sor_pas_04, nilai_sor_pas_05,
                    pst_sor_pas_01, pst_sor_pas_02, pst_sor_pas_03, pst_sor_pas_04, pst_sor_pas_05
                } );

                return resultTemplate;
            }, _logger);
        }
    }
}