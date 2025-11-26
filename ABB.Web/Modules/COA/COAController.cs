using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Coas.Commands;
using ABB.Application.Coas.Queries;
using ABB.Application.TypeCoas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.COA.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ABB.Web.Modules.COA
{
    public class COAController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

          [HttpPost]
        public async Task<ActionResult> GetCoa([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllCoaQuery() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var model = new CoaViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CoaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            var existingData = await Mediator.Send(new GetCoaByIdQuery { Kode = model.Kode });
            if (existingData != null)
            {
                await Mediator.Send(Mapper.Map<UpdateCoaCommand>(model));
            }
            else
            {
                await Mediator.Send(Mapper.Map<CreateCoaCommand>(model));
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> Edit(string Kode)
        {

                var databaseName = Request.Cookies["DatabaseValue"];
                var dto = await Mediator.Send(new GetCoaByIdQuery { Kode = Kode });
                if (dto == null)
                {
                    return NotFound();
                }
                var model = Mapper.Map<CoaViewModel>(dto);
                return PartialView(model);
        }

         [HttpGet]
        public async Task<IActionResult> Delete(string Kode)
        {
            await Mediator.Send(new DeleteCoaCommand { Kode = Kode });
            return Json(new { success = true });
        }

    
        public IActionResult PilihTypeCoa()
        {
            return PartialView("PilihTypeCoa");
        }

        [HttpPost]
        public async Task<IActionResult> GetTypeCoa([DataSourceRequest] DataSourceRequest request)
        {
            var data = await Mediator.Send(new GetAllTypeCoaQuery());
            return Json(data.ToDataSourceResult(request));
        }  

    }
}