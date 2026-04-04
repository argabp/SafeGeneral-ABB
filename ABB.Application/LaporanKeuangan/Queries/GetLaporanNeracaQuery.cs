using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic; 
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; 
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Scriban;
using Scriban.Runtime;
using Microsoft.Extensions.Hosting;

namespace ABB.Application.LaporanKeuangan.Queries
{
    // =========================================================================
    // 1. KELAS PENAMPUNG DATA & MODEL UTAMA
    // =========================================================================
    public class LaporanKeuanganResponse
    {
        public string HtmlString { get; set; }
        public List<LaporanExcelRow> ExcelData { get; set; }
    }

    public class LaporanExcelRow
    {
        public string TipeBaris { get; set; }
        public int Level { get; set; }
        public string Deskripsi { get; set; }
        public decimal? NilaiIni { get; set; }
        public decimal? NilaiLalu { get; set; }
        public decimal? NilaiMutasi { get; set; } 
        public bool IsHeaderKolom { get; set; }
        public string HeaderTahunIni { get; set; }
        public string HeaderTahunLalu { get; set; }
    }

    public class AkunSaldo 
    {
        public string KodeAkun { get; set; }
        public decimal Total { get; set; }
    }

    // =========================================================================
    // 2. QUERY TUNGGAL (UNTUK NERACA & NERACA BULAN)
    // =========================================================================
    public class GetLaporanNeracaQuery : IRequest<LaporanKeuanganResponse>
    {
        public string TipeLaporan { get; set; } // Tambahan indikator
        public string JenisPeriode { get; set; } 
        public int Bulan { get; set; } 
        public int Tahun { get; set; } 
    }

    public class GetLaporanNeracaQueryHandler : IRequestHandler<GetLaporanNeracaQuery, LaporanKeuanganResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanNeracaQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<LaporanKeuanganResponse> Handle(GetLaporanNeracaQuery request, CancellationToken cancellationToken)
        {
            // Cek apakah user minta versi Bulanan (3 Kolom) atau Tahunan (2 Kolom)
            bool isBulanan = request.TipeLaporan == "NERACA (BULAN)";
            
            // [PERBAIKAN DARI ATASAN]: Selalu tembak ke Master "NERACA" biasa!
            string templateName = "NERACA"; 

            var templates = await _context.TemplateLapKeu
                                    .Where(t => t.TipeLaporan == templateName)
                                    .OrderBy(t => t.Urutan).ThenBy(t => t.Id)
                                    .AsNoTracking().ToListAsync(cancellationToken);

            var mapTipeBaris = templates.ToDictionary(x => x.Urutan, x => x.TipeBaris);
            
            StringBuilder sb = new StringBuilder();
            var excelData = new List<LaporanExcelRow>(); 
            int romanCounter = 0, detailCounter = 0, subDetailCounter = 0;

