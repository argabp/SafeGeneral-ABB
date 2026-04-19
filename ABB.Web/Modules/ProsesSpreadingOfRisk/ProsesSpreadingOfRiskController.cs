using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesSpreadingOfRisks.Commands;
using ABB.Application.ProsesSpreadingOfRisks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesSpreadingOfRisk.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesSpreadingOfRisk
{
    public class ProsesSpreadingOfRiskController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProsesSpreadingOfRisks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetProsesSpreadingOfRisksQuery()
            {
                Grid = grid
            });

            return Json(result);
        }

        public async Task<IActionResult> AlokasiReasuransi([FromBody] List<ProsesSpreadingOfRiskViewModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    var command = Mapper.Map<AlokasiReasCommand>(model);
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