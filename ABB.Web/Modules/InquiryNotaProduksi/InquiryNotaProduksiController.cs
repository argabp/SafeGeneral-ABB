using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Application.InquiryNotaProduksis.Commands;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.InquiryNotaProduksi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;
using ClosedXML.Excel;
using System.IO;

namespace ABB.Web.Modules.InquiryNotaProduksi
{
    public class InquiryNotaProduksiController : AuthorizedBaseController
    {

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }


         public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            var kodeCabangCookie = GetCleanCabangCookie();
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(kodeCabangCookie))
            {
                await HttpContext.SignOutAsync("Identity.Application");

                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserLogin = CurrentUser.UserId;
            
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

            // 4. Kirim ke View
            ViewBag.UserCabangValue = kodeCabangCookie; // Untuk .Value()
            ViewBag.UserCabangText = displayCabang;     // Untuk .Text()

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetInquiryNotaProduksi(
            [DataSourceRequest] DataSourceRequest request,
            string searchKeyword,
            DateTime? startDate,
            DateTime? endDate,
            string jenisAsset,
            string KodeCabang)
        {
            var rawCabang = KodeCabang;
            string kodeCabangs = "";

            if (!string.IsNullOrEmpty(rawCabang))
            {
                rawCabang = rawCabang.Trim();

                if (rawCabang.Length >= 2)
                    kodeCabangs = rawCabang.Substring(rawCabang.Length - 2);
                else
                    kodeCabangs = rawCabang;
            }
            else
            {
                return Json(new List<object>().ToDataSourceResult(request));
            }

            if (string.IsNullOrEmpty(searchKeyword) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(jenisAsset))
            {
                return Json(new List<object>().ToDataSourceResult(request));
            }

            var data = await Mediator.Send(new InquiryNotaProduksiQuery()
            {
                SearchKeyword = searchKeyword,
                StartDate = startDate,
                EndDate = endDate,
                JenisAsset = jenisAsset,
                KodeCabang = kodeCabangs
            });

            var result = data?.ToList() ?? new List<InquiryNotaProduksiDto>();

            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Add(int id)
        {
            var InquiryNotaProduksiDto = await Mediator.Send(new GetInquiryNotaProduksiByIdQuery { id = id });

            if (InquiryNotaProduksiDto == null)
                return NotFound();

            var viewModel = new InquiryNotaProduksiViewModel
            {
                InquiryNotaProduksiHeader = InquiryNotaProduksiDto,
                id = id
            };

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetJenisAssetList()
        {
            // 1. Logic potong string (Sama seperti diatas)
            var rawCabang = Request.Cookies["UserCabang"];
            string kodeCabang = "";

            if (!string.IsNullOrEmpty(rawCabang))
            {
                rawCabang = rawCabang.Trim();
                if (rawCabang.Length >= 2)
                {
                    kodeCabang = rawCabang.Substring(rawCabang.Length - 2);
                }
                else
                {
                    kodeCabang = rawCabang;
                }
            }

            // 2. Kirim ke Query
            var list = await Mediator.Send(new GetDistinctJenisAssetQuery { KodeCabang = kodeCabang });

            var result = list.Select(x => new
            {
                NamaJenisAsset = x,
                KodeJenisAsset = x
            }).ToList();

            return Json(result);
        }

       public async Task<IActionResult> Pembayaran(string no_nd)
        {
            if (string.IsNullOrWhiteSpace(no_nd))
                return BadRequest("No Nota tidak boleh kosong");

            // Ambil data dari Mediator (DTO)
            var data = await Mediator.Send(
                new GetInquiryNotaProduksiPembayaranQuery { NoNota = no_nd });

            if (data == null)
                return NotFound($"Nota {no_nd} tidak ditemukan");

            // Map ke ViewModel
            var model = new InquiryNotaProduksiViewModel
            {
                InquiryNotaProduksiHeader = data.Header,
                PembayaranBankList = data.PembayaranBank,
                PembayaranKasList = data.PembayaranKas,
                PembayaranPiutangList = data.PembayaranPiutang
            };

            return PartialView(model); // sekarang view menerima tipe yang cocok
        }

        public async Task<IActionResult> Keterangan(int id)
        {
            var header = await Mediator.Send(
                new GetInquiryNotaProduksiByIdQuery { id = id });

            if (header == null)
                return NotFound();

            var viewModel = new InquiryNotaProduksiViewModel
            {
                id = id,
                InquiryNotaProduksiHeader = header
            };

            return PartialView(viewModel);
        }


       [HttpPost]
        public async Task<IActionResult> SaveKeterangan(
            [FromBody] SaveKeteranganOSCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var result = await Mediator.Send(command);

            return Json(new
            {
                success = result
            });
        }

        public async Task<IActionResult> GetKeteranganProduksi(
                [DataSourceRequest] DataSourceRequest request,
                string noNota)
            {
                var data = await Mediator.Send(new GetKeteranganProduksiQuery
                {
                    NoNota = noNota
                });

                return Json(data.ToDataSourceResult(request));
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
                return Json(new List<object>());

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });
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
        [HttpPost]
        public async Task<IActionResult> ExportExcel(
            string searchKeyword,
            DateTime? startDate,
            DateTime? endDate,
            string jenisAsset,
            string KodeCabang)
        {
            var rawCabang = KodeCabang?.Trim();
            if (string.IsNullOrEmpty(rawCabang))
                return BadRequest("Kode Cabang kosong");

            var kodeCabangs = rawCabang.Length >= 2
                ? rawCabang.Substring(rawCabang.Length - 2)
                : rawCabang;

            var data = await Mediator.Send(new InquiryNotaProduksiQuery()
            {
                SearchKeyword = searchKeyword,
                StartDate = startDate,
                EndDate = endDate,
                JenisAsset = jenisAsset,
                KodeCabang = kodeCabangs
            });

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Inquiry Nota");

                // --- 1. HEADER (Struktur Awal + Tambahan Request) ---
                worksheet.Cell(1, 1).Value = "Lokasi";
                worksheet.Cell(1, 2).Value = "Tipe";
                worksheet.Cell(1, 3).Value = "Jenis Ass";
                worksheet.Cell(1, 4).Value = "No Referensi";
                worksheet.Cell(1, 5).Value = "No Nota";
                worksheet.Cell(1, 6).Value = "Tanggal Nota";
                worksheet.Cell(1, 7).Value = "No Polis";
                worksheet.Cell(1, 8).Value = "Customer";
                worksheet.Cell(1, 9).Value = "Customer 2";
                worksheet.Cell(1, 10).Value = "D/K";
                worksheet.Cell(1, 11).Value = "Mata Uang";
                worksheet.Cell(1, 12).Value = "Nilai Kurs";
                
                // Tambahan Kolom Baru
                worksheet.Cell(1, 13).Value = "Premi";
                worksheet.Cell(1, 14).Value = "Diskon";
                worksheet.Cell(1, 15).Value = "Polis";
                worksheet.Cell(1, 16).Value = "Materai";
                worksheet.Cell(1, 17).Value = "Komisi";
                worksheet.Cell(1, 18).Value = "Handling Fee";
                worksheet.Cell(1, 19).Value = "Lain-Lain";
                worksheet.Cell(1, 20).Value = "Klaim";
                
                // Kolom Akhir
                worksheet.Cell(1, 21).Value = "Nilai Nota (Netto)";
                worksheet.Cell(1, 22).Value = "Nilai Bayar";
                worksheet.Cell(1, 23).Value = "Saldo";

                // Styling Header biar Bold
                worksheet.Range("A1:W1").Style.Font.Bold = true;
                worksheet.Range("A1:W1").Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.lok;
                    worksheet.Cell(row, 2).Value = item.type;
                    worksheet.Cell(row, 3).Value = item.jn_ass;
                    worksheet.Cell(row, 4).Value = item.no_ref;
                    worksheet.Cell(row, 5).Value = item.no_nd;
                    worksheet.Cell(row, 6).Value = item.date_input;
                    worksheet.Cell(row, 7).Value = item.no_pl;
                    worksheet.Cell(row, 8).Value = item.nm_cust;
                    worksheet.Cell(row, 9).Value = item.nm_cust2;
                    worksheet.Cell(row, 10).Value = item.d_k;
                    worksheet.Cell(row, 11).Value = item.curensi;
                    worksheet.Cell(row, 12).Value = item.kurs;

                    // --- Isi Nilai Keuangan (Pakai n_ field untuk nilainya) ---
                    worksheet.Cell(row, 13).Value = item.premi;      // Premi
                    worksheet.Cell(row, 14).Value = item.n_rabat;    // Nilai Diskon
                    worksheet.Cell(row, 15).Value = item.polis;      // Polis
                    worksheet.Cell(row, 16).Value = item.materai;    // Materai
                    worksheet.Cell(row, 17).Value = item.n_komisi;   // Nilai Komisi
                    worksheet.Cell(row, 18).Value = item.n_hfee;     // Nilai Handling Fee
                    worksheet.Cell(row, 19).Value = item.lain;       // Lain-Lain
                    worksheet.Cell(row, 20).Value = item.klaim;      // Klaim
                    
                    // --- Nilai Akhir ---
                    worksheet.Cell(row, 21).Value = item.netto;      // Nilai Nota
                    worksheet.Cell(row, 22).Value = item.jumlah;     // Nilai Bayar
                    worksheet.Cell(row, 23).Value = item.saldo;      // Saldo

                    // Format angka biar ada pemisah ribuan (Excel format)
                    worksheet.Range(row, 12, row, 23).Style.NumberFormat.Format = "#,##0.00";

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"InquiryNotaFull_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                    );
                }
            }
        }


         [HttpPost]
        public async Task<IActionResult> ExportExcelProduksi(
            string searchKeyword,
            DateTime? startDate,
            DateTime? endDate,
            string jenisAsset,
            string KodeCabang)
        {
            var rawCabang = KodeCabang?.Trim();
            if (string.IsNullOrEmpty(rawCabang))
                return BadRequest("Kode Cabang kosong");

            var kodeCabangs = rawCabang.Length >= 2
                ? rawCabang.Substring(rawCabang.Length - 2)
                : rawCabang;

            var data = await Mediator.Send(new GetCekProdukInquiryNotaProduksiQuery()
            {
                SearchKeyword = searchKeyword,
                StartDate = startDate,
                EndDate = endDate,
                JenisAsset = jenisAsset,
                KodeCabang = kodeCabangs
            });

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cek Produk Inquiry Nota");

                // --- 1. HEADER (Struktur Awal + Tambahan Request) ---
                worksheet.Cell(1, 1).Value = "Lokasi";
                worksheet.Cell(1, 2).Value = "Kode Ass 2";
                worksheet.Cell(1, 3).Value = "Premi";
                worksheet.Cell(1, 4).Value = "Diskon";
                worksheet.Cell(1, 5).Value = "Bruto";
                worksheet.Cell(1, 6).Value = "Polis Materai";
                worksheet.Cell(1, 7).Value = "Komisi";
                worksheet.Cell(1, 8).Value = "Klaim";
                worksheet.Cell(1, 9).Value = "h_fee";
                worksheet.Cell(1, 10).Value = "Lain";
                worksheet.Cell(1, 11).Value = "Netto";


                // Styling Header biar Bold
                worksheet.Range("A1:K1").Style.Font.Bold = true;
                worksheet.Range("A1:K1").Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.lok;
                    worksheet.Cell(row, 2).Value = item.kd_ass2;
                    worksheet.Cell(row, 3).Value = item.premi;
                    worksheet.Cell(row, 4).Value = item.n_rabat;
                    worksheet.Cell(row, 5).Value = item.n_bruto;
                    worksheet.Cell(row, 6).Value = item.polis;
                    worksheet.Cell(row, 7).Value = item.n_komisi;
                    worksheet.Cell(row, 8).Value = item.klaim;
                    worksheet.Cell(row, 9).Value = item.h_fee;
                    worksheet.Cell(row, 10).Value = item.lain;
                    worksheet.Cell(row, 11).Value = item.netto;
                    
                    worksheet.Range(row, 12, row, 23).Style.NumberFormat.Format = "#,##0.00";

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Cek Produk_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                    );
                }
            }
        }

    }
}
