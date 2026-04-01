using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanOutstandings.Queries;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ClosedXML.Excel;
using System.IO;

namespace ABB.Web.Modules.LaporanOutstanding
{
    public class LaporanOutstandingController : AuthorizedBaseController
    {

        private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanOutstandingController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
            // _viewRenderService = viewRenderService; // Injeksi
        }

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

       public async Task<IActionResult> Index()
        {
            var databaseName = Request.Cookies["DatabaseValue"]; 
            
            // --- 2. GUNAKAN FUNGSI PEMBERSIH DI SINI ---
            var kodeCabangCookie = GetCleanCabangCookie();

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(kodeCabangCookie))
            {
                await HttpContext.SignOutAsync("Identity.Application");
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            // --- 3. TAMBAHKAN LOGIKA PS10 DI SINI ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            ViewBag.IsPusat = isPusat;
            // ----------------------------------------

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
           
            var userCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;
           
            if (isPusat)
                {
                    // User pusat → jangan isi value
                    ViewBag.UserCabangValue = "";
                    ViewBag.UserCabangText = "";
                }
                else
                {
                    // User cabang → otomatis sesuai login
                    ViewBag.UserCabangValue = kodeCabangCookie;
                    ViewBag.UserCabangText = displayCabang;
                }
    
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            
            // --- 4. GUNAKAN FUNGSI PEMBERSIH DI SINI ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 5. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); 

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

            return Json(filtered);
        }

        [HttpGet]
            public async Task<IActionResult> GetJenisAssetList()
        {
            var list = await Mediator.Send(new GetDistinctJenisAssetQuery());

            var result = list.Select(x => new
            {
                NamaJenisAsset = x,
                KodeJenisAsset = x
            }).ToList();

            return Json(result);
        }



        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanOutstandingFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan.");

