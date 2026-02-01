using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.KasBanks.Commands;
using ABB.Application.KasBanks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KasBank.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ABB.Application.Coas.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;


namespace ABB.Web.Modules.KasBank
{
    // [Area("KasBank")]
    // [Route("KasBank")]
    public class KasBankController : AuthorizedBaseController
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

         [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // Filter cabang sesuai cookie user
            var filtered = result
                .Where(c => c.kd_cb?.Trim() == kodeCabangCookie.Trim())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); // <-- WAJIB untuk ComboBox

            // kirim ke View kalau ingin dipakai
            ViewBag.UserCabang = kodeCabangCookie;

            return Json(filtered);
        }

        [HttpPost]
        public async Task<ActionResult> GetKasBank([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllKasBankQuery { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // Action untuk menampilkan form Add
        public async Task<IActionResult> Add()
        {

             var databaseName = Request.Cookies["DatabaseValue"]; 
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            ViewBag.TipeKasBankOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Kas", Value = "KAS" },
                new SelectListItem { Text = "Bank", Value = "BANK" }
            };

            var coaList = await Mediator.Send(new GetAllCoaQuery());
            ViewBag.NoPerkiraanOptions = coaList.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}" 
            }).ToList();
            
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var userCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
           
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;

           
            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;

             var model = new KasBankViewModel();
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.TipeKasBankOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Kas", Value = "KAS" },
                new SelectListItem { Text = "Bank", Value = "BANK" }
            };
            
            var coaList = await Mediator.Send(new GetAllCoaQuery());
            ViewBag.NoPerkiraanOptions = coaList.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}" 
            }).ToList();

            var kasBankDto = await Mediator.Send(new GetKasBankByIdQuery { Kode = id });
            if (kasBankDto == null)
            {
                return NotFound();
            }
            var model = Mapper.Map<KasBankViewModel>(kasBankDto);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] KasBankViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            var existingData = await Mediator.Send(new GetKasBankByIdQuery { Kode = model.Kode });
            if (existingData != null)
            {
                await Mediator.Send(Mapper.Map<UpdateKasBankCommand>(model));
            }
            else
            {
                await Mediator.Send(Mapper.Map<CreateKasBankCommand>(model));
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteKasBankCommand { Kode = id });
            return Json(new { success = true });
        }
        
        
    }
}