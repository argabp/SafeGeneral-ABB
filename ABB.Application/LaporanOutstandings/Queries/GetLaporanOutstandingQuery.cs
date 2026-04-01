using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scriban;
using Scriban.Runtime;
using System.IO;
using System.Text;

namespace ABB.Application.LaporanOutstandings.Queries
{
    public class LaporanOutstandingResponse
    {
        public string HtmlString { get; set; }
        public List<SpLaporanOutstandingResult> RawData { get; set; }
    }

    public class GetLaporanOutstandingQuery : IRequest<LaporanOutstandingResponse>
    {
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string TglProduksiAwal { get; set; }
        public string TglProduksiAkhir { get; set; }
        public string TglPelunasan { get; set; }
        public string JenisTransaksi { get; set; }
        public string JenisLaporan { get; set; }
        public string UserLogin { get; set; }
        public List<string> SelectedCodes { get; set; }
    }

    public class GetLaporanOutstandingQueryHandler : IRequestHandler<GetLaporanOutstandingQuery, LaporanOutstandingResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanOutstandingQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<LaporanOutstandingResponse> Handle(GetLaporanOutstandingQuery request, CancellationToken cancellationToken)
        {
            // 1. Persiapkan Parameter
            DateTime tglProdAwal = DateTime.Parse(request.TglProduksiAwal);
            DateTime tglProdAkhir = DateTime.Parse(request.TglProduksiAkhir);
            DateTime tglPelunasan = DateTime.Parse(request.TglPelunasan);
            string strSelectedCodes = request.SelectedCodes != null ? string.Join(",", request.SelectedCodes) : "";

            // 2. Eksekusi Stored Procedure
            var dataLaporan = await _context.SpLaporanOutstandingResults
                .FromSqlRaw("EXEC sp_LaporanOutstanding {0}, {1}, {2}, {3}, {4}, {5}",
                    request.KodeCabang ?? "",
                    tglProdAwal,
                    tglProdAkhir,
                    tglPelunasan,
                    request.JenisTransaksi ?? "",
                    strSelectedCodes)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any()) throw new Exception("Data tidak ditemukan.");

