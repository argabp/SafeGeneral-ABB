using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesAlokasiKlaimReasuransis.Commands;
using ABB.Application.ProsesAlokasiKlaimReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesAlokasiKlaimReasuransi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesAlokasiKlaimReasuransi
{
    public class ProsesAlokasiKlaimReasuransiController : AuthorizedBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProsesAlokasiKlaimReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetProsesAlokasiKlaimReasuransisQuery()
            {
                Grid = grid
            });

            return Json(result);
        }

        public async Task<IActionResult> AlokasiReasuransi([FromBody] List<ProsesAlokasiKlaimReasuransiViewModel> models)
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