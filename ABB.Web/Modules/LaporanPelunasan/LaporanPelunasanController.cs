using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanPelunasans.Queries;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;



namespace ABB.Web.Modules.LaporanPelunasan
{
    public class LaporanPelunasanController : AuthorizedBaseController
    {

        private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanPelunasanController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
            // _viewRenderService = viewRenderService; // Injeksi
        }

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

        public async Task<IActionResult> Index() // Ubah jadi async Task
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var kodeCabangCookie = GetCleanCabangCookie();  
            // 1. Ambil Cookie
            var databaseName = Request.Cookies["DatabaseValue"]; // Pastikan pake DatabaseValue buat query


           // Cek PS10 (Anti Gagal: potong spasi & huruf besar semua)
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

            // 4. Kirim ke View
            // ViewBag.UserCabangValue = kodeCabangCookie; // Untuk .Value()
            // ViewBag.UserCabangText = displayCabang;     // Untuk .Text()

            return View();
        }


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
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // --- INI PERBAIKANNYA: Pake logika isPusat ---
            // Kalau isPusat (PS10), keluarin SEMUA cabang.
            // Kalau bukan PS10, filter sesuai cookie doang.
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

        [HttpGet]
        public async Task<IActionResult> CetakLaporan(
            string KodeCabang,
            string JenisAwal,
            string JenisAkhir,
            string BulanAwal,
            string BulanAkhir,
            string Tahun)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var user = CurrentUser.UserId;

            var query = new GetLaporanPelunasanQuery
            {
                DatabaseName = databaseName,
                KodeCabang = KodeCabang,
                JenisAwal = JenisAwal,
                JenisAkhir = JenisAkhir,
                BulanAwal = BulanAwal,
                BulanAkhir = BulanAkhir,
                Tahun = Tahun,
                UserLogin = user
            };

            var response = await Mediator.Send(query);

            if (response == null || response.RawData == null || !response.RawData.Any())
                return Content("<h3 style='text-align:center;'>Data tidak ditemukan</h3>", "text/html");

            return View("CetakLaporan", response.RawData);
        }

         [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanPelunasanFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanPelunasanQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    JenisAwal = model.JenisAwal,
                    JenisAkhir = model.JenisAkhir,
                    BulanAwal = model.BulanAwal,
                    BulanAkhir = model.BulanAkhir,
                    Tahun = model.Tahun,
                    UserLogin = user
                };

                var response = await Mediator.Send(query);
                
                if (response == null || response.RawData == null || !response.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                var userId = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new Exception("User ID tidak ditemukan. Tidak dapat menyimpan laporan.");                
                
                _reportGeneratorService.GenerateReport(
                    "LaporanPelunasan.pdf",
                    response.HtmlString, 
                    userId,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                   PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = userId });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // --- 3. GENERATE EXCEL (CLOSEDXML) ---
        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanPelunasanFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanPelunasanQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    JenisAwal = model.JenisAwal,
                    JenisAkhir = model.JenisAkhir,
                    BulanAwal = model.BulanAwal,
                    BulanAkhir = model.BulanAkhir,
                    Tahun = model.Tahun,
                    UserLogin = user
                };

                var response = await Mediator.Send(query);
                
                var data = response.RawData;
                
                if (data == null || !data.Any())
                    throw new Exception("Data tidak ditemukan.");
                
                string NamaCabangAja;

                if (string.IsNullOrEmpty(model.KodeCabang))
                {
                    NamaCabangAja = "SEMUA CABANG";
                }
                else
                {
                    NamaCabangAja = model.NamaCabang;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Laporan Pelunasan");

                    // ==========================================
                    // 1. BIKIN KOP LAPORAN (BARIS 1 - 3)
                    // ==========================================
                    
                    // Baris 1: Judul
                    worksheet.Cell(1, 1).Value = "LAPORAN PELUNASAN POS SENDIRI";
                    worksheet.Range("A1:M1").Merge(); 
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Baris 2: Lokasi
                    worksheet.Cell(2, 1).Value = $"LOKASI : {NamaCabangAja}";
                    worksheet.Range("A2:M2").Merge(); // <--- BIKIN MERGE KE TENGAH
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // <--- CENTER TEXT

                    // Baris 3: Periode
                    worksheet.Cell(3, 1).Value = $"PERIODE : {model.BulanAwal} s/d {model.BulanAkhir} - {model.Tahun}";
                    worksheet.Range("A3:M3").Merge(); // <--- BIKIN MERGE KE TENGAH
                    worksheet.Cell(3, 1).Style.Font.Bold = true;
                    worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // <--- CENTER TEXT

                    // ==========================================
                    // 2. SETUP HEADER TABEL (BARIS 5)
                    // ==========================================
                    int startRow = 5; 

                    worksheet.Cell(startRow, 1).Value = "No";
                    worksheet.Cell(startRow, 2).Value = "TGL PRODUKSI";
                    worksheet.Cell(startRow, 3).Value = "No.Nota";
                    worksheet.Cell(startRow, 4).Value = "No.Polis";
                    worksheet.Cell(startRow, 5).Value = "TERTANGGUNG";
                    worksheet.Cell(startRow, 6).Value = "PEMBAWA POS";
                    worksheet.Cell(startRow, 7).Value = "AGEN/BROKER";
                    worksheet.Cell(startRow, 8).Value = "CEDING";
                    worksheet.Cell(startRow, 9).Value = "LOK";
                    worksheet.Cell(startRow, 10).Value = "COB";
                    worksheet.Cell(startRow, 11).Value = "NO.BUKTI";
                    worksheet.Cell(startRow, 12).Value = "TGL LUNAS";
                    worksheet.Cell(startRow, 13).Value = "NILAI LUNAS";

                    var headerRange = worksheet.Range($"A{startRow}:M{startRow}");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Biar Header Tabel di tengah juga

                    // ==========================================
                    // 3. ISI DATA (Anti Kosong/Bolong)
                    // ==========================================
                    int row = 6;
                    int idx = 1;
                    foreach (var item in data)
                    {
                        worksheet.Cell(row, 1).Value = idx;
                        worksheet.Cell(row, 2).Value = item.date.HasValue ? item.date.Value.ToString("dd-MM-yyyy") : "-";
                        
                        // Pake string.IsNullOrWhiteSpace biar kalau datanya "" (kosong) tetep diubah jadi "-"
                        worksheet.Cell(row, 3).Value = string.IsNullOrWhiteSpace(item.no_nd) ? "-" : item.no_nd.Trim();
                        worksheet.Cell(row, 4).Value = string.IsNullOrWhiteSpace(item.no_pl) ? "-" : item.no_pl.Trim();
                        worksheet.Cell(row, 5).Value = string.IsNullOrWhiteSpace(item.nm_cust2) ? "-" : item.nm_cust2.Trim();
                        worksheet.Cell(row, 6).Value = string.IsNullOrWhiteSpace(item.nm_pos) ? "-" : item.nm_pos.Trim();
                        worksheet.Cell(row, 7).Value = string.IsNullOrWhiteSpace(item.nm_brok) ? "-" : item.nm_brok.Trim();
                        worksheet.Cell(row, 8).Value = string.IsNullOrWhiteSpace(item.nm_cust2) ? "-" : item.nm_cust2.Trim(); 
                        worksheet.Cell(row, 9).Value = string.IsNullOrWhiteSpace(item.lok) ? "-" : item.lok.Trim();
                        worksheet.Cell(row, 10).Value = string.IsNullOrWhiteSpace(item.kd_tutup) ? "-" : item.kd_tutup.Trim();
                        worksheet.Cell(row, 11).Value = string.IsNullOrWhiteSpace(item.no_bukti) ? "-" : item.no_bukti.Trim();
                        
                        worksheet.Cell(row, 12).Value = item.tgl_byr.HasValue ? item.tgl_byr.Value.ToString("dd-MM-yyyy") : "-";
                        
                        worksheet.Cell(row, 13).Value = item.jumlah ?? 0;
                        worksheet.Cell(row, 13).Style.NumberFormat.Format = "#,##0.00"; 

                        // Kalau teksnya isinya "-", kita taruh di tengah (center) biar manis diliatnya
                        for (int col = 2; col <= 12; col++)
                        {
                            if (worksheet.Cell(row, col).GetString() == "-")
                            {
                                worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }
                        }

                        row++;
                        idx++;
                    }

                    // Tambahan: Kasih Border ke semua isi tabel biar makin cakep
                    var tableRange = worksheet.Range($"A5:M{row - 1}");
                    tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        var base64Str = Convert.ToBase64String(content);

                        return Ok(new { Status = "OK", FileName = "Laporan_Pelunasan.xlsx", FileData = base64Str });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

    }

    
    public class LaporanPelunasanFilterDto
    {
        public string KodeCabang { get; set; }
        public string NamaCabang { get; set; }
        public string JenisAwal { get; set; }
        public string JenisAkhir { get; set; }
        public string BulanAwal { get; set; }
        public string BulanAkhir { get; set; }
        public string Tahun { get; set; }
    }
    
}