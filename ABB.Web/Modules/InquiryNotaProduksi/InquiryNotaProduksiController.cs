using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.InquiryNotaProduksis.Queries;
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
                JenisAsset = jenisAsset
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
            var list = await Mediator.Send(new GetDistinctJenisAssetQuery());

            var result = list.Select(x => new
            {
                NamaJenisAsset = x,
                KodeJenisAsset = x
            }).ToList();

            return Json(result);
        }
    }
}
