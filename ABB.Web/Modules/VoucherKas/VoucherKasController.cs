using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.VoucherKass.Queries;
// tambahan cabang
using ABB.Application.Cabangs.Queries;
// tambahan mata uang
using ABB.Application.MataUangs.Queries;
// tambahan untuk kode akun
using ABB.Application.KasBanks.Queries;

using ABB.Web.Modules.Base;
using ABB.Web.Modules.VoucherKas.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ABB.Web.Modules.VoucherKas
{
    public class VoucherKasController : AuthorizedBaseController
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
        public async Task<ActionResult> GetVoucherKas([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var kodeCabang = Request.Cookies["UserCabang"];

            var data = await Mediator.Send(new GetAllVoucherKasQuery() { 
                SearchKeyword = searchKeyword ,
                KodeCabang = kodeCabang
                
            });
            return Json(await data.ToDataSourceResultAsync(request));
        }

         [HttpGet]
        public async Task<IActionResult> GetKurs(string kodeMataUang, DateTime tanggalVoucher)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            // if (string.IsNullOrEmpty(databaseName))
            // {
            //     return RedirectToAction("Logout", "Account");
            // }
            var kurs = await Mediator.Send(new GetKursMataUangQuery
            {
                DatabaseName = databaseName,
                KodeMataUang = kodeMataUang,
                TanggalVoucher = tanggalVoucher
            });
            return Json(new { nilai_kurs = kurs });
        }

        [HttpGet]
          public async Task<IActionResult> GetNextVoucherNumber()
        {
            var now = DateTime.Now;
            var nextNumber = await Mediator.Send(new GetNextVoucherNumberQuery 
            { 
                Bulan = now.Month, 
                Tahun = now.Year % 100 // Ambil 2 digit terakhir tahun
            });
            
            return Json(new { success = true, nextNumber = nextNumber });
        }


       
         // Action untuk menampilkan form Add
        public async Task<IActionResult> Add()
        {
             var databaseName = Request.Cookies["DatabaseValue"];
                var viewModel = new VoucherKasViewModel();

                var userCabang = Request.Cookies["UserCabang"];
                //  kodecabang
                var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });

               var namaCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), userCabang?.Trim(), StringComparison.OrdinalIgnoreCase))
                ?.nm_cb?.Trim();

                ViewBag.DisplayCabang = $"{userCabang} - {namaCabang}";

                 ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
                    {
                        Value = c.kd_cb.Trim(),
                        Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}",
                    
                    }).ToList();
              
                
                viewModel.KodeCabang = userCabang;
               

                 


                // u/ debetkredit
                ViewBag.DebetKreditOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Pilih..", Value = "" },
                    new SelectListItem { Text = "Kredit", Value = "K" },
                    new SelectListItem { Text = "Debit", Value = "D" }
                    
                };

                // select mata uang
                var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
                ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
                {
                    Value = x.kd_mtu.Trim(),
                    Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
                }).ToList();

                // kodeakun
                var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS" });
                ViewBag.KodeAkun = akunlist.FirstOrDefault()?.NoPerkiraan;
                ViewBag.Kode = akunlist.FirstOrDefault()?.Kode;

                // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };

            
            // var model = new VoucherKasViewModel();
            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] VoucherKasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            var existingData = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = model.NoVoucher });
            if (existingData != null)
            {
                await Mediator.Send(Mapper.Map<UpdateVoucherKasCommand>(model));
            }
            else
            {
                await Mediator.Send(Mapper.Map<CreateVoucherKasCommand>(model));
            }
            return Json(new { success = true });
        }

         // Action untuk menampilkan form Edit (dengan data yang sudah ada)
        public async Task<IActionResult> Edit(string id)
        {

             var databaseName = Request.Cookies["DatabaseValue"];
            // if (string.IsNullOrEmpty(databaseName))
            //     {
            //         return RedirectToAction("Logout", "Account");
            //     }
                // kode cabang
                var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
                ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
                {
                    Value = c.kd_cb.Trim(),
                    Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}"
                }).ToList();

                // u/debetkredit
                ViewBag.DebetKreditOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Debit", Value = "D" },
                    new SelectListItem { Text = "Kredit", Value = "K" }
                };

                // select mata uang
                var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
                ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
                {
                    Value = x.kd_mtu.Trim(),
                    Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
                }).ToList();

                // kodeakun
                var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS" });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{int.Parse(x.NoPerkiraan):N0} - {x.Keterangan}" // Format: NO PERKIRAAN - NAMA BANK
                }).ToList();

                // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };

                var dto = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = id });
                if (dto == null)
                {
                    return NotFound();
                }
                var model = Mapper.Map<VoucherKasViewModel>(dto);
                return PartialView(model);
        }

        // Action untuk menghapus data
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteVoucherKasCommand { NoVoucher = id });
            return Json(new { success = true });
        }

    }
}