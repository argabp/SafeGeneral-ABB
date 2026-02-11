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

namespace ABB.Web.Modules.InquiryNotaProduksi
{
    public class InquiryNotaProduksiController : AuthorizedBaseController
    {
         public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            var kodeCabangCookie = Request.Cookies["UserCabang"];
            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(kodeCabangCookie))
            {
                await HttpContext.SignOutAsync("Identity.Application");

                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetInquiryNotaProduksi(
            [DataSourceRequest] DataSourceRequest request,
            string searchKeyword,
            DateTime? startDate,
            DateTime? endDate,
            string jenisAsset)
        {
            var rawCabang = Request.Cookies["UserCabang"];
            string kodeCabang = "";
            // 2. LOGIKA AMBIL 2 ANGKA BELAKANG
            if (!string.IsNullOrEmpty(rawCabang))
            {
                // Bersihkan spasi dulu
                rawCabang = rawCabang.Trim(); 

                if (rawCabang.Length >= 2)
                {
                    // Ambil 2 karakter terakhir. Contoh: "JK10" -> "10"
                    kodeCabang = rawCabang.Substring(rawCabang.Length - 2);
                }
                else 
                {
                    // Jaga-jaga kalau isinya cuma "1" atau "5", ambil apa adanya
                    kodeCabang = rawCabang;
                }
            }
            else 
            {
                // Kalau cookie kosong, return kosong (Safety)
                return Json(new List<object>().ToDataSourceResult(request));
            }

            // ✅ Cegah load data jika semua filter kosong
            if (string.IsNullOrEmpty(searchKeyword) &&
                !startDate.HasValue &&
                !endDate.HasValue &&
                string.IsNullOrEmpty(jenisAsset))
            {
                return Json(new List<object>().ToDataSourceResult(request));
            }

            // 🔹 Ambil data sesuai filter
            var data = await Mediator.Send(new InquiryNotaProduksiQuery()
            {
                SearchKeyword = searchKeyword,
                StartDate = startDate,
                EndDate = endDate,
                JenisAsset = jenisAsset,
                KodeCabang = kodeCabang
            });

            // ✅ Jika hasil kosong, kirim response dengan indikator “tidak ditemukan”
            if (data == null || !data.Any())
            {
                var emptyResult = new
                {
                    Errors = "Data tidak ditemukan",
                    Data = new List<object>()
                };
                return Json(emptyResult);
            }

            return Json(await data.ToDataSourceResultAsync(request));
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


    }
}