            // =================================================================
            // LOGIKA CABANG: JIKA BULANAN (3 KOLOM)
            // =================================================================
            if (isBulanan)
            {
                int targetBulanDB = request.Bulan - 1; 

                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu = 
                    x => x.thn == request.Tahun && x.bln < targetBulanDB;
                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterMutasi = 
                    x => x.thn == request.Tahun && x.bln == targetBulanDB;

                async Task<List<AkunSaldo>> GetSaldoByFilterBulan(System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filter)
                {
                    return await _context.RekapJurnal.Where(filter).GroupBy(x => x.gl_akun)
                        .Select(g => new AkunSaldo { KodeAkun = g.Key, Total = g.Sum(x => x.gl_dk == "D" ? (decimal)x.gl_nilai_idr : -(decimal)x.gl_nilai_idr) })
                        .ToListAsync(cancellationToken);
                }

                var dataSaldoLalu = await GetSaldoByFilterBulan(filterLalu);
                var dataSaldoMutasi = await GetSaldoByFilterBulan(filterMutasi);
                var hasilPerBaris = new Dictionary<int, (decimal Lalu, decimal Mutasi, decimal Ini)>();

                foreach (var item in templates)
                {
                    int currentLevel = int.Parse(item.Level ?? "1");
                    decimal nilaiLalu = 0, nilaiMutasi = 0, nilaiIni = 0;

                    if (item.TipeBaris == "DETAIL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        var akunList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var akun in akunList)
                        {
                            var clean = akun.Trim();
                            nilaiLalu += dataSaldoLalu.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                            nilaiMutasi += dataSaldoMutasi.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                        }
                    }
                    else if (item.TipeBaris == "TOTAL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        if (item.Rumus.Contains("-"))
                        {
                            var parts = item.Rumus.Split('-');
                            if (parts.Length == 2 && int.TryParse(parts[0], out int startUrutan) && int.TryParse(parts[1], out int endUrutan))
                            {
                                for (int i = startUrutan; i <= endUrutan; i++)
                                {
                                    if (hasilPerBaris.ContainsKey(i) && mapTipeBaris.ContainsKey(i) && mapTipeBaris[i] == "DETAIL") 
                                    {
                                        nilaiLalu += hasilPerBaris[i].Lalu; nilaiMutasi += hasilPerBaris[i].Mutasi;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            var urutanList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var strUrutan in urutanList)
                            {
                                if (int.TryParse(strUrutan.Trim(), out int targetUrutan) && hasilPerBaris.ContainsKey(targetUrutan))
                                {
                                    nilaiLalu += hasilPerBaris[targetUrutan].Lalu; nilaiMutasi += hasilPerBaris[targetUrutan].Mutasi;
                                }
                            }
                        }
                    }

                    nilaiIni = nilaiLalu + nilaiMutasi;
                    if (item.Urutan > 0) hasilPerBaris[item.Urutan] = (nilaiLalu, nilaiMutasi, nilaiIni);

                    string strLalu = "", strMutasi = "", strIni = "";
                    if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    {
                        strLalu = nilaiLalu.ToString("#,##0.00"); strMutasi = nilaiMutasi.ToString("#,##0.00"); strIni = nilaiIni.ToString("#,##0.00");
                    }

                    if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                    {
                        sb.Append("<tr><td colspan='4' style='height:15px'>&nbsp;</td></tr>");
                        excelData.Add(new LaporanExcelRow { TipeBaris = item.TipeBaris }); 
                    }
                    else
                    {
                        string cssClass = $"lvl-{currentLevel}";
                        string cssTotal = item.TipeBaris == "TOTAL" ? (currentLevel == 1 ? "grand-total" : "total-line") : "";
                        if(currentLevel == 1 && item.TipeBaris == "TOTAL") cssClass += " grand-total"; 

                        string deskripsiFinal = item.Deskripsi; 
                        if (item.TipeBaris == "HEADING")
                        {
                            if (currentLevel == 1) { romanCounter = 0; detailCounter = 0; subDetailCounter = 0; }
                            else { romanCounter++; subDetailCounter = 0; deskripsiFinal = $"{ToRoman(romanCounter)}. {item.Deskripsi}"; }
                        }
                        else if (item.TipeBaris == "DETAIL" || (item.TipeBaris == "TOTAL" && currentLevel > 1))
                        {
                            if (currentLevel == 5) deskripsiFinal = $"- {item.Deskripsi}";
                            else if (currentLevel == 4) { subDetailCounter++; deskripsiFinal = $"{ToAlpha(subDetailCounter)}. {item.Deskripsi}"; }
                            else { detailCounter++; subDetailCounter = 0; deskripsiFinal = $"{detailCounter}. {item.Deskripsi}"; }
                        }

                        var rowExcel = new LaporanExcelRow {
                            TipeBaris = item.TipeBaris, Level = currentLevel, Deskripsi = deskripsiFinal,
                            NilaiLalu = (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL") ? nilaiLalu : (decimal?)null,
                            NilaiMutasi = (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL") ? nilaiMutasi : (decimal?)null,
                            NilaiIni = (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL") ? nilaiIni : (decimal?)null
                        };

                        if (item.TipeBaris == "HEADING" && currentLevel == 1) {
                            rowExcel.Deskripsi = string.Join("  ", item.Deskripsi.Trim().ToCharArray());
                            rowExcel.IsHeaderKolom = true;
                        }
                        excelData.Add(rowExcel);

                        sb.Append("<tr>");
                        if (item.TipeBaris == "HEADING")
                        {
                            if (currentLevel == 1)
                            {
                                sb.Append($"<td class='lvl-1'>{string.Join("  ", item.Deskripsi.Trim().ToCharArray())}</td>");
                                sb.Append($"<td class='lvl-1' style='text-align:center;'>s/d Bulan Lalu</td><td class='lvl-1' style='text-align:center;'>Mutasi</td><td class='lvl-1' style='text-align:center;'>s/d Bulan Ini</td>");
                            }
                            else sb.Append($"<td colspan='4' class='{cssClass}'>{deskripsiFinal}</td>");
                        }
                        else
                        {
                            sb.Append($"<td class='{cssClass}'>{deskripsiFinal}</td><td class='text-right {cssTotal}'>{strLalu}</td><td class='text-right {cssTotal}'>{strMutasi}</td><td class='text-right {cssTotal}'>{strIni}</td>");
                        }
                        sb.Append("</tr>");
                    }
                }
            }
            // =================================================================
            // LOGIKA CABANG: JIKA TAHUNAN (2 KOLOM - LAMA)
            // =================================================================
            else
            {
                int targetBulanDB = request.Bulan - 1;

                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterIni = 
                    x => x.thn == request.Tahun && x.bln <= targetBulanDB;
                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu = 
                    x => x.thn == request.Tahun - 1 && x.bln <= targetBulanDB;

                async Task<List<AkunSaldo>> GetSaldoByFilterTahun(System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filter)
                {
                    return await _context.RekapJurnal.Where(filter).GroupBy(x => x.gl_akun)
                        .Select(g => new AkunSaldo { KodeAkun = g.Key, Total = g.Sum(x => x.gl_dk == "D" ? (decimal)x.gl_nilai_idr : -(decimal)x.gl_nilai_idr) })
                        .ToListAsync(cancellationToken);
                }

                var dataSaldoIni = await GetSaldoByFilterTahun(filterIni);
                var dataSaldoLalu = await GetSaldoByFilterTahun(filterLalu);
                var hasilPerBaris = new Dictionary<int, (decimal Ini, decimal Lalu)>();

                foreach (var item in templates)
                {
                    int currentLevel = int.Parse(item.Level ?? "1");
                    decimal nilaiIni = 0, nilaiLalu = 0;

                    if (item.TipeBaris == "DETAIL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        var akunList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var akun in akunList)
                        {
                            var clean = akun.Trim();
                            nilaiIni += dataSaldoIni.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                            nilaiLalu += dataSaldoLalu.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                        }
                    }
                    else if (item.TipeBaris == "TOTAL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        if (item.Rumus.Contains("-"))
                        {
                            var parts = item.Rumus.Split('-');
                            if (parts.Length == 2 && int.TryParse(parts[0], out int startUrutan) && int.TryParse(parts[1], out int endUrutan))
                            {
                                for (int i = startUrutan; i <= endUrutan; i++)
                                {
                                    if (hasilPerBaris.ContainsKey(i) && mapTipeBaris.ContainsKey(i) && mapTipeBaris[i] == "DETAIL") 
                                    {
                                        nilaiIni += hasilPerBaris[i].Ini; nilaiLalu += hasilPerBaris[i].Lalu;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            var urutanList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var strUrutan in urutanList)
                            {
                                if (int.TryParse(strUrutan.Trim(), out int targetUrutan) && hasilPerBaris.ContainsKey(targetUrutan))
                                {
                                    nilaiIni += hasilPerBaris[targetUrutan].Ini; nilaiLalu += hasilPerBaris[targetUrutan].Lalu;
                                }
                            }
                        }
                    }

                    if (item.Urutan > 0) hasilPerBaris[item.Urutan] = (nilaiIni, nilaiLalu);

                    string strIni = "", strLalu = "";
                    if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    {
                        strIni = nilaiIni.ToString("#,##0.00"); strLalu = nilaiLalu.ToString("#,##0.00");
                    }

                    if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                    {
                        sb.Append("<tr><td colspan='3' style='height:15px'>&nbsp;</td></tr>");
                        excelData.Add(new LaporanExcelRow { TipeBaris = item.TipeBaris }); 
                    }
                    else
                    {
                        string cssClass = $"lvl-{currentLevel}";
                        string cssTotal = item.TipeBaris == "TOTAL" ? (currentLevel == 1 ? "grand-total" : "total-line") : "";
                        if(currentLevel == 1 && item.TipeBaris == "TOTAL") cssClass += " grand-total"; 

                        string deskripsiFinal = item.Deskripsi; 
                        if (item.TipeBaris == "HEADING")
                        {
                            if (currentLevel == 1) { romanCounter = 0; detailCounter = 0; subDetailCounter = 0; }
                            else { romanCounter++; subDetailCounter = 0; deskripsiFinal = $"{ToRoman(romanCounter)}. {item.Deskripsi}"; }
                        }
                        else if (item.TipeBaris == "DETAIL" || (item.TipeBaris == "TOTAL" && currentLevel > 1))
                        {
                            if (currentLevel == 5) deskripsiFinal = $"- {item.Deskripsi}";
                            else if (currentLevel == 4) { subDetailCounter++; deskripsiFinal = $"{ToAlpha(subDetailCounter)}. {item.Deskripsi}"; }
                            else { detailCounter++; subDetailCounter = 0; deskripsiFinal = $"{detailCounter}. {item.Deskripsi}"; }
                        }

                        var rowExcel = new LaporanExcelRow {
                            TipeBaris = item.TipeBaris, Level = currentLevel, Deskripsi = deskripsiFinal,
                            NilaiIni = (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL") ? nilaiIni : (decimal?)null,
                            NilaiLalu = (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL") ? nilaiLalu : (decimal?)null
                        };

                        if (item.TipeBaris == "HEADING" && currentLevel == 1) {
                            rowExcel.Deskripsi = string.Join("  ", item.Deskripsi.Trim().ToCharArray());
                            rowExcel.IsHeaderKolom = true;
                            rowExcel.HeaderTahunIni = request.Tahun.ToString();
                            rowExcel.HeaderTahunLalu = (request.Tahun - 1).ToString();
                        }
                        excelData.Add(rowExcel);

                        sb.Append("<tr>");
                        if (item.TipeBaris == "HEADING")
                        {
                            if (currentLevel == 1)
                            {
                                sb.Append($"<td class='lvl-1'>{string.Join("  ", item.Deskripsi.Trim().ToCharArray())}</td>");
                                sb.Append($"<td class='lvl-1' style='text-align:center;'>{request.Tahun}</td><td class='lvl-1' style='text-align:center;'>{request.Tahun - 1}</td>");
                            }
                            else sb.Append($"<td colspan='3' class='{cssClass}'>{deskripsiFinal}</td>");
                        }
                        else
                        {
                            sb.Append($"<td class='{cssClass}'>{deskripsiFinal}</td><td class='text-right {cssTotal}'>{strIni}</td><td class='text-right {cssTotal}'>{strLalu}</td>");
                        }
                        sb.Append("</tr>");
                    }
                }
            }

            // ===============================================
            // EKSEKUSI SCRIBAN & RENDER HTML AKHIR
            // ===============================================
            string logoBase64 = "";
            try 
            {
                string imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "img", "Logo-Icon.png"); 
                if (File.Exists(imagePath)) { logoBase64 = $"data:image/png;base64,{Convert.ToBase64String(await File.ReadAllBytesAsync(imagePath, cancellationToken))}"; }
            }
            catch { logoBase64 = ""; }

            DateTime tanggalLaporan = new DateTime(request.Tahun, request.Bulan, 1).AddMonths(1).AddDays(-1); 
            
            // Ambil template HTML sesuai versinya
            string templateHtmlName = isBulanan ? "LaporanNeracaBulan.html" : "LaporanNeraca.html";
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", templateHtmlName);
            if (!File.Exists(reportPath)) throw new FileNotFoundException($"Template not found: {reportPath}");

            var template = Scriban.Template.Parse(await File.ReadAllTextAsync(reportPath, cancellationToken));
            var modelData = new {
                Rows = sb.ToString(),
                HeaderTahunIni = request.Tahun.ToString(),
                HeaderTahunLalu = (request.Tahun - 1).ToString(),
                TanggalLaporan = tanggalLaporan.ToString("dd MMMM yyyy"), 
                WaktuCetak = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                LogoImage = logoBase64
            };

            var context = new TemplateContext();
            var scriptObj = new ScriptObject();
            scriptObj.Import(modelData, renamer: member => member.Name);
            context.PushGlobal(scriptObj);
            
            return new LaporanKeuanganResponse { HtmlString = template.Render(context), ExcelData = excelData };
        }

        private string ToRoman(int number) {
            if (number < 1) return string.Empty; if (number >= 20) return number.ToString(); 
            string[] romans = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            return romans[number - 1];
        }

        private string ToAlpha(int number) {
            if (number < 1) return ""; return ((char)('a' + number - 1)).ToString();
        }
    }
}