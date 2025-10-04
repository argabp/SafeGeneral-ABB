using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaKomisiTambahans.Queries
{
    public class GetCetakNotaKomisiTambahanQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string jns_sb_nt { get; set; }
        public string kd_cb { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string kd_thn { get; set; }
        public string kd_bln { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
        public string jns_lap { get; set; }
    }

    public class GetCetakNotaKomisiTambahanQueryHandler : IRequestHandler<GetCetakNotaKomisiTambahanQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public GetCetakNotaKomisiTambahanQueryHandler(IDbConnectionFactory connectionFactory, 
	        IHostEnvironment environment, ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetCetakNotaKomisiTambahanQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<CetakNotaKomisiTambahanModel>("spr_uw13r_02", 
                new
                {
                    input_str = $"{request.jns_sb_nt.Trim()},{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()},{request.jns_lap.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "CetakNotaKomisiTambahan.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            var reportConfig = _reportConfig.Configurations.First(w => w.Database == request.DatabaseName);
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();

            var no_polis = data.no_updt == 0
                ? $"{data.kd_cb}.{data.kd_cob}.{data.kd_scob}.{data.kd_thn}.{data.no_pol}-{data.st_pas}" : 
                $"{data.kd_cb}.{data.kd_cob}.{data.kd_scob}.{data.kd_thn}.{data.no_pol}.{data.no_updt}-{data.st_pas}";
            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);
            var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
            var nilai_pph = ReportHelper.ConvertToReportFormat(data.nilai_pph);
            var nilai_ppn = ReportHelper.ConvertToReportFormat(data.nilai_ppn);
            var nilai_lain = ReportHelper.ConvertToReportFormat(data.nilai_lain);
            var nilai_koperasi = ReportHelper.ConvertToReportFormat(data.nilai_koperasi);
            
            string text_komisi;

            if (data.uraian == "KOMISI" && data.kd_grp_brk == "2")
            {
                text_komisi = "BROKERAGE" + 
                              (data.ket_nt?.Length != 0 
                                  ? "" 
                                  : "  (" + data.pst_nota.Value.ToString("#,##0.00") + "%)");
            }
            else
            {
                text_komisi = data.uraian + "  " + 
                              (data.ket_nt?.Length != 0 
                                  ? "" 
                                  : "(" + data.pst_nota.Value.ToString("#,##0.00") + "%)");
            }

            string text_ppn =
                data.pst_ppn == 0 ? "" : $"PPN {ReportHelper.ConvertToReportFormat(data.pst_ppn, true)} %";
            string text_pph = data.pst_ppn != 0 ? data.nm_grp_sbr_bis : "";
            string text_koperasi = data.pst_lain != 0 ? "Jasa Koperasi" : "";

            string total = ReportHelper.ConvertToReportFormat(data.nilai_nt + data.nilai_ppn + data.nilai_pph + data.nilai_lain);

            var resultTemplate = templateProfileResult.Render( new
            {
                data.nm_ttj_nt, data.almt_ttj_nt, data.no_npwp, data.no_reg, no_polis, nilai_nt, 
                data.jns_tr, data.jns_nt_msk, data.jns_nt_kel, data.nm_cb, data.almt_ttj,
                nilai_prm, nilai_pph, nilai_ppn, nilai_lain, data.nm_ttj, data.kt_ttj,
                nilai_koperasi, data.kt_cb, data.kd_mtu_symbol, data.tgl_nt_ind, data.no_nota,
                data.period_polis, text_komisi, text_ppn, text_pph, text_koperasi, total,
                title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
            } );

            return resultTemplate;
        }
    }
}