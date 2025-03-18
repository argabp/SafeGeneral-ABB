using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Kotas.Commands;
using ABB.Application.Kotas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Kota.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Kota
{
    public class KotaController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetKota([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetKotaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKota([FromBody] KotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Kota"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteKota([FromBody] KotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Kota"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}