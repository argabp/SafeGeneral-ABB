using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Coverages.Commands;
using ABB.Application.Coverages.Queries;
using ABB.Application.Kotas.Commands;
using ABB.Application.Kotas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Coverage.Models;
using ABB.Web.Modules.Kota.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Coverage
{
    public class CoverageController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetCoverages([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetCoverageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveCoverage([FromBody] CoverageViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveCoverageCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Coverage"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteCoverage([FromBody] CoverageViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteCoverageCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Coverage"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}