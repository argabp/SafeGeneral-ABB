using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.TipeAkuns117.Commands;
using ABB.Application.TipeAkuns117.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TipeAkun117.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ABB.Web.Modules.TipeAkun117
{
    public class TipeAkun117Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetTipeAkun117([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllTipeAkun117Query() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // Action untuk membuka form Add
        public async Task<IActionResult> Add()
        {
            var model = new TipeAkun117ViewModel();
            PrepareViewBag(); // Siapkan data dropdown jika ada
            return PartialView("Add", model); // Menggunakan view "Add"
        }

        // Action untuk membuka form Edit
        public async Task<IActionResult> Edit(string id)
        {
            var dto = await Mediator.Send(new GetTipeAkun117ByIdQuery { Kode = id });
            
            if (dto == null)
            {
                return NotFound();
            }

            // Map DTO ke ViewModel
            var model = Mapper.Map<TipeAkun117ViewModel>(dto);
            
            PrepareViewBag();
            return PartialView("Add", model); // Re-use view "Add"
        }

        // Action untuk Simpan (Create atau Update)
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TipeAkun117ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Cek apakah data sudah ada (untuk menentukan Create atau Update)
                var existingData = await Mediator.Send(new GetTipeAkun117ByIdQuery { Kode = model.Kode });

                if (existingData != null)
                {
                    // Mode Update
                    var command = Mapper.Map<UpdateTipeAkun117Command>(model);
                    await Mediator.Send(command);
                }
                else
                {
                    // Mode Create
                    var command = Mapper.Map<CreateTipeAkun117Command>(model);
                    await Mediator.Send(command);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Action untuk Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await Mediator.Send(new DeleteTipeAkun117Command { Kode = id });
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper untuk mengisi Dropdown (opsional, jika kolom Pos butuh dropdown)
        private void PrepareViewBag()
        {
            

            ViewBag.KasBankOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "0 - Bukan Kas/Bank", Value = "false" },
                new SelectListItem { Text = "1 - Kas/Bank", Value = "true" }
            };
        }
    }
}