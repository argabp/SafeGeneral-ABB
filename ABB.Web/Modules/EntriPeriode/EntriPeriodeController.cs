using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.EntriPeriodes.Commands;
using ABB.Application.EntriPeriodes.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriPeriode.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ABB.Web.Modules.EntriPeriode
{
    public class EntriPeriodeController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            // ViewBag.Module = Request.Cookies["Module"];
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            // Tidak butuh databaseName karena pakai view di pst_nota
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetPeriode([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // Hapus parameter flagClosing di sini agar mengambil SEMUA data
            var data = await Mediator.Send(new GetAllPeriodeQuery 
            { 
                SearchKeyword = searchKeyword 
            });
            
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {
            var viewModel = new EntriPeriodeViewModel
            {
                // Set default values jika perlu
                TglMul = DateTime.Now,
                TglAkh = DateTime.Now.AddMonths(1),
                FlagClosing = "N"
            };
            
            PrepareViewBag();
            return PartialView("Add", viewModel);
        }

        public async Task<IActionResult> Edit(decimal thn, short bln)
        {
            var dto = await Mediator.Send(new GetPeriodeByIdQuery { ThnPrd = thn, BlnPrd = bln });
            
            if (dto == null) return NotFound();

            var viewModel = Mapper.Map<EntriPeriodeViewModel>(dto);
            
            PrepareViewBag();
            return PartialView("Add", viewModel); // Gunakan view yang sama (Add.cshtml)
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriPeriodeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Cek apakah data sudah ada untuk menentukan Create atau Update
                var existing = await Mediator.Send(new GetPeriodeByIdQuery 
                { 
                    ThnPrd = model.ThnPrd.Value, 
                    BlnPrd = model.BlnPrd.Value 
                });

                if (existing != null)
                {
                    // Update
                    var command = Mapper.Map<UpdateEntriPeriodeCommand>(model);
                    await Mediator.Send(command);
                }
                else
                {
                    // Create
                    var command = Mapper.Map<CreateEntriPeriodeCommand>(model);
                    await Mediator.Send(command);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(decimal thn, short bln)
        {
            try
            {
                await Mediator.Send(new DeleteEntriPeriodeCommand { ThnPrd = thn, BlnPrd = bln });
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void PrepareViewBag()
        {
            ViewBag.FlagClosingOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Open (N)", Value = "N" },
                new SelectListItem { Text = "Closed (Y)", Value = "Y" }
            };
        }
    }
}