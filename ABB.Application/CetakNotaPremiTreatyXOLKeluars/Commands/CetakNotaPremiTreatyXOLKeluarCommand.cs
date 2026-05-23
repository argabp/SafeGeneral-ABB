using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiTreatyXOLKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaPremiTreatyXOLKeluars.Commands
{
    public class CetakNotaPremiTreatyXOLKeluarCommand : IRequest<string>
    {
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
    }

    public class CetakNotaPremiTreatyXOLKeluarCommandHandler : IRequestHandler<CetakNotaPremiTreatyXOLKeluarCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public CetakNotaPremiTreatyXOLKeluarCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(CetakNotaPremiTreatyXOLKeluarCommand request, CancellationToken cancellationToken)
        {
                var datas = (await _connectionPst.QueryProc<CetakNotaPremiTreatyXOLKeluarModel>("spr_fn04r_01", 
                new
                {
                    input_str = $"{request.jns_sb_nt.Trim()},{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "NotaPremiTreatyXOLKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            var nilai_nt = data.nilai_nt == 0 ? string.Empty : ReportHelper.ConvertToReportFormat(data.nilai_nt);
            string nilai_01 = string.Empty;
            string nilai_02 = string.Empty;
            string nilai_03 = string.Empty;
            string nilai_04 = string.Empty;
            string nilai_05 = string.Empty;

            string nilai_01_1 = string.Empty;
            string nilai_02_1 = string.Empty;
            string nilai_03_1 = string.Empty;
            string nilai_04_1 = string.Empty;
            string nilai_05_1 = string.Empty;

            string header;
            string first_nilai_nota = string.Empty;
            string second_nilai_nota = string.Empty;

            if (data.st_nota == "D")
            {
                header = "NOTA DEBET";
                first_nilai_nota = nilai_nt;

                // CREDIT SIDE (no leading space)
                nilai_01 = ReportHelper.FormatIf(!ReportHelper.StartsWithSpace(data.uraian_01), data.nilai_nt);
                nilai_02 = ReportHelper.FormatIf(!ReportHelper.StartsWithSpace(data.uraian_02), data.nilai_02);
                nilai_03 = ReportHelper.FormatIf(!ReportHelper.StartsWithSpace(data.uraian_03), data.nilai_03);
                nilai_04 = ReportHelper.FormatIf(!ReportHelper.StartsWithSpace(data.uraian_04), data.nilai_04);
                nilai_05 = ReportHelper.FormatIf(!ReportHelper.StartsWithSpace(data.uraian_05), data.nilai_05);
            }
            else
            {
                header = "NOTA KREDIT";
                second_nilai_nota = nilai_nt;

                // DEBIT SIDE (leading space)
                nilai_01_1 = ReportHelper.FormatIf(ReportHelper.StartsWithSpace(data.uraian_01), data.nilai_nt);
                nilai_02_1 = ReportHelper.FormatIf(ReportHelper.StartsWithSpace(data.uraian_02), data.nilai_02);
                nilai_03_1 = ReportHelper.FormatIf(ReportHelper.StartsWithSpace(data.uraian_03), data.nilai_03);
                nilai_04_1 = ReportHelper.FormatIf(ReportHelper.StartsWithSpace(data.uraian_04), data.nilai_04);
                nilai_05_1 = ReportHelper.FormatIf(ReportHelper.StartsWithSpace(data.uraian_05), data.nilai_05);
            }
            
            decimal total =
                (data.st_nota == "D" ? 0 : Math.Abs(data.nilai_nt))
                + (!ReportHelper.StartsWithSpace(data.uraian_01) ? Math.Abs(data.nilai_01) : 0)
                + (!ReportHelper.StartsWithSpace(data.uraian_02) ? Math.Abs(data.nilai_02) : 0)
                + (!ReportHelper.StartsWithSpace(data.uraian_03) ? Math.Abs(data.nilai_03) : 0)
                + (!ReportHelper.StartsWithSpace(data.uraian_04) ? Math.Abs(data.nilai_04) : 0)
                + (!ReportHelper.StartsWithSpace(data.uraian_05) ? Math.Abs(data.nilai_05) : 0);

            var nilai_total = total != 0
                ? ReportHelper.ConvertToReportFormat(total)
                : string.Empty;

            var grand_nilai_total = nilai_total;
            
            var view_jumlah_untuk = @$"
                <tr class='no-border'>
                    <td style='width: 20%;'>JUMLAH UNTUK ANDA</td>
                    <td style='width: 20%; text-align: right;'>{first_nilai_nota}</td>
                    <td style='width: 20%; text-align: right;'>{second_nilai_nota}</td>
                </tr>";
            
            var resultTemplate = templateProfileResult.Render( new
            {
                nilai_01_1 = second_nilai_nota, nilai_02_1, nilai_03_1, nilai_04_1, nilai_05_1,
                nilai_nt, nilai_01 = first_nilai_nota, nilai_02, nilai_03, nilai_04, nilai_05, nilai_total, 
                data.jns_tr, data.jns_nt_msk, data.jns_nt_kel, data.nm_cb, data.almt_ttj,
                data.almt_ttg, data.nm_ttg, data.nm_ttj, data.kt_ttj,
                data.no_berkas, data.no_pol_ttg, data.kt_ttg, data.kd_mtu_symbol, data.kd_tl,
                data.tgl_nt_ind, data.no_nota, data.nm_scob, data.nm_scob_ing,
                data.kd_cob, data.kd_scob, nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu), data.ket_nt,
                data.uraian_01, data.uraian_02, data.uraian_03, data.uraian_04, data.uraian_05,
                data.kd_mtu_pol_symbol, header, view_jumlah_untuk, grand_nilai_total,
                kd_thn_pol = data.tgl_nt_ind.Split(" ")[2], data.kt_cb
            } );

            return resultTemplate;
        }
    }
}