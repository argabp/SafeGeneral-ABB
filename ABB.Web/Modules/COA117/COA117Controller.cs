using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Coas117.Commands;
using ABB.Application.Coas117.Queries;
using ABB.Application.TipeAkuns117.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.COA117.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ABB.Web.Modules.COA117
{
    public class COA117Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

          [HttpPost]
        public async Task<ActionResult> GetCoa117([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllCoa117Query() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var model = new Coa117ViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Coa117ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            var existingData = await Mediator.Send(new GetCoa117ByIdQuery { Kode = model.Kode });
            if (existingData != null)
            {
                await Mediator.Send(Mapper.Map<UpdateCoa117Command>(model));
            }
            else
            {
                await Mediator.Send(Mapper.Map<CreateCoa117Command>(model));
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> Edit(string Kode)
        {

                var databaseName = Request.Cookies["DatabaseValue"];
                var dto = await Mediator.Send(new GetCoa117ByIdQuery { Kode = Kode });
                if (dto == null)
                {
                    return NotFound();
                }
                var model = Mapper.Map<Coa117ViewModel>(dto);
                return PartialView(model);
        }

         [HttpGet]
        public async Task<IActionResult> Delete(string Kode)
        {
            await Mediator.Send(new DeleteCoa117Command { Kode = Kode });
            return Json(new { success = true });
        }

    
        public IActionResult PilihTypeCoa()
        {
            return PartialView("PilihTypeCoa");
        }

        [HttpPost]
        public async Task<IActionResult> GetTypeCoa([DataSourceRequest] DataSourceRequest request)
        {
            var data = await Mediator.Send(new GetAllTipeAkun117Query());
            return Json(data.ToDataSourceResult(request));
        }  

    }
}