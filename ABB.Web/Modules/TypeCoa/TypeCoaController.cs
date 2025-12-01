using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.TypeCoas.Commands;
using ABB.Application.TypeCoas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TypeCoa.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ABB.Web.Modules.TypeCoa
{
    public class TypeCoaController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

          [HttpPost]
        public async Task<ActionResult> GetTypeCoa([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllTypeCoaQuery() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {

               var databaseName = Request.Cookies["DatabaseValue"];
                ViewBag.DebetKreditOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Pilih..", Value = "" },
                    new SelectListItem { Text = "Kredit", Value = "K" },
                    new SelectListItem { Text = "Debit", Value = "D" }
                    
                };

            var model = new TypeCoaViewModel();
            PrepareViewBag(); // Siapkan data dropdown jika ada
            return PartialView("Add", model); // Menggunakan view "Add"
        }

        // Action untuk membuka form Edit
        public async Task<IActionResult> Edit(string id)
        {

             var databaseName = Request.Cookies["DatabaseValue"];
            ViewBag.DebetKreditOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pilih..", Value = "" },
                new SelectListItem { Text = "Kredit", Value = "K" },
                new SelectListItem { Text = "Debit", Value = "D" }
                
            };

            var dto = await Mediator.Send(new GetTypeCoaByIdQuery { Type = id });
            
            if (dto == null)
            {
                return NotFound();
            }

            // Map DTO ke ViewModel
            var model = Mapper.Map<TypeCoaViewModel>(dto);
            
            PrepareViewBag();
            return PartialView("Add", model); // Re-use view "Add"
        }

       [HttpPost]
        public async Task<IActionResult> Save([FromBody] TypeCoaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Cek apakah data sudah ada (untuk menentukan Create atau Update)
                var existingData = await Mediator.Send(new GetTypeCoaByIdQuery { Type = model.Type });

                if (existingData != null)
                {
                    // Mode Update
                    var command = Mapper.Map<UpdateTypeCoaCommand>(model);
                    await Mediator.Send(command);
                }
                else
                {
                    // Mode Create
                    var command = Mapper.Map<CreateTypeCoaCommand>(model);
                    await Mediator.Send(command);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

         [HttpGet]
        public async Task<IActionResult> Delete(string Type)
        {
            await Mediator.Send(new DeleteTypeCoaCommand { Type = Type });
            return Json(new { success = true });
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