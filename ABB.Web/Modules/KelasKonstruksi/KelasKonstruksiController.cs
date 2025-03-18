using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.KelasKonsturksis.Commands;
using ABB.Application.KelasKonsturksis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KelasKonstruksi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KelasKonstruksi
{
    public class KelasKonstruksiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetKelasKonstruksi([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetKelasKonstruksiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKelasKonstruksi([FromBody] KelasKonstruksiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKelasKonstruksiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Kelas Konstruksi"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteKelasKonstruksi([FromBody] KelasKonstruksiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKelasKonstruksiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Kelas Konstruksi"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}