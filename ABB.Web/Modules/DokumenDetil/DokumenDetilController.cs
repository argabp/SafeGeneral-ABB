using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.DokumenDetils.Commands;
using ABB.Application.DokumenDetils.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DokumenDetil.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DokumenDetil
{
    public class DokumenDetilController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetDokumenDetils([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetDokumenDetilsQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDokumenDetil([FromBody] DokumenDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDokumenDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDokumenDetil([FromBody] DokumenDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}