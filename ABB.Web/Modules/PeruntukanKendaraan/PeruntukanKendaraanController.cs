using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.PeruntukanKendaraans.Commands;
using ABB.Application.PeruntukanKendaraans.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PeruntukanKendaraan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PeruntukanKendaraan
{
    public class PeruntukanKendaraanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetPeruntukanKendaraan([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetPeruntukanKendaraanQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SavePeruntukanKendaraan([FromBody] PeruntukanKendaraanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SavePeruntukanKendaraanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Peruntukan Kendaraan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeletePeruntukanKendaraan([FromBody] PeruntukanKendaraanViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeletePeruntukanKendaraanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Peruntukan Kendaraan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}