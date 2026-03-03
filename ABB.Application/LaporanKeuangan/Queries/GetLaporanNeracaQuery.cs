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
        public bool IsHeaderKolom { get; set; }
        public string HeaderTahunIni { get; set; }
        public string HeaderTahunLalu { get; set; }
    }
    
    public class GetLaporanNeracaQuery : IRequest<LaporanKeuanganResponse>
    {
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
            // 1. TENTUKAN RANGE WAKTU
            int thnIni = request.Tahun;
            int thnLalu = request.Tahun - 1;

            System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterIni;
            System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu;

            if (request.JenisPeriode == "BULANAN")
            {
                filterIni = x => x.thn == thnIni && x.bln == request.Bulan;
                filterLalu = x => x.thn == thnLalu && x.bln == request.Bulan;
            }
            else 
            {
                filterIni = x => x.thn == thnIni && x.bln <= request.Bulan;
                filterLalu = x => x.thn == thnLalu && x.bln <= request.Bulan;
            }

            // 2. FUNGSI LOKAL SALDO
            async Task<List<AkunSaldo>> GetSaldoByFilter(System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filter)
            {
                return await _context.RekapJurnal
                    .Where(filter)
                    .GroupBy(x => x.gl_akun)
                    .Select(g => new AkunSaldo 
                    {
                        KodeAkun = g.Key,
                        // Cast double ke decimal
                       Total = g.Sum(x => x.gl_dk == "D" ? (decimal)x.gl_nilai_idr : -(decimal)x.gl_nilai_idr)
                    })
                    .ToListAsync(cancellationToken);
            }

            // AMBIL DATA (Sequential)
            var dataSaldoIni = await GetSaldoByFilter(filterIni);
            var dataSaldoLalu = await GetSaldoByFilter(filterLalu);
            
            var templates = await _context.TemplateLapKeu
                                    .Where(t => t.TipeLaporan == "NERACA")
                                    .OrderBy(t => t.Urutan).ThenBy(t => t.Id)
                                    .AsNoTracking().ToListAsync(cancellationToken);

            var mapTipeBaris = templates.ToDictionary(x => x.Urutan, x => x.TipeBaris);

            // 3. RAKIT HTML
            // 3. RAKIT HTML
            StringBuilder sb = new StringBuilder();

            int romanCounter = 0;       // Level 2 (I, II)
            int detailCounter = 0;      // Level 3 (1, 2, 3) & Total
            int subDetailCounter = 0;   // Level 4 (a, b, c)

            // --- LOGIKA BARU: KAMUS HASIL ---
            var hasilPerBaris = new Dictionary<int, (decimal Ini, decimal Lalu)>();

            // --- [PENTING]: DEKLARASI EXCEL DATA DI SINI ---
            var excelData = new List<LaporanExcelRow>(); 

            foreach (var item in templates)
            {
                int currentLevel = int.Parse(item.Level ?? "1");
                decimal nilaiIni = 0;
                decimal nilaiLalu = 0;

                // ==========================================
                // LOGIKA HITUNG ANGKA
                // ==========================================
                
                if (item.TipeBaris == "DETAIL")
                {
                    if (!string.IsNullOrEmpty(item.Rumus))
                    {
                        var akunList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var akun in akunList)
                        {
                            var clean = akun.Trim();
                            nilaiIni += dataSaldoIni.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                            nilaiLalu += dataSaldoLalu.Where(x => x.KodeAkun != null && x.KodeAkun.StartsWith(clean)).Sum(x => x.Total);
                        }
                    }
                }
                else if (item.TipeBaris == "TOTAL")
                {
                    if (!string.IsNullOrEmpty(item.Rumus))
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
                                        var hasilTarget = hasilPerBaris[i];
                                        nilaiIni += hasilTarget.Ini;
                                        nilaiLalu += hasilTarget.Lalu;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            var urutanList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var strUrutan in urutanList)
                            {
                                if (int.TryParse(strUrutan.Trim(), out int targetUrutan))
                                {
                                    if (hasilPerBaris.ContainsKey(targetUrutan))
                                    {
                                        var hasilTarget = hasilPerBaris[targetUrutan];
                                        nilaiIni += hasilTarget.Ini;
                                        nilaiLalu += hasilTarget.Lalu;
                                    }
                                }
                            }
                        }
                    }
                }

                if (item.Urutan > 0) 
                {
                    hasilPerBaris[item.Urutan] = (nilaiIni, nilaiLalu);
                }

                // ==========================================
                // LOGIKA FORMAT & PENOMORAN (VISUAL)
                // ==========================================

                string strIni = "";
                string strLalu = "";

                if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                {
                    strIni = nilaiIni.ToString("#,##0.00");
                    strLalu = nilaiLalu.ToString("#,##0.00");
                }

                if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                {
                    sb.Append("<tr><td colspan='3' style='height:15px'>&nbsp;</td></tr>");
                    excelData.Add(new LaporanExcelRow { TipeBaris = item.TipeBaris }); // Sekarang excelData dikenali
                }
                else
                {
                    string cssClass = $"lvl-{currentLevel}";
                    string cssTotal = "";
                    if (item.TipeBaris == "TOTAL")
                    {
                        cssTotal = (currentLevel == 1) ? "grand-total" : "total-line";
                        if(currentLevel == 1) cssClass += " grand-total"; 
                    }

                    string deskripsiFinal = item.Deskripsi; 
                    
                    if (item.TipeBaris == "HEADING")
                    {
                        if (currentLevel == 1)
                        {
                            romanCounter = 0; detailCounter = 0; subDetailCounter = 0;
                        }
                        else 
                        {
                            romanCounter++; subDetailCounter = 0;
                            deskripsiFinal = $"{ToRoman(romanCounter)}. {item.Deskripsi}";
                        }
                    }
                    else if (item.TipeBaris == "DETAIL" || (item.TipeBaris == "TOTAL" && currentLevel > 1))
                    {
                        if (currentLevel == 5) deskripsiFinal = $"- {item.Deskripsi}";
                        else if (currentLevel == 4) { subDetailCounter++; deskripsiFinal = $"{ToAlpha(subDetailCounter)}. {item.Deskripsi}"; }
                        else { detailCounter++; subDetailCounter = 0; deskripsiFinal = $"{detailCounter}. {item.Deskripsi}"; }
                    }

                    // --- INSERT DATA KE OBJECT EXCEL ---
                    var rowExcel = new LaporanExcelRow {
                        TipeBaris = item.TipeBaris,
                        Level = currentLevel,
                        Deskripsi = deskripsiFinal,
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
                    // -----------------------------------

                    sb.Append("<tr>");

                    if (item.TipeBaris == "HEADING")
                    {
                        if (currentLevel == 1)
                        {
                            string spaced = string.Join("  ", item.Deskripsi.Trim().ToCharArray());
                            sb.Append($"<td class='lvl-1'>{spaced}</td>");
                            sb.Append($"<td class='lvl-1' style='text-align:center;'>{request.Tahun}</td>");
                            sb.Append($"<td class='lvl-1' style='text-align:center;'>{request.Tahun - 1}</td>");
                        }
                        else
                        {
                            sb.Append($"<td colspan='3' class='{cssClass}'>{deskripsiFinal}</td>");
                        }
                    }
                    else
                    {
                        sb.Append($"<td class='{cssClass}'>{deskripsiFinal}</td>");
                        sb.Append($"<td class='text-right {cssTotal}'>{strIni}</td>");
                        sb.Append($"<td class='text-right {cssTotal}'>{strLalu}</td>");
                    }
                    sb.Append("</tr>");
                }
            }
            
            // ===============================================
            // LOGIKA GAMBAR LOGO (BASE64)
            // ===============================================
            string logoBase64 = "";
            try 
            {
                // 1. Arahkan ke wwwroot/img/Logo-Icon.png
                string webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot"); 
                string imagePath = Path.Combine(webRootPath, "img", "Logo-Icon.png"); 

                if (File.Exists(imagePath))
                {
                    byte[] imageArray = await File.ReadAllBytesAsync(imagePath, cancellationToken);
                    string base64Data = Convert.ToBase64String(imageArray);
                    
                    // PENTING: Karena filenya .png, header-nya pakai image/png
                    logoBase64 = $"data:image/png;base64,{base64Data}"; 
                }
            }
            catch
            {
                logoBase64 = ""; 
            }

            // --- DATE TIME ---
            DateTime tanggalLaporan = new DateTime(request.Tahun, request.Bulan, 1).AddMonths(1).AddDays(-1); 

            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanNeraca.html");
            if (!File.Exists(reportPath)) throw new FileNotFoundException($"Template not found: {reportPath}");

            string templateHtml = await File.ReadAllTextAsync(reportPath, cancellationToken);
            var template = Scriban.Template.Parse(templateHtml);
            
            var modelData = new
            {
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
            
            return new LaporanKeuanganResponse 
            {
                HtmlString = template.Render(context),
                ExcelData = excelData
            };
        }

        private string ToRoman(int number)
        {
            if (number < 1) return string.Empty;
            if (number >= 20) return number.ToString(); 
            string[] romans = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            return romans[number - 1];
        }

        private string ToAlpha(int number)
        {
            if (number < 1) return "";
            return ((char)('a' + number - 1)).ToString();
        }

        public class AkunSaldo 
        {
            public string KodeAkun { get; set; }
            public decimal Total { get; set; }
        }
    }
}