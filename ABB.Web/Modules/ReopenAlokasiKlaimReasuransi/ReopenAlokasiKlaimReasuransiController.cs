using System;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
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

        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangPSTQuery());

            return Json(result);
        }
        
        public async Task<ActionResult> GetReopenAlokasiKlaimReasuransis(GridRequest grid,
            string kodeCabang, DateTime startDate, DateTime endDate)
        {
            var result = await Mediator.Send(new GetReopenAlokasiKlaimReasuransisQuery()
            {
                Grid = grid,
                KodeCabang = kodeCabang,
                StartDate = startDate,
                EndDate = endDate
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> ReopenAlokasiKlaimReasuransi([FromBody] ReopenAlokasiKlaimReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<ReopenAlokasiKlaimReasuransiCommand>(model);
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}