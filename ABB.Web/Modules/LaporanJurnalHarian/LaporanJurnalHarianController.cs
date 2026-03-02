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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanJurnalHarians.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Application.JenisTransaksis.Queries;
using ClosedXML.Excel;
using System.IO;


namespace ABB.Web.Modules.LaporanJurnalHarian
{
    public class LaporanJurnalHarianController : AuthorizedBaseController
    {

         private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanJurnalHarianController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
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
            
            // --- 2. PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 3. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            ViewBag.IsPusat = isPusat;
            // ------------------------

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
           
            var userCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;
           
            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;
            return View();
        }

         [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            
            // --- 4. PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 5. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // --- 6. FILTER PAKAI isPusat || ---
            var filtered = result
                .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); 

            ViewBag.UserCabang = kodeCabangCookie;

            return Json(filtered);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanJurnalHarianFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanJurnalHarianQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    JenisTransaksi = model.JenisTransaksi,
                    UserLogin = user
                };

                // 1. Ganti nama biar enak dibaca
                var result = await Mediator.Send(query);

                // 2. Cek RawData-nya ada isinya gak
                if (result.RawData == null || !result.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                // 3. Pakai properti HtmlString
                _reportGeneratorService.GenerateReport(
                    "LaporanJurnalHarian.pdf",
                    result.HtmlString, // <--- Ini kuncinya
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

         [HttpGet]
            public async Task<IActionResult> GetJenisTransaksi()
        {
            var list = await Mediator.Send(new GetAllJenisTransaksiQuery());

            var result = list.Select(x => new
            {
                NamaJenisTransaksi = $"{x.nama.Trim()} ({x.kode.Trim()})",
                KodeJenisTransaksi = x.kode.Trim()
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanJurnalHarianFilterDto model)
        {
            try
            {
                var response = await Mediator.Send(new GetLaporanJurnalHarianQuery {
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    JenisTransaksi = model.JenisTransaksi
                });

               using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Jurnal Harian");

                    // ==========================================
                    // 1. KOP LAPORAN (Merge ke Tengah & Bold)
                    // ==========================================
                    // Baris 1: Judul
                    worksheet.Cell(1, 1).Value = "LAPORAN JURNAL HARIAN";
                    worksheet.Range("A1:H1").Merge();
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Baris 2: Periode
                    worksheet.Cell(2, 1).Value = $"PERIODE : {model.PeriodeAwal} s/d {model.PeriodeAkhir}";
                    worksheet.Range("A2:H2").Merge();
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // ==========================================
                    // 2. SETUP LEBAR KOLOM (Lega & Aesthetic)
                    // ==========================================
                    worksheet.Column(1).Width = 5;  // No
                    worksheet.Column(2).Width = 15; // Tanggal
                    worksheet.Column(3).Width = 15; // Akun
                    worksheet.Column(4).Width = 8;  // Mtu
                    worksheet.Column(5).Width = 18; // Nilai Org
                    worksheet.Column(6).Width = 20; // Debet (IDR)
                    worksheet.Column(7).Width = 20; // Kredit (IDR)
                    worksheet.Column(8).Width = 45; // Keterangan (Paling lebar)
                    worksheet.Column(8).Style.Alignment.WrapText = true; // Bungkus teks panjang

                    // ==========================================
                    // 3. HEADER TABEL (Baris 4)
                    // ==========================================
                    int currentRow = 4;
                    var headers = new[] { "No", "Tanggal", "Akun", "Mtu", "Nilai Org", "Debet (IDR)", "Kredit (IDR)", "Keterangan" };
                    for (int i = 0; i < headers.Length; i++) {
                        worksheet.Cell(currentRow, i + 1).Value = headers[i];
                    }
                    var headerRange = worksheet.Range(currentRow, 1, currentRow, 8);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;

                    // ==========================================
                    // 4. ISI DATA DENGAN GROUPING & BORDER
                    // ==========================================
                    var groupedData = response.RawData.GroupBy(x => (string)x.GlBukti);

                    foreach (var group in groupedData)
                    {
                        // --- Header No. Jurnal (Warna Abu Muda) ---
                        var groupHeader = worksheet.Range(currentRow, 1, currentRow, 8);
                        groupHeader.Merge().Value = $"No. Jurnal : {group.Key}";
                        groupHeader.Style.Font.Bold = true;
                        groupHeader.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                        groupHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        currentRow++;

                        decimal subD = 0, subK = 0;
                        int idx = 1;

                        foreach (var item in group)
                        {
                            worksheet.Cell(currentRow, 1).Value = idx++;
                            worksheet.Cell(currentRow, 2).Value = item.GlTanggal?.ToString("dd/MM/yyyy") ?? "-";
                            worksheet.Cell(currentRow, 3).Value = string.IsNullOrWhiteSpace(item.GlAkun) ? "-" : item.GlAkun.Trim();
                            worksheet.Cell(currentRow, 4).Value = string.IsNullOrWhiteSpace(item.GlMtu) ? "-" : item.GlMtu.Trim();
                            
                            worksheet.Cell(currentRow, 5).Value = item.GlNilaiOrg ?? 0;
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0.00";

                            decimal d = item.GlDk == "D" ? (item.GlNilaiIdr ?? 0) : 0;
                            decimal k = item.GlDk == "K" ? (item.GlNilaiIdr ?? 0) : 0;
                            
                            worksheet.Cell(currentRow, 6).Value = d;
                            worksheet.Cell(currentRow, 7).Value = k;
                            worksheet.Cell(currentRow, 8).Value = string.IsNullOrWhiteSpace(item.GlKet) ? "-" : item.GlKet.Trim();

                            // Format angka untuk Debet & Kredit
                            worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0.00";
                            worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "#,##0.00";

                            // Pasang Border di tiap baris data
                            var rowRange = worksheet.Range(currentRow, 1, currentRow, 8);
                            rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            
                            // Pusatkan teks untuk No, Tgl, Akun, Mtu
                            worksheet.Range(currentRow, 1, currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            subD += d; subK += k;
                            currentRow++;
                        }

                        // --- Subtotal Row (Warna Biru Muda Aesthetic) ---
                        var subTotalRange = worksheet.Range(currentRow, 1, currentRow, 8);
                        worksheet.Cell(currentRow, 5).Value = "Sub Total :";
                        worksheet.Cell(currentRow, 6).Value = subD;
                        worksheet.Cell(currentRow, 7).Value = subK;

                        subTotalRange.Style.Font.Bold = true;
                        subTotalRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#E3F2FD"); // Light Blue
                        subTotalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        subTotalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "#,##0.00";

                        currentRow += 2; // Kasih jarak antar jurnal
                    }

                    // ==========================================
                    // 5. DOWNLOAD FILE
                    // ==========================================
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return Ok(new { Status = "OK", FileName = "Laporan_Jurnal_Harian.xlsx", FileData = Convert.ToBase64String(content) });
                    }
                }
            }
            catch (Exception ex) { return Ok(new { Status = "ERROR", Message = ex.Message }); }
        }
    }

      public class LaporanJurnalHarianFilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string JenisTransaksi { get; set; }
    }
}