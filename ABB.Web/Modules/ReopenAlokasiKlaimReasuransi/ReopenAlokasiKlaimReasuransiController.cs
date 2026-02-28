using System;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ReopenAlokasiKlaimReasuransis.Commands;
using ABB.Application.ReopenAlokasiKlaimReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ReopenAlokasiKlaimReasuransi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ReopenAlokasiKlaimReasuransi
{
    public class ReopenAlokasiKlaimReasuransiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetReopenAlokasiKlaimReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetReopenAlokasiKlaimReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> ReopenAlokasiKlaimReasuransi([FromBody] ReopenAlokasiKlaimReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<ReopenAlokasiKlaimReasuransiCommand>(model);
                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}