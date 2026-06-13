using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Queries;
using ABB.Application.ReopenSpreadingOfRisks.Commands;
using ABB.Application.ReopenSpreadingOfRisks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ReopenSpreadingOfRisk.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ReopenSpreadingOfRisk
{
    public class ReopenSpreadingOfRiskController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        // [HttpPost]
        // public async Task<IActionResult> GetReopenSpreadingOfRisks(GridRequest grid)
        // {
        //     var result = await Mediator.Send(new GetReopenSpreadingOfRisksQuery()
        //     {
        //         Grid = grid
        //     });
        //
        //     return Json(result);
        // }

        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangPSTQuery());

            return Json(result);
        }

        public async Task<ActionResult> GetReopenSpreadingOfRisks([DataSourceRequest] DataSourceRequest request, 
            string kodeCabang, DateTime startDate, DateTime endDate)
        {
            var ds = await Mediator.Send(new GetReopenSpreadingOfRisksQuery()
            {
                KodeCabang = kodeCabang,
                StartDate = startDate,
                EndDate = endDate
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<IActionResult> Reopen([FromBody] List<ReopenSpreadingOfRiskViewModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    var command = Mapper.Map<ReopenSpreadingOfRiskCommand>(model);
                    await Mediator.Send(command);
                }
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}