            // 3. Ambil Nama Cabang
             string namaCabang = "SEMUA CABANG";
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var cabangEntity = await _context.Set<Cabang>().FirstOrDefaultAsync(c => c.kd_cb == request.KodeCabang, cancellationToken);
                if (cabangEntity != null) namaCabang = cabangEntity.nm_cb;
            }

            // =========================================================================================
            // 4. LOGIKA GROUPING & RENDERING (C# Tetap Pegang Kendali Visualnya)
            // =========================================================================================
            string displayLabel = request.JenisLaporan switch {
                "Pos" => "PEMBAWA POS",
                "Broker" => "AGEN/BROKER",
                "COB" => "COB",
                "Tertanggung" => "TERTANGGUNG",
                _ => "DETAIL (KESELURUHAN)"
            };

            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? $"{n.Value:N2}" : "0.00";

            string BuildRowHtml(int index, SpLaporanOutstandingResult item)
            {
                int umur = (item.date.HasValue && item.tgl_jth_tempo.HasValue) ? Math.Max(0, (item.tgl_jth_tempo.Value - item.date.Value).Days) : 0;
                decimal nNota = item.netto ?? 0;
                decimal nBayar = item.jumlah ?? 0; 
                decimal nOs = nNota - nBayar;

                return $@"
                    <tr>
                        <td class='center'>{index}</td>
                        <td>{item.no_nd}/<br>{item.no_pl}</td>
                        <td>{item.nm_cust2}/<br>{item.nm_pos}</td>
                        <td>{item.nm_brok}</td>
                        <td>{item.jn_ass}</td>
                        <td>{item.lok}/<br>{item.kd_tutup}</td>
                        <td>{fmtDate(item.date)}/<br>{fmtDate(item.tgl_jth_tempo)}</td>
                        <td>{item.curensi}/<br>{item.kurs}</td>
                        <td>{umur}</td>
                        <td style='text-align:right'>{fmtNum(nNota)}</td>
                        <td style='text-align:right'>{fmtNum(nBayar)}</td>
                        <td style='text-align:right'>{fmtNum(nOs)}</td>
                    </tr>";
            }

            string BuildSubtotalHtml(string label, decimal totNota, decimal totBayar, decimal totOs)
            {
                return $@"
                    <tr style='font-weight:bold; background-color:#f0f0f0;'>
                        <td colspan='9' style='text-align:right; padding-right:10px;'>TOTAL :</td>
                        <td style='text-align:right'>{fmtNum(totNota)}</td>
                        <td style='text-align:right'>{fmtNum(totBayar)}</td>
                        <td style='text-align:right'>{fmtNum(totOs)}</td>
                    </tr>";
            }

            StringBuilder detailsBuilder = new StringBuilder();
            int idx = 1;

            if (request.JenisLaporan == "Detail" || string.IsNullOrEmpty(request.JenisLaporan))
            {
                foreach (var item in dataLaporan) detailsBuilder.Append(BuildRowHtml(idx++, item));
                
                decimal gNota = dataLaporan.Sum(x => x.saldo ?? 0);
                decimal gBayar = dataLaporan.Sum(x => x.jumlah ?? 0);
                detailsBuilder.Append(BuildSubtotalHtml("KESELURUHAN", gNota, gBayar, gNota - gBayar));
            }
            else
            {
                IEnumerable<IGrouping<string, SpLaporanOutstandingResult>> groupedData = request.JenisLaporan switch {
                    "COB" => dataLaporan.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key),
                    "Tertanggung" => dataLaporan.GroupBy(x => x.nm_cust2 ?? "TANPA NAMA").OrderBy(g => g.Key),
                    "Pos" => dataLaporan.GroupBy(x => x.nm_pos ?? "TANPA POS").OrderBy(g => g.Key),
                    "Broker" => dataLaporan.GroupBy(x => x.nm_brok ?? "DIRECT").OrderBy(g => g.Key),
                    _ => dataLaporan.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key)
                };

                foreach (var group in groupedData)
                {
                    detailsBuilder.Append($@"<tr><td colspan='12' style='background-color:#e6f7ff; font-weight:bold; padding-left:10px;'>{displayLabel}: {group.Key}</td></tr>");
                    
                    decimal subNota = 0, subBayar = 0;
                    foreach (var item in group)
                    {
                        detailsBuilder.Append(BuildRowHtml(idx++, item));
                        subNota += (item.saldo ?? 0);
                        subBayar += (item.jumlah ?? 0);
                    }
                    detailsBuilder.Append(BuildSubtotalHtml(group.Key, subNota, subBayar, subNota - subBayar));
                }
            }

            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanOutstanding.html");
            string templateHtml = await File.ReadAllTextAsync(reportPath);
            var template = Scriban.Template.Parse(templateHtml);

            var modelData = new {
                details = detailsBuilder.ToString(),
                KodeCabang = request.KodeCabang,
                NamaCabang = namaCabang,
                TglProduksiAwal = request.TglProduksiAwal,
                TglProduksiAkhir = request.TglProduksiAkhir,
                TanggalPosisi = DateTime.Now.ToString("dd-MM-yyyy"),
                JudulLaporan = $"Laporan Outstanding - {request.JenisTransaksi} ({displayLabel})",
                TglPelunasan = request.TglPelunasan
            };

            var context = new TemplateContext();
            var scriptObj = new ScriptObject();
            scriptObj.Import(modelData, renamer: member => member.Name);
            context.PushGlobal(scriptObj);

            return new LaporanOutstandingResponse
            {
                HtmlString = template.Render(context),
                RawData = dataLaporan
            };
        }
    }
}