using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Scriban;
using Scriban.Runtime;
using Microsoft.Extensions.Hosting;

namespace ABB.Application.LaporanKeuangan.Queries
{
    public class GetLaporanNeracaQuery : IRequest<string>
    {
        public DateTime PerTanggal { get; set; }
    }

    public class GetLaporanNeracaQueryHandler : IRequestHandler<GetLaporanNeracaQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanNeracaQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanNeracaQuery request, CancellationToken cancellationToken)
        {
            // 1. AMBIL DATA
            var templates = await _context.TemplateLapKeu
                .Where(t => t.TipeLaporan == "NERACA") 
                .OrderBy(t => t.Urutan) // <-- Sortir berdasarkan nomor urut yang kamu set di DB
                .ThenBy(t => t.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var querySaldo = from h in _context.JurnalMemorial117
                             join d in _context.JurnalMemorial117Detail on h.NoVoucher equals d.NoVoucher
                             where h.Tanggal <= request.PerTanggal
                             group d by d.KodeAkun into g
                             select new 
                             {
                                 KodeAkun = g.Key,
                                 SaldoAkhir = g.Sum(x => x.NilaiDebet - x.NilaiKredit)
                             };

            var dataSaldo = await querySaldo.AsNoTracking().ToListAsync(cancellationToken);

            // 2. RAKIT HTML
            StringBuilder sb = new StringBuilder();

            int romanCounter = 0; 
            int detailCounter = 0;

            foreach (var item in templates)
            {
                // Hitung Saldo
                decimal jumlah = 0;
                if (!string.IsNullOrEmpty(item.Rumus))
                {
                    var akunList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var akun in akunList)
                    {
                        var clean = akun.Trim();
                        jumlah += dataSaldo.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.SaldoAkhir ?? 0);
                    }
                }

                // --- LOGIKA PENOMORAN ---
                string deskripsiFinal = item.Deskripsi; 
                int currentLevel = int.Parse(item.Level ?? "1");

                if (item.TipeBaris == "HEADING")
                {
                    // LEVEL 1 (ASET): Reset Semua, Tanpa Nomor
                    if (currentLevel == 1)
                    {
                        romanCounter = 0;
                        detailCounter = 0;
                    }
                    // LEVEL 2 (INVESTASI): Romawi (I, II, III)
                    else 
                    {
                        romanCounter++;
                        
                        deskripsiFinal = $"{ToRoman(romanCounter)}. {item.Deskripsi}";
                    }
                }
                else if (item.TipeBaris == "DETAIL")
                {
                    // Angka Biasa (1, 2, 3...)
                    detailCounter++;
                    deskripsiFinal = $"{detailCounter}. {item.Deskripsi}";
                }
                else if (item.TipeBaris == "TOTAL")
                {
                    // JIKA USER MINTA TOTAL LANJUT NOMOR DARI DETAIL
                    // Hanya untuk Sub-Total (Level > 1), Grand Total (Level 1) biasanya polos
                    if (currentLevel > 1) 
                    {
                        detailCounter++; // Lanjutkan nomor urut
                        deskripsiFinal = $"{detailCounter}. {item.Deskripsi}";
                    }
                    // Jika Level 1 (TOTAL ASET), biarkan tanpa nomor
                }


                // Format Angka
                string strJumlah = "";
                if (jumlah != 0 && (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL"))
                {
                    strJumlah = jumlah.ToString("#,##0.00");
                }

                // RENDER HTML
                if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                {
                    sb.Append("<tr><td colspan='2' style='height:15px'>&nbsp;</td></tr>");
                }
                else
                {
                    string cssClass = $"lvl-{currentLevel}";

                    // Styling Class Tambahan
                    if (item.TipeBaris == "TOTAL")
                    {
                        if (currentLevel == 1) cssClass += " grand-total"; 
                        else cssClass += " total-line";
                    }

                    sb.Append("<tr>");

                    if (item.TipeBaris == "HEADING")
                    {
                        // HEADING
                        if (currentLevel == 1)
                        {
                            // ASET -> Jadi Center & Bold (Colspan 2)
                            sb.Append($"<td colspan='2' class='lvl-1'>{item.Deskripsi}</td>");
                        }
                        else
                        {
                            // INVESTASI -> Colspan 2
                            sb.Append($"<td colspan='2' class='{cssClass}'>{deskripsiFinal}</td>");
                        }
                    }
                    else if (item.TipeBaris == "TOTAL")
                    {
                        // TOTAL
                        sb.Append($"<td class='{cssClass}'>{deskripsiFinal}</td>");
                        
                        string styleJml = "class='text-right'";
                        if (currentLevel == 1) styleJml += " grand-total"; else styleJml += " total-line";
                        
                        sb.Append($"<td {styleJml}>{strJumlah}</td>");
                    }
                    else
                    {
                        // DETAIL
                        sb.Append($"<td class='{cssClass}'>{deskripsiFinal}</td>");
                        sb.Append($"<td class='text-right'>{strJumlah}</td>");
                    }
                    sb.Append("</tr>");
                }
            }

            // ... (Bagian Scriban Render sama seperti sebelumnya) ...
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanNeraca.html");
            if (!File.Exists(reportPath)) throw new FileNotFoundException($"Template not found: {reportPath}");

            string templateHtml = await File.ReadAllTextAsync(reportPath, cancellationToken);
            var template = Scriban.Template.Parse(templateHtml);
            
            var modelData = new
            {
                Rows = sb.ToString(),
                TanggalLaporan = request.PerTanggal.ToString("dd MMMM yyyy"),
                WaktuCetak = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            var context = new TemplateContext();
            var scriptObj = new ScriptObject();
            scriptObj.Import(modelData, renamer: member => member.Name);
            context.PushGlobal(scriptObj);
            
            return template.Render(context);
        }

        private string ToRoman(int number)
        {
            if (number < 1) return string.Empty;
            if (number >= 20) return number.ToString(); 
            string[] romans = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            return romans[number - 1];
        }
    }
}