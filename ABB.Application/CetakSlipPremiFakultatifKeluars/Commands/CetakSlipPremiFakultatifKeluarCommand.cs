using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakSlipPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakSlipPremiFakultatifKeluars.Commands
{
    public class CetakSlipPremiFakultatifKeluarCommand : IRequest<string>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }

        public string kd_grp_sor { get; set; }

        public string kd_rk_sor { get; set; }
    }

    public class CetakSlipPremiFakultatifKeluarCommandHandler : IRequestHandler<CetakSlipPremiFakultatifKeluarCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakSlipPremiFakultatifKeluarCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakSlipPremiFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<CetakSlipPremiFakultatifKeluarModel>("spr_ri02r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt}," +
                                $"{request.no_rsk},{request.kd_endt.Trim()},{request.no_updt_reas}," +
                                $"{request.kd_grp_sor},{request.kd_rk_sor.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "SlipPremiFakultatifKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            var nilai_nt = ReportHelper.ConvertToReportFormat(Math.Abs(data?.nilai_nt ?? 0));
            var pst_adj_reas = ReportHelper.ConvertToReportFormat(data?.pst_adj_reas, true);
            var nilai_ttl_ptg_reas = ReportHelper.ConvertToReportFormat(data?.nilai_ttl_ptg_reas);
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data?.nilai_ttl_ptg);
            var pst_kms_reas = ReportHelper.ConvertToReportFormat(data?.pst_kms_reas);
            var nilai_adj_reas = ReportHelper.ConvertToReportFormat(data?.nilai_adj_reas);
            var nilai_kms_reas = ReportHelper.ConvertToReportFormat(data?.nilai_kms_reas);
            var nilai_prm_reas = ReportHelper.ConvertToReportFormat(data?.nilai_prm_reas);
            var stn_adj_reas = ReportHelper.ConvertSatuanType(data?.stn_adj_reas);
            var gross_premium_total = ReportHelper.ConvertToDecimalFormat(nilai_prm_reas) +
                                      ReportHelper.ConvertToDecimalFormat(nilai_adj_reas);
            
            var resultTemplate = templateProfileResult.Render( new
            {
                data?.nm_cob, data?.nm_rk, data?.no_slip, data?.symbol, data?.ket_tc,
                nilai_nt, pst_adj_reas, nilai_ttl_ptg_reas, nilai_ttl_ptg, pst_kms_reas,  
                data?.ket_net, data?.almt_rsk, nilai_kms_reas, stn_adj_reas, gross_premium_total,
                data?.nm_ttg, data?.no_pol_ttg, data?.nm_scob, data?.tgl_nt, data?.nm_bag, data?.nm_kpl_bag,
                tgl_mul_reas = ReportHelper.ConvertDateTime(data?.tgl_mul_reas, "dd MMM yyyy"),
                tgl_akh_reas = ReportHelper.ConvertDateTime(data?.tgl_akh_reas, "dd MMM yyyy"),
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data?.tgl_mul_ptg, "dd MMM yyyy"),
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data?.tgl_akh_ptg, "dd MMM yyyy")
            } );

            return resultTemplate;
        }
    }
}