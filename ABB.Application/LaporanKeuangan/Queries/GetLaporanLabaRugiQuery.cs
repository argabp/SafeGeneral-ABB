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
    // CATATAN: Pastikan class LaporanKeuanganResponse, LaporanExcelRow, dan AkunSaldo 
    // tidak bentrok (duplicate) dengan yang ada di file GetLaporanNeracaQuery.cs.
    // Jika error duplicate, hapus 3 class penampung ini dari salah satu file (biarkan di Neraca saja).

    public class GetLaporanLabaRugiQuery : IRequest<LaporanKeuanganResponse>
    {
        public string TipeLaporan { get; set; } 
        public string JenisPeriode { get; set; } 
        public int Bulan { get; set; } 
        public int Tahun { get; set; } 
    }

    public class GetLaporanLabaRugiQueryHandler : IRequestHandler<GetLaporanLabaRugiQuery, LaporanKeuanganResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanLabaRugiQueryHandler(IDbContextPstNota context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<LaporanKeuanganResponse> Handle(GetLaporanLabaRugiQuery request, CancellationToken cancellationToken)
        {
            bool isBulanan = request.TipeLaporan == "LABA RUGI (BULAN)";
            string templateName = "LABARUGI"; 

            var templates = await _context.TemplateLapKeu
                                    .Where(t => t.TipeLaporan == templateName)
                                    .OrderBy(t => t.Urutan).ThenBy(t => t.Id)
                                    .AsNoTracking().ToListAsync(cancellationToken);

            var mapTipeBaris = templates.ToDictionary(x => x.Urutan, x => x.TipeBaris);
            
            // =================================================================
            // KAMUS (DICTIONARY) DARI MASTER COA & TYPE COA
            // =================================================================
            var typeCoaList = await _context.TypeCoa.AsNoTracking().ToListAsync(cancellationToken);
            var mapTipeToDK = new Dictionary<string, string>();
            foreach (var tc in typeCoaList)
            {
                if (!string.IsNullOrEmpty(tc.Type))
                {
                    string normalBalance = tc.Dk != null ? tc.Dk.ToString().Trim().ToUpper() : "D"; 
                    mapTipeToDK[tc.Type.Trim().ToUpper()] = normalBalance;
                }
            }

            var coaList = await _context.Set<Coa>().AsNoTracking().ToListAsync(cancellationToken);
            var mapAkunToTipe = new Dictionary<string, string>();
            foreach(var c in coaList) 
            {
                if(!string.IsNullOrEmpty(c.gl_kode)) 
                {
                    string tipeAkun = c.gl_type != null ? c.gl_type.Trim().ToUpper() : "";
                    mapAkunToTipe[c.gl_kode.Trim()] = tipeAkun;
                }
            }

            StringBuilder sb = new StringBuilder();
            var excelData = new List<LaporanExcelRow>(); 
            int romanCounter = 0, detailCounter = 0, subDetailCounter = 0;

            // =================================================================
            // LOGIKA CABANG: JIKA BULANAN (3 KOLOM)
            // =================================================================
            if (isBulanan)
            {
               int targetBulanDB = request.Bulan - 1; 

                // Filter untuk kolom "s/d Bulan Lalu" (Contoh: Jan s/d Feb)
                // Menambahkan x.bln >= 1 agar bulan 0 tidak ikut
                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu = 
                    x => x.thn == request.Tahun && x.bln >= 1 && x.bln < targetBulanDB;

                // Filter untuk kolom "Mutasi" (Hanya bulan terpilih)
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
                        var tipeList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var tipeInput in tipeList)
                        {
                            var cleanTipe = tipeInput.Trim().ToUpper();
                            string posisiDK = mapTipeToDK.ContainsKey(cleanTipe) ? mapTipeToDK[cleanTipe] : "D";

                            decimal sumLalu = dataSaldoLalu.Where(x => 
                                x.KodeAkun != null && 
                                mapAkunToTipe.ContainsKey(x.KodeAkun.Trim()) && 
                                mapAkunToTipe[x.KodeAkun.Trim()] == cleanTipe
                            ).Sum(x => x.Total);
                            
                            decimal sumMutasi = dataSaldoMutasi.Where(x => 
                                x.KodeAkun != null && 
                                mapAkunToTipe.ContainsKey(x.KodeAkun.Trim()) && 
                                mapAkunToTipe[x.KodeAkun.Trim()] == cleanTipe
                            ).Sum(x => x.Total);

                            if (posisiDK == "K")
                            {
                                sumLalu = sumLalu * -1;
                                sumMutasi = sumMutasi * -1;
                            }

                            nilaiLalu += sumLalu;
                            nilaiMutasi += sumMutasi;
                        }
                    }
                    else if (item.TipeBaris == "TOTAL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        string rumus = item.Rumus.Trim();

                        if (rumus.Contains("-") && !rumus.Contains("+") && !rumus.Contains("*") && !rumus.Contains("/") && !rumus.Contains(" ") && rumus.Split('-').Length == 2)
                        {
                            var parts = rumus.Split('-');
                            if (int.TryParse(parts[0], out int startUrutan) && int.TryParse(parts[1], out int endUrutan))
                            {
                                for (int i = startUrutan; i <= endUrutan; i++)
                                {
                                    if (hasilPerBaris.ContainsKey(i)) 
                                    {
                                        nilaiLalu += hasilPerBaris[i].Lalu; 
                                        nilaiMutasi += hasilPerBaris[i].Mutasi;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            rumus = rumus.Replace(",", "+"); 
                            try
                            {
                                string calcLalu = rumus;
                                string calcMutasi = rumus;

                                var numsInRumus = System.Text.RegularExpressions.Regex.Matches(rumus, @"\d+").Cast<System.Text.RegularExpressions.Match>().Select(m => m.Value).Distinct().OrderByDescending(x => int.Parse(x)).ToList();

                                foreach (var numStr in numsInRumus)
                                {
                                    int noUrutTarget = int.Parse(numStr);
                                    
                                    decimal valLalu = hasilPerBaris.ContainsKey(noUrutTarget) ? hasilPerBaris[noUrutTarget].Lalu : 0;
                                    decimal valMutasi = hasilPerBaris.ContainsKey(noUrutTarget) ? hasilPerBaris[noUrutTarget].Mutasi : 0;

                                    // PERBAIKAN: Gunakan format 4 desimal agar terhindar dari Int32 overflow dan bungkus kurung
                                    string calcValLaluStr = $"({valLalu.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)})";
                                    string calcValMutasiStr = $"({valMutasi.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)})";

                                    calcLalu = System.Text.RegularExpressions.Regex.Replace(calcLalu, @"\b" + numStr + @"\b", calcValLaluStr);
                                    calcMutasi = System.Text.RegularExpressions.Regex.Replace(calcMutasi, @"\b" + numStr + @"\b", calcValMutasiStr);
                                }

                                var dt = new System.Data.DataTable();
                                nilaiLalu += Convert.ToDecimal(dt.Compute(calcLalu, ""));
                                nilaiMutasi += Convert.ToDecimal(dt.Compute(calcMutasi, ""));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Kalkulasi Bulanan: " + ex.Message);
                                nilaiLalu = 0; nilaiMutasi = 0;
                            }
                        }
                    }

                    nilaiIni = nilaiLalu + nilaiMutasi;
                    if (item.Urutan > 0) hasilPerBaris[item.Urutan] = (nilaiLalu, nilaiMutasi, nilaiIni);

                    string strLalu = "", strMutasi = "", strIni = "";
                    // if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    // {
                    //     // strLalu = nilaiLalu.ToString("#,##0.00;(#,##0.00);0.00"); 
                    //     strLalu = nilaiLalu == 0 ? "" : nilaiLalu.ToString("#,##0.00;(#,##0.00)");
                    //     strMutasi = nilaiMutasi.ToString("#,##0.00;(#,##0.00);0.00"); 
                    //     strIni = nilaiIni.ToString("#,##0.00;(#,##0.00);0.00");
                    // }

                    if (item.TipeBaris == "DETAIL" && string.IsNullOrEmpty(item.Rumus))
                    {
                        // Jika DETAIL tapi tidak ada rumus, kita paksa jadi kosong
                        strLalu = "";
                        strMutasi = "";
                        strIni = "";
                    }
                    else if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    {
                        // Jika ada rumus atau tipe TOTAL, baru gunakan format angka
                        strLalu = nilaiLalu.ToString("#,##0.00;(#,##0.00);0.00"); 
                        strMutasi = nilaiMutasi.ToString("#,##0.00;(#,##0.00);0.00"); 
                        strIni = nilaiIni.ToString("#,##0.00;(#,##0.00);0.00");
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
            // LOGIKA CABANG: JIKA TAHUNAN (2 KOLOM)
            // =================================================================
            else
            {
                int targetBulanDB = request.Bulan;

                string[] arrayBulan = { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
                string namaBulan = arrayBulan[request.Bulan - 1]; 
                
                string judulKolomIni = $"s/d {namaBulan} {request.Tahun}";
                string judulKolomLalu = $"s/d {namaBulan} {request.Tahun - 1}";

                // System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterIni = 
                //     x => x.thn == request.Tahun && x.bln <= targetBulanDB;
                // System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu = 
                //     x => x.thn == request.Tahun - 1 && x.bln <= targetBulanDB;

                // Filter Tahun Ini (Januari s/d Bulan Terpilih)
                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterIni = 
                    x => x.thn == request.Tahun && x.bln >= 1 && x.bln <= targetBulanDB;

                // Filter Tahun Lalu (Januari s/d Bulan Terpilih di tahun sebelumnya)
                System.Linq.Expressions.Expression<Func<RekapJurnal, bool>> filterLalu = 
                    x => x.thn == request.Tahun - 1 && x.bln >= 1 && x.bln <= targetBulanDB;

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
                        var tipeList = item.Rumus.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var tipeInput in tipeList)
                        {
                            var cleanTipe = tipeInput.Trim().ToUpper();
                            string posisiDK = mapTipeToDK.ContainsKey(cleanTipe) ? mapTipeToDK[cleanTipe] : "D";

                            decimal sumIni = dataSaldoIni.Where(x => 
                                x.KodeAkun != null && 
                                mapAkunToTipe.ContainsKey(x.KodeAkun.Trim()) && 
                                mapAkunToTipe[x.KodeAkun.Trim()] == cleanTipe
                            ).Sum(x => x.Total);

                            decimal sumLalu = dataSaldoLalu.Where(x => 
                                x.KodeAkun != null && 
                                mapAkunToTipe.ContainsKey(x.KodeAkun.Trim()) && 
                                mapAkunToTipe[x.KodeAkun.Trim()] == cleanTipe
                            ).Sum(x => x.Total);

                            if (posisiDK == "K")
                            {
                                sumIni = sumIni * -1;
                                sumLalu = sumLalu * -1;
                            }

                            nilaiIni += sumIni;
                            nilaiLalu += sumLalu;
                        }
                    }
                   else if (item.TipeBaris == "TOTAL" && !string.IsNullOrEmpty(item.Rumus))
                    {
                        string rumus = item.Rumus.Trim();

                        if (rumus.Contains("-") && !rumus.Contains("+") && !rumus.Contains("*") && !rumus.Contains("/") && !rumus.Contains(" ") && rumus.Split('-').Length == 2)
                        {
                            var parts = rumus.Split('-');
                            if (int.TryParse(parts[0], out int startUrutan) && int.TryParse(parts[1], out int endUrutan))
                            {
                                for (int i = startUrutan; i <= endUrutan; i++)
                                {
                                    if (hasilPerBaris.ContainsKey(i)) 
                                    {
                                        nilaiIni += hasilPerBaris[i].Ini; 
                                        nilaiLalu += hasilPerBaris[i].Lalu;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            rumus = rumus.Replace(",", "+"); 
                            try
                            {
                                string calcIni = rumus;
                                string calcLalu = rumus;

                                var numsInRumus = System.Text.RegularExpressions.Regex.Matches(rumus, @"\d+").Cast<System.Text.RegularExpressions.Match>().Select(m => m.Value).Distinct().OrderByDescending(x => int.Parse(x)).ToList();

                                foreach (var numStr in numsInRumus)
                                {
                                    int noUrutTarget = int.Parse(numStr);
                                    
                                    decimal valIni = hasilPerBaris.ContainsKey(noUrutTarget) ? hasilPerBaris[noUrutTarget].Ini : 0;
                                    decimal valLalu = hasilPerBaris.ContainsKey(noUrutTarget) ? hasilPerBaris[noUrutTarget].Lalu : 0;

                                    // PERBAIKAN: Gunakan format 4 desimal agar terhindar dari Int32 overflow dan bungkus kurung
                                    string calcValIniStr = $"({valIni.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)})";
                                    string calcValLaluStr = $"({valLalu.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)})";

                                    calcIni = System.Text.RegularExpressions.Regex.Replace(calcIni, @"\b" + numStr + @"\b", calcValIniStr);
                                    calcLalu = System.Text.RegularExpressions.Regex.Replace(calcLalu, @"\b" + numStr + @"\b", calcValLaluStr);
                                }

                                var dt = new System.Data.DataTable();
                                nilaiIni += Convert.ToDecimal(dt.Compute(calcIni, ""));
                                nilaiLalu += Convert.ToDecimal(dt.Compute(calcLalu, ""));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Kalkulasi Tahunan: " + ex.Message);
                                nilaiIni = 0; nilaiLalu = 0;
                            }
                        }
                    }

                    if (item.Urutan > 0) hasilPerBaris[item.Urutan] = (nilaiIni, nilaiLalu);

                    string strIni = "", strLalu = "";
                    // if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    // {
                    //     strIni = nilaiIni.ToString("#,##0.00;(#,##0.00);0.00"); 
                    //     // strLalu = nilaiLalu.ToString("#,##0.00;(#,##0.00);0.00");
                    //     strLalu = nilaiLalu == 0 ? "" : nilaiLalu.ToString("#,##0.00;(#,##0.00)");
                    // }

                    if (item.TipeBaris == "DETAIL" && string.IsNullOrEmpty(item.Rumus))
                    {
                        strIni = "";
                        strLalu = "";
                    }
                    else if (item.TipeBaris == "DETAIL" || item.TipeBaris == "TOTAL")
                    {
                        strIni = nilaiIni.ToString("#,##0.00;(#,##0.00);0.00"); 
                        strLalu = nilaiLalu.ToString("#,##0.00;(#,##0.00);0.00");
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
                            rowExcel.HeaderTahunIni = judulKolomIni;
                            rowExcel.HeaderTahunLalu = judulKolomLalu;
                        }
                        excelData.Add(rowExcel);

                        sb.Append("<tr>");
                        if (item.TipeBaris == "HEADING")
                        {
                            if (currentLevel == 1)
                            {
                                sb.Append($"<td class='lvl-1'>{string.Join("  ", item.Deskripsi.Trim().ToCharArray())}</td>");
                                sb.Append($"<td class='lvl-1' style='text-align:center;'>{judulKolomIni}</td><td class='lvl-1' style='text-align:center;'>{judulKolomLalu}</td>");
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
            
            string templateHtmlName = isBulanan ? "LaporanLabaRugiBulan.html" : "LaporanLabaRugi.html";
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