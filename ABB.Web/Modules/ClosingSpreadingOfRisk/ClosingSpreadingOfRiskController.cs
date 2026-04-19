using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.ClosingSpreadingOfRisks.Commands;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ClosingSpreadingOfRisks.Queries;
using ABB.Application.Common;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ClosingSpreadingOfRisk.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ClosingSpreadingOfRisk
{
    public class ClosingSpreadingOfRiskController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetClosingSpreadingOfRisks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetClosingSpreadingOfRisksQuery()
            {
                Grid = grid
            });

            return Json(result);
        }

        public async Task<IActionResult> Closing([FromBody] List<ClosingSpreadingOfRiskViewModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    var command = Mapper.Map<ClosingSpreadingOfRiskCommand>(model);
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