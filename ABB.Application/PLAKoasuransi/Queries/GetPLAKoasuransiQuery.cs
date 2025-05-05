using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.PLAKoasuransi.Queries
{
    public class GetPLAKoasuransiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public Int16 no_dla { get; set; }
        public string bahasa { get; set; }
    }

    public class GetPLAKoasuransiQueryHandler : IRequestHandler<GetPLAKoasuransiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetPLAKoasuransiQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetPLAKoasuransiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var no_dla = request.no_dla == 0 ? 2 : request.no_dla;
            var datas = (await _connectionFactory.QueryProc<PLAKoasuransiDto>("spr_cl03r_01", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_kl.Trim()},{request.no_mts},{no_dla}"
                })).ToList();

            var report = string.Empty;

            switch (request.bahasa)
            {
                case "INA":
                    report = "PLAKoasuransiINA.html";
                    break;
                case "ENG":
                    report = "PLAKoasuransiENG.html";
                    break;
            }
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", report );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
            var nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu);
            var nilai_ttl_kl = ReportHelper.ConvertToReportFormat(data.nilai_ttl_kl);
            var pst_sor_pas = ReportHelper.ConvertToReportFormat(data.pst_sor_pas, true);
            var nilai_jns_sor_01 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_01);
            var nilai_jns_sor_02 = ReportHelper.ConvertToReportFormat(data.nilai_jns_sor_02);
            var pst_share = ReportHelper.ConvertToReportFormat(data.pst_share, true);
            var nilai_kl = ReportHelper.ConvertToReportFormat(data.nilai_kl);
            var resultTemplate = templateProfileResult.Render( new
            {
                    
                data.no_berkas_reas, data.nm_ttg, data.no_pol_ttg, data.nm_scob, 
                tgl_kej = ReportHelper.ConvertDateTime(data.tgl_kej, "dd/MM/yyyy"),
                data.nm_oby, data.symbol, nilai_ttl_ptg, nilai_share_bgu,
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"), 
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"), 
                data.tempat_kej, pst_sor_pas, data.ket_dia, data.nm_jns_sor_01,
                data.sebab_kerugian, data.nm_jns_sor_02, nilai_ttl_kl, data.no_sert, data.sifat_kerugian,
                data.kt_cb, tgl_closing_ind = ReportHelper.ConvertDateTime(data.tgl_closing_ind, "MMM dd yyyy"), nilai_jns_sor_01, nilai_jns_sor_02,
                data.almt_pas, data.kt_pas, data.nm_pas, pst_share, data.kd_cb,
                data.kd_cob, data.kd_scob, data.kd_thn, data.no_kl, data.no_mts,
                no_dla, nilai_kl, data.footer
            } );

            return resultTemplate;
        }
    }
}