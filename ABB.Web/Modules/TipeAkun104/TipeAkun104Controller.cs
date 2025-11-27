using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.TipeAkuns104.Commands;
using ABB.Application.TipeAkuns104.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TipeAkun104.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ABB.Web.Modules.TipeAkun104
{
    public class TipeAkun104Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

          [HttpPost]
        public async Task<ActionResult> GetTipeAkun104([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllTipeAkun104Query() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var model = new TipeAkun104ViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TipeAkun104ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            // var existingData = await Mediator.Send(new GetCoaByIdQuery { Kode = model.Kode });
            // if (existingData != null)
            // {
            //     await Mediator.Send(Mapper.Map<UpdateCoaCommand>(model));
            // }
            // else
            // {
                await Mediator.Send(Mapper.Map<CreateTipeAkun104Command>(model));
           // }
            return Json(new { success = true });
        }

         [HttpGet]
        public async Task<IActionResult> Delete(string Kode)
        {
            await Mediator.Send(new DeleteTipeAkun104Command { Kode = Kode });
            return Json(new { success = true });
        }

         

    }
}