                // Sesuaikan QUERY dengan data yang datang dari JS
                var query = new GetLaporanOutstandingQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    TglProduksiAwal = model.TglProduksiAwal,
                    TglProduksiAkhir = model.TglProduksiAkhir,
                    TglPelunasan = model.TglPelunasan,
                    JenisLaporan = model.JenisLaporan,
                    JenisTransaksi = model.JenisTransaksi,
                    SelectedCodes = model.SelectedCodes,
                    UserLogin = user
                };

                var response = await Mediator.Send(query);

                // Cek RawData (bukan Any() dari string lagi)
                if (response == null || response.RawData == null || !response.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanOutstanding.pdf",
                    response.HtmlString,
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
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanOutstandingFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan.");

                // Sesuaikan QUERY dengan data yang datang dari JS
                var query = new GetLaporanOutstandingQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    TglProduksiAwal = model.TglProduksiAwal,
                    TglProduksiAkhir = model.TglProduksiAkhir,
                    TglPelunasan = model.TglPelunasan,
                    JenisLaporan = model.JenisLaporan,
                    JenisTransaksi = model.JenisTransaksi,
                    SelectedCodes = model.SelectedCodes,
                    UserLogin = user
                };

                var response = await Mediator.Send(query);
                
                var data = response.RawData;

                if (data == null || !data.Any())
                    throw new Exception("Data tidak ditemukan.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Laporan Outstanding");

                    string displayLabel = "DETAIL (KESELURUHAN)"; 
                    switch (model.JenisLaporan)
                    {
                        case "Pos": displayLabel = "PEMBAWA POS"; break;
                        case "Broker": displayLabel = "AGEN/BROKER"; break;
                        case "COB": displayLabel = "COB"; break;
                        case "Tertanggung": displayLabel = "TERTANGGUNG"; break;
                        case "Detail": default: displayLabel = "DETAIL (KESELURUHAN)"; break;
                    }

                    worksheet.Cell(1, 1).Value = $"{displayLabel}";
                    worksheet.Range("A1:T1").Merge(); 
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                  // Baris 2: Posisi
                    worksheet.Cell(2, 1).Value = $"POSISI : {DateTime.Now:dd-MM-yyyy}";
                    worksheet.Range("A2:T2").Merge();
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Baris 3: Lokasi
                    worksheet.Cell(3, 1).Value = $"LOKASI : {model.KodeCabang} - {model.NamaCabang}";
                    worksheet.Range("A3:T3").Merge();
                    worksheet.Cell(3, 1).Style.Font.Bold = true;
                    worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Geser baris berikutnya
                    worksheet.Cell(4, 1).Value = $"PRODUKSI : {model.TglProduksiAwal} s/d {model.TglProduksiAkhir}";
                    worksheet.Range("A4:T4").Merge();
                    worksheet.Cell(4, 1).Style.Font.Bold = true;
                    worksheet.Cell(4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(5, 1).Value = $"PELUNASAN S/D : {model.TglPelunasan}";
                    worksheet.Range("A5:T5").Merge();
                    worksheet.Cell(5, 1).Style.Font.Bold = true;
                    worksheet.Cell(5, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    //judul table
                    int startRow = 7; 

                    worksheet.Cell(startRow, 1).Value = "No";
                    worksheet.Cell(startRow, 2).Value = "NO NOTA";
                    worksheet.Cell(startRow, 3).Value = "No.POLIS";
                    worksheet.Cell(startRow, 4).Value = "TERTANGGUNG";
                    worksheet.Cell(startRow, 5).Value = "PEMBAWA POS";
                    worksheet.Cell(startRow, 6).Value = "AGEN";
                    worksheet.Cell(startRow, 7).Value = "BROKER";
                    worksheet.Cell(startRow, 8).Value = "NO POLIS LEADER";
                    worksheet.Cell(startRow, 9).Value = "CEDING";
                    worksheet.Cell(startRow, 10).Value = "LOKASI";
                    worksheet.Cell(startRow, 11).Value = "COB";
                    worksheet.Cell(startRow, 12).Value = "TGL NOTA";
                    worksheet.Cell(startRow, 13).Value = "JATUH TEMPO";
                    worksheet.Cell(startRow, 14).Value = "CURRENCY";
                    worksheet.Cell(startRow, 15).Value = "KURS";
                    worksheet.Cell(startRow, 16).Value = "UMUR";
                    worksheet.Cell(startRow, 17).Value = "RANGE";
                    worksheet.Cell(startRow, 18).Value = "NILAI NOTA (IDR)";
                    worksheet.Cell(startRow, 19).Value = "NILAI BAYAR (IDR)";
                    worksheet.Cell(startRow, 20).Value = "NILAI OS(IDR)";

                    var headerRange = worksheet.Range($"A{startRow}:T{startRow}");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    int row = startRow + 1;
                   

                    Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "";
                    Func<decimal?, decimal> fmtDec = d => d ?? 0;

                    // ===== GROUPING =====
                    // Menggunakan var adalah cara termudah untuk menghindari mismatch tipe data LINQ
                    var groupedData = model.JenisLaporan switch {
                        "COB" => response.RawData.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key).AsEnumerable(),
                        "Tertanggung" => response.RawData.GroupBy(x => x.nm_cust ?? "TANPA NAMA").OrderBy(g => g.Key).AsEnumerable(),
                        "Pos" => response.RawData.GroupBy(x => x.nm_pos ?? "TANPA POS").OrderBy(g => g.Key).AsEnumerable(),
                        "Broker" => response.RawData.GroupBy(x => x.nm_brok ?? "DIRECT").OrderBy(g => g.Key).AsEnumerable(),
                        _ => response.RawData.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key).AsEnumerable()
                    };

                    if (model.JenisLaporan == "Detail" || string.IsNullOrEmpty(model.JenisLaporan))
                    {
                        groupedData = data.GroupBy(x => "DETAIL");
                    }
                    else
                    {
                        switch (model.JenisLaporan)
                        {
                            case "COB":
                                groupedData = data.GroupBy(x => x.jn_ass ?? "LAINNYA");
                                break;
                            case "Tertanggung":
                                groupedData = data.GroupBy(x => x.nm_cust ?? "TANPA NAMA");
                                break;
                            case "Pos":
                                groupedData = data.GroupBy(x => x.nm_pos ?? "TANPA POS");
                                break;
                            case "Broker":
                                groupedData = data.GroupBy(x => x.nm_brok ?? "DIRECT");
                                break;
                            default:
                                groupedData = data.GroupBy(x => "DETAIL");
                                break;
                        }
                    }

                    decimal grandNota = 0;
                    decimal grandBayar = 0;

                    foreach (var group in groupedData)
                    {
                         int no = 1;
                        // ===== JUDUL GROUP =====
                        if (model.JenisLaporan != "Detail")
                        {
                            worksheet.Cell(row, 1).Value = $"{displayLabel} : {group.Key}";
                            worksheet.Range(row, 1, row, 20).Merge();
                            worksheet.Cell(row, 1).Style.Font.Bold = true;
                            worksheet.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#E6F7FF");
                            row++;
                        }

                        decimal subNota = 0;
                        decimal subBayar = 0;

                        foreach (var item in group)
                        {
                            int umur = 0;
                            if (item.date.HasValue && item.tgl_jth_tempo.HasValue)
                            {
                                umur = (item.tgl_jth_tempo.Value - item.date.Value).Days;
                                if (umur < 0) umur = 0;
                            }

                            decimal nNota = fmtDec(item.netto);
                            decimal nBayar = fmtDec(item.jumlah);
                            decimal nOs = nNota - nBayar;

                            worksheet.Cell(row, 1).Value = no++;
                            worksheet.Cell(row, 2).Value = item.no_nd;
                            worksheet.Cell(row, 3).Value = item.no_pl;
                            worksheet.Cell(row, 4).Value = item.nm_cust;
                            worksheet.Cell(row, 5).Value = item.nm_pos;
                            worksheet.Cell(row, 6).Value = item.nm_brok;
                            worksheet.Cell(row, 7).Value = item.nm_brok;
                            worksheet.Cell(row, 8).Value = "";
                            worksheet.Cell(row, 9).Value = "";
                            worksheet.Cell(row, 10).Value = item.lok;
                            worksheet.Cell(row, 11).Value = item.jn_ass;
                            if (item.date.HasValue)
                                {
                                    worksheet.Cell(row, 12).Value = item.date.Value;
                                    worksheet.Cell(row, 12).Style.DateFormat.Format = "dd-MM-yyyy";
                                }
                                else
                                {
                                    worksheet.Cell(row, 12).Value = "";
                                }

                                if (item.tgl_jth_tempo.HasValue)
                                {
                                    worksheet.Cell(row, 13).Value = item.tgl_jth_tempo.Value;
                                    worksheet.Cell(row, 13).Style.DateFormat.Format = "dd-MM-yyyy";
                                }
                                else
                                {
                                    worksheet.Cell(row, 13).Value = "";
                                }
                            worksheet.Cell(row, 14).Value = item.curensi;
                            worksheet.Cell(row, 15).Value = item.kurs;
                            worksheet.Cell(row, 16).Value = umur;
                            worksheet.Cell(row, 17).Value = ""; // range kalau mau ditambah aging
                            worksheet.Cell(row, 18).Value = nNota;
                            worksheet.Cell(row, 19).Value = nBayar;
                            worksheet.Cell(row, 20).Value = nOs;

                            worksheet.Range(row, 18, row, 20).Style.NumberFormat.Format = "#,##0.00";

                            subNota += nNota;
                            subBayar += nBayar;

                            row++;
                        }

                        decimal subOs = subNota - subBayar;

                            // ===== SUBTOTAL =====
                            worksheet.Cell(row, 1).Value = "TOTAL :";
                            worksheet.Range(row, 1, row, 17).Merge();
                            worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(row, 1).Style.Font.Bold = true;

                            worksheet.Cell(row, 18).Value = subNota;
                            worksheet.Cell(row, 19).Value = subBayar;
                            worksheet.Cell(row, 20).Value = subOs;

                            worksheet.Range(row, 18, row, 20).Style.NumberFormat.Format = "#,##0.00";
                            worksheet.Range(row, 1, row, 20).Style.Font.Bold = true;
                            worksheet.Range(row, 1, row, 20).Style.Fill.BackgroundColor = XLColor.LightGray;

                            grandNota += subNota;
                            grandBayar += subBayar;

                            row++;
                    }

                        // ===== GRAND TOTAL =====
                        decimal grandOs = grandNota - grandBayar;

                        worksheet.Cell(row, 1).Value = "GRAND TOTAL :";
                        worksheet.Range(row, 1, row, 17).Merge();
                        worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        worksheet.Cell(row, 1).Style.Font.Bold = true;

                        worksheet.Cell(row, 18).Value = grandNota;
                        worksheet.Cell(row, 19).Value = grandBayar;
                        worksheet.Cell(row, 20).Value = grandOs;

                        worksheet.Range(row, 18, row, 20).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Range(row, 1, row, 20).Style.Font.Bold = true;
                        worksheet.Range(row, 1, row, 20).Style.Fill.BackgroundColor = XLColor.Yellow;
                        
                        worksheet.Columns().AdjustToContents();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            var base64Str = Convert.ToBase64String(content);

                            return Ok(new { Status = "OK", FileName = "Laporan_Outstanding.xlsx", FileData = base64Str });
                        }

                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }

    public class LaporanOutstandingFilterDto
    {
        public string KodeCabang { get; set; }
        public string NamaCabang { get; set; }
        public string TglProduksiAwal { get; set; }
        public string TglProduksiAkhir { get; set; }
        public string TglPelunasan { get; set; }
        public string JenisLaporan { get; set; }
        public string JenisTransaksi { get; set; }
        public List<string> SelectedCodes { get; set; }
    }
}