using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanBukuBesars117.Queries; // Namespace Baru
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Domain.Entities; 
using ClosedXML.Excel;
using System.IO;


namespace ABB.Web.Modules.LaporanBukuBesar117
{
    public class LaporanBukuBesar117Controller : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IDbContextPstNota _context; // Inject Context buat ambil coa117 buat dropdown

        public LaporanBukuBesar117Controller(IReportGeneratorService reportGeneratorService, IDbContextPstNota context)
        {
            _reportGeneratorService = reportGeneratorService;
            _context = context;
        }

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            
            // --- 2. PAKAI PEMBERSIH COOKIE ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 3. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            ViewBag.IsPusat = isPusat;

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var userCabang = cabangList.FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;

            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;

            return View();
        }

        // --- 4. FUNGSI GET KODE CABANG BARU ---
        [HttpGet]
        public async Task<IActionResult> GetKodeCabang()
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = GetCleanCabangCookie();

            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); 

            var result = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });

            var filtered = result
                .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); 

            return Json(filtered);
        }

        // Dropdown Kode Akun (KHUSUS COA 104)
        // --- 5. UBAH FUNGSI INI AGAR NERIMA PARAMETER CABANG DROPDOWN ---
        [HttpGet]
        public async Task<IActionResult> GetCoaList(string text, string kodeCabangDropdown)
        {
            var kodeCabangCookie = GetCleanCabangCookie();
            
            // Pakai cabang dari dropdown jika ada
            var targetCabang = !string.IsNullOrEmpty(kodeCabangDropdown) ? kodeCabangDropdown : kodeCabangCookie;

            string glDept = null;
            if (!string.IsNullOrEmpty(targetCabang) && targetCabang.Length >= 2)
            {
                glDept = targetCabang.Substring(targetCabang.Length - 2);
            }

            var query = _context.Set<Coa117>()
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(targetCabang))
            {
                query = query.Where(x => x.gl_dept == glDept);
            }

            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(x => 
                    x.gl_kode.Contains(text) || 
                    x.gl_nama.Contains(text));
            }

            var list = await query
                .OrderBy(x => x.gl_kode)
                .Take(50) 
                .Select(x => new 
                {
                    Kode = x.gl_kode.Trim(),
                    Nama = x.gl_nama.Trim(),
                    Display = $"{x.gl_kode.Trim()} - {x.gl_nama.Trim()}"
                })
                .ToListAsync();

            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanBukuBesar117FilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanBukuBesar117Query
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    AkunAwal = model.AkunAwal,
                    AkunAkhir = model.AkunAkhir,
                    UserLogin = user
                };

                // [PERBAIKAN 1]: Gunakan nama variabel 'result' agar sinkron dengan pemanggilan di bawah
                var result = await Mediator.Send(query);

                // [PERBAIKAN 2]: Cek validitas object result
                if (result == null || result.RawData == null || !result.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanBukuBesar117.pdf",
                    result.HtmlString, // [PERBAIKAN 3]: Sekarang variabel 'result' sudah dikenali
                    user,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                    PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanBukuBesar117FilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                // 1. Ambil data dari MediatR (Query Handler)
                var response = await Mediator.Send(new GetLaporanBukuBesar117Query
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    AkunAwal = model.AkunAwal,
                    AkunAkhir = model.AkunAkhir,
                    UserLogin = user
                });

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Buku Besar 117");

                    // --- 2. KOP LAPORAN (Merge & Center) ---
                    worksheet.Cell(1, 1).Value = "LAPORAN BUKU BESAR 117";
                    worksheet.Range("A1:G1").Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(2, 1).Value = $"PERIODE : {model.PeriodeAwal} s/d {model.PeriodeAkhir}";
                    worksheet.Range("A2:G2").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // --- 3. HEADER TABEL UTAMA ---
                    int currentRow = 4;
                    var headers = new[] { "No Akun", "Nama Perkiraan", "Tanggal", "No Bukti", "Keterangan", "Debet (Rp)", "Kredit (Rp)" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        var cell = worksheet.Cell(currentRow, i + 1);
                        cell.Value = headers[i];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                    currentRow++;

                    // Setup Lebar Kolom
                    worksheet.Column(1).Width = 15; // No Akun
                    worksheet.Column(2).Width = 20; // Nama
                    worksheet.Column(3).Width = 12; // Tgl
                    worksheet.Column(4).Width = 18; // Bukti
                    worksheet.Column(5).Width = 40; // Keterangan (WrapText)
                    worksheet.Column(6).Width = 18; // Debet
                    worksheet.Column(7).Width = 18; // Kredit
                    worksheet.Column(5).Style.Alignment.WrapText = true;

                    // --- 4. ISI DATA BERDASARKAN GROUPING ---
                    var groupedData = response.RawData.GroupBy(x => x.KodeAkun);

                    foreach (var group in groupedData)
                    {
                        var headerInfo = group.First();
                        decimal saldoAwal = headerInfo.SaldoAwal ?? 0;
                        decimal totalDebetBulanIni = 0;
                        decimal totalKreditBulanIni = 0;

                        // Data Transaksi (Sesuai Logic PDF: RowType == 1)
                        var transaksiList = group.Where(x => x.RowType == 1).OrderBy(x => x.Tanggal).ThenBy(x => x.NoBukti).ToList();
                        bool isFirstRow = true;

                        if (transaksiList.Any())
                        {
                            foreach (var item in transaksiList)
                            {
                                if (isFirstRow) {
                                    worksheet.Cell(currentRow, 1).Value = headerInfo.KodeAkun;
                                    worksheet.Cell(currentRow, 2).Value = headerInfo.NamaAkun;
                                    worksheet.Range(currentRow, 1, currentRow, 2).Style.Font.Bold = true;
                                    // Garis pemisah antar akun (Border Atas Tebal)
                                    worksheet.Range(currentRow, 1, currentRow, 7).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                                }

                                worksheet.Cell(currentRow, 3).Value = item.Tanggal;
                                worksheet.Cell(currentRow, 4).Value = item.NoBukti;
                                worksheet.Cell(currentRow, 5).Value = item.Keterangan ?? "-";
                                worksheet.Cell(currentRow, 6).Value = item.Debet ?? 0;
                                worksheet.Cell(currentRow, 7).Value = item.Kredit ?? 0;

                                // Borders per baris
                                worksheet.Range(currentRow, 1, currentRow, 7).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                                totalDebetBulanIni += item.Debet ?? 0;
                                totalKreditBulanIni += item.Kredit ?? 0;
                                isFirstRow = false;
                                currentRow++;
                            }
                        }
                        else if (saldoAwal != 0) // Case: Gak ada transaksi tapi ada saldo awal
                        {
                            worksheet.Cell(currentRow, 1).Value = headerInfo.KodeAkun;
                            worksheet.Cell(currentRow, 2).Value = headerInfo.NamaAkun;
                            worksheet.Range(currentRow, 1, currentRow, 7).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            currentRow++;
                        }

                        // --- 5. FOOTER PER AKUN (IDENTIK PDF) ---
                        decimal selisihBulanBerjalan = totalDebetBulanIni - totalKreditBulanIni;
                        decimal saldoAkhir = saldoAwal + selisihBulanBerjalan;

                        // Format Angka (Kolom F & G)
                        worksheet.Range(currentRow, 6, currentRow + 3, 7).Style.NumberFormat.Format = "#,##0.00";

                        // Baris: JUMLAH
                        worksheet.Cell(currentRow, 5).Value = "*** J u m l a h";
                        worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        worksheet.Cell(currentRow, 6).Value = totalDebetBulanIni;
                        worksheet.Cell(currentRow, 7).Value = totalKreditBulanIni;
                        worksheet.Range(currentRow, 6, currentRow, 7).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        currentRow++;

                        // Baris: SALDO LALU
                        worksheet.Cell(currentRow, 5).Value = "*** Saldo s/d Bulan Lalu";
                        worksheet.Cell(currentRow, 6).Value = saldoAwal >= 0 ? saldoAwal : 0;
                        worksheet.Cell(currentRow, 7).Value = saldoAwal < 0 ? Math.Abs(saldoAwal) : 0;
                        currentRow++;

                        // Baris: SELISIH
                        worksheet.Cell(currentRow, 5).Value = "*** Selisih Bulan Berjalan";
                        worksheet.Cell(currentRow, 6).Value = selisihBulanBerjalan >= 0 ? selisihBulanBerjalan : 0;
                        worksheet.Cell(currentRow, 7).Value = selisihBulanBerjalan < 0 ? Math.Abs(selisihBulanBerjalan) : 0;
                        currentRow++;

                        // Baris: SALDO INI (BOLD)
                        worksheet.Cell(currentRow, 5).Value = "*** Saldo s/d Bulan Ini";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = saldoAkhir >= 0 ? saldoAkhir : 0;
                        worksheet.Cell(currentRow, 7).Value = saldoAkhir < 0 ? Math.Abs(saldoAkhir) : 0;
                        worksheet.Range(currentRow, 6, currentRow, 7).Style.Font.Bold = true;

                        currentRow += 2; // Kasih spasi antar akun biar gak berhimpitan
                    }

                    // --- 6. EXPORT KE STREAM ---
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return Ok(new { 
                            Status = "OK", 
                            FileName = $"BukuBesar117_{DateTime.Now:yyyyMMddHHmmss}.xlsx", 
                            FileData = Convert.ToBase64String(content) 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.Message });
            }
        }
    }

    public class LaporanBukuBesar117FilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string AkunAwal { get; set; }
        public string AkunAkhir { get; set; }
    }
}