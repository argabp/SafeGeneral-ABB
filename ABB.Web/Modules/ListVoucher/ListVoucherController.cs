using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.KasBanks.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.ListVoucher.Models;
using ABB.Web.Modules.ListVoucherKas.Models;
using ABB.Application.VoucherKass.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using MediatR;
using System.Drawing.Printing;
using ABB.Application.Cabangs.Queries;
using ClosedXML.Excel;
using System.IO;

namespace ABB.Web.Modules.ListVoucher
{
    public class ListVoucherController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IMediator _mediator;

        // Inject Mediator dan Report Generator Service
        public ListVoucherController(IMediator mediator, IReportGeneratorService reportGeneratorService)
        {
            _mediator = mediator;
            _reportGeneratorService = reportGeneratorService;
        }
        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            // --- INI BARU BENAR PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie(); 
            // ---------------------------------------------

            ViewBag.DatabaseName = Request.Cookies["DatabaseName"]; 
            var databaseName = Request.Cookies["DatabaseValue"]; 

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

            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = GetCleanCabangCookie();
            

            // Cek PS10 (Anti Gagal)
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

            // Filter cabang sesuai cookie user
            var filtered = result
               .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); // <-- WAJIB untuk ComboBox

                
            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> GetKasBankList(string tipe, string kodeCabangDropdown) // <--- Tambah parameter ini
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = GetCleanCabangCookie();
            
            // LOGIKA PENTING:
            // Kalau dropdown dikirim (saat PS10 pilih cabang), pakai cabang dropdown.
            // Kalau kosong, pakai cabang bawaan cookie.
            var targetCabang = !string.IsNullOrEmpty(kodeCabangDropdown) ? kodeCabangDropdown : kodeCabangCookie;

            var result = await _mediator.Send(new GetAllKasBankQuery
            {
                TipeKasBank = tipe,
                SearchKeyword = null
            });

            var dataFormatted = result
                .Where(x => x.KodeCabang == targetCabang) // <--- Filter pakai targetCabang, bukan cookie lagi
                .Select(x => new 
                {
                    Kode = x.Kode,
                    Keterangan = x.Keterangan,
                    TampilanLengkap = $"{x.Kode} - {x.Keterangan}" 
                });

            return Json(dataFormatted);
        }

       [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] ListVoucherFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;
                var kodeCabangCookie = GetCleanCabangCookie();
                 var targetCabang = !string.IsNullOrEmpty(model.KodeCabang)
                    ? model.KodeCabang
                    : kodeCabangCookie;

                if (!DateTime.TryParse(model.tglAwal, out DateTime tglAwal))
                    throw new Exception("Tanggal Awal tidak valid");

                if (!DateTime.TryParse(model.tglAkhir, out DateTime tglAkhir))
                    throw new Exception("Tanggal Akhir tidak valid");

                string htmlResult = string.Empty;

                if (model.tipe == "KAS")
                {
                    var response = await _mediator.Send(
                        new GetVoucherKasByTanggalRangeQuery
                        {
                            DatabaseName = databaseName,
                            TanggalAwal = tglAwal,
                            TanggalAkhir = tglAkhir,
                            UserLogin = user,
                            KodeKas = model.kodeKas,
                            KodeCabang = targetCabang
                        });

                    htmlResult = response?.HtmlString;
                }
                else if (model.tipe == "BANK")
                {
                    var response = await _mediator.Send(
                        new GetVoucherBankByTanggalRangeQuery
                        {
                            DatabaseName = databaseName,
                            KodeBank = model.kodeBank,
                            TanggalAwal = tglAwal,
                            TanggalAkhir = tglAkhir,
                            KodeCabang = targetCabang,
                            UserLogin = user
                        });

                    htmlResult = response?.HtmlString;
                }
                else
                {
                    throw new Exception("Tipe voucher tidak valid");
                }

                if (string.IsNullOrWhiteSpace(htmlResult))
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "ListVoucher.pdf",
                    htmlResult,
                    user,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                    PaperKind.A4
                );

                return Ok(new { Status = "OK", Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = "ERROR",
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] ListVoucherFilterDto model)
        {
                try
                {
                    var databaseName = Request.Cookies["DatabaseValue"];
                    var user = CurrentUser.UserId;
                    var kodeCabangCookie = GetCleanCabangCookie();

                    var targetCabang = !string.IsNullOrEmpty(model.KodeCabang)
                    ? model.KodeCabang
                    : kodeCabangCookie;
                    

                    if (!DateTime.TryParse(model.tglAwal, out DateTime tglAwal))
                        throw new Exception("Tanggal Awal tidak valid");

                    if (!DateTime.TryParse(model.tglAkhir, out DateTime tglAkhir))
                        throw new Exception("Tanggal Akhir tidak valid");

                    List<dynamic> data;
                    decimal saldoAwal = 0;
                    decimal totalDebet = 0;
                    decimal totalKredit = 0;
                    decimal saldoAkhir = 0;
                    string namabank = "";
                    string namakas = "";

                    if (model.tipe == "KAS")
                        {
                            var response = await _mediator.Send(
                                new GetVoucherKasByTanggalRangeQuery
                                {
                                    DatabaseName = databaseName,
                                    TanggalAwal = tglAwal,
                                    TanggalAkhir = tglAkhir,
                                    UserLogin = user,
                                    KodeKas = model.kodeKas,
                                    KodeCabang = targetCabang
                                });

                            if (response == null || response.RawData == null)
                                throw new Exception("Data tidak ditemukan.");

                            data = response.RawData.Cast<dynamic>().ToList();

                            saldoAwal = response.SaldoAwal;
                            totalDebet = response.TotalDebet;
                            totalKredit = response.TotalKredit;
                            saldoAkhir = response.SaldoAkhir;
                            namakas = response.NmKas;   // ✅ ini yang benar
                        }
                    else if (model.tipe == "BANK")
                    {
                        var response = await _mediator.Send(
                            new GetVoucherBankByTanggalRangeQuery
                            {
                                DatabaseName = databaseName,
                                KodeBank = model.kodeBank,
                                TanggalAwal = tglAwal,
                                TanggalAkhir = tglAkhir,
                                KodeCabang = targetCabang,
                                UserLogin = user
                            });

                        if (response == null || response.RawData == null)
                            throw new Exception("Data tidak ditemukan.");

                        data = response.RawData.Cast<dynamic>().ToList();

                        saldoAwal = response.SaldoAwal;
                        totalDebet = response.TotalDebet;
                        totalKredit = response.TotalKredit;
                        saldoAkhir = response.SaldoAkhir;
                        namabank = response.NamaBank;
                    }
                    else
                    {
                        throw new Exception("Tipe voucher tidak valid");
                    }

                    if (data == null || !data.Any())
                        throw new Exception("Data tidak ditemukan.");

                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                            var worksheet = workbook.Worksheets.Add("List Voucher");

                            worksheet.Cell(1, 1).Value = $"VOUCHER {model.tipe}";
                            worksheet.Range("A1:F1").Merge(); 
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Baris 2: Lokasi
                            worksheet.Cell(2, 1).Value = $"PERIODE : {model.tglAwal} s/d {model.tglAkhir}";
                            worksheet.Range("A2:F2").Merge(); // <--- BIKIN MERGE KE TENGAH
                            worksheet.Cell(2, 1).Style.Font.Bold = true;
                            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // <--- CENTER TEXT

                            if (model.tipe == "KAS")
                            {
                                worksheet.Cell(3, 1).Value = $"KODE KAS {model.kodeKas} - {namakas} ";
                                worksheet.Range("A3:F3").Merge(); // <--- BIKIN MERGE KE TENGAH
                                worksheet.Cell(3, 1).Style.Font.Bold = true;
                                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }else if (model.tipe == "BANK"){
                                worksheet.Cell(3, 1).Value = $"KODE BANK {model.kodeBank} - {namabank} ";
                                worksheet.Range("A3:F3").Merge(); // <--- BIKIN MERGE KE TENGAH
                                worksheet.Cell(3, 1).Style.Font.Bold = true;
                                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }
                
                            // Baris 3: Periode
                            // <--- CENTER TEXT

                            int startRow = 5; 

                            // Header
                            worksheet.Cell(startRow, 1).Value = "No";
                            worksheet.Cell(startRow, 2).Value = "Tanggal";
                            worksheet.Cell(startRow, 3).Value = "No Voucher";
                            worksheet.Cell(startRow, 4).Value = "Debet";
                            worksheet.Cell(startRow, 5).Value = "Kredit";
                            worksheet.Cell(startRow, 6).Value = "Keterangan";

                            worksheet.Cell(4, 1).Value = "Saldo Awal";
                            worksheet.Cell(4, 4).Value = saldoAwal;
                            worksheet.Range("A4:F4").Style.Font.Bold = true;
                            worksheet.Range("A4:F4").Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(4, 4).Style.NumberFormat.Format = "#,##0.00";

                            int row = 6;
                            int no = 1;

                            foreach (var item in data)
                            {
                                worksheet.Cell(row, 1).Value = no++;
                                worksheet.Cell(row, 2).Value = item.TanggalVoucher;
                                worksheet.Cell(row, 3).Value = item.NoVoucher;

                                if (item.DebetKredit == "D")
                                {
                                    worksheet.Cell(row, 4).Value = item.TotalVoucher;
                                }
                                else
                                {
                                    worksheet.Cell(row, 5).Value = item.TotalVoucher;
                                }

                                worksheet.Cell(row, 6).Value = item.KeteranganVoucher;

                                row++;
                            }

                            worksheet.Cell(row, 3).Value = "TOTAL";
                            worksheet.Cell(row, 4).Value = totalDebet;
                            worksheet.Cell(row, 5).Value = totalKredit;

                                var totalRange = worksheet.Range(row, 1, row, 6);
                                totalRange.Style.Font.Bold = true;
                                totalRange.Style.Fill.BackgroundColor = XLColor.LightGreen;

                                worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                                worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0.00";

                            row++;

                            worksheet.Cell(row, 1).Value = "Saldo Akhir";
                            worksheet.Cell(row, 4).Value = saldoAkhir;

                            var saldoAkhirRange = worksheet.Range(row, 1, row, 6);
                            saldoAkhirRange.Style.Font.Bold = true;
                            saldoAkhirRange.Style.Fill.BackgroundColor = XLColor.LightYellow;

                            var tableRange = worksheet.Range(5, 1, row, 6);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";


                            worksheet.Columns().AdjustToContents();

                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                var base64Str = Convert.ToBase64String(content);

                                return Ok(new { Status = "OK", FileName = "List Voucher.xlsx", FileData = base64Str });
                            }
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new
                    {
                        Status = "ERROR",
                        Message = ex.InnerException?.Message ?? ex.Message
                    });
                }
        }
    }

    public class ListVoucherFilterDto
    {
        public string tipe { get; set; }
        public string KodeCabang { get; set; }
        public string namaCabangLengkap { get; set; }

        public string kodeBank { get; set; }
        public string keterangan { get; set; }

        public string kodeKas { get; set; }
        public string keteranganKas { get; set; }

        public string tglAwal { get; set; }
        public string tglAkhir { get; set; }
    }
}
