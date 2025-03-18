using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.RenewalPolis.Commands;
using ABB.Application.RenewalPolis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RenewalPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RenewalPolis
{
    public class RenewalPolisController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetRenewalPolis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRenewalPolisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> RenewalPolis([FromBody] RenewalPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<RenewalPolisCommand>(model);
                command.kd_cb = command.kd_cb.Trim();
                command.kd_cob = command.kd_cob.Trim();
                command.kd_scob = command.kd_scob.Trim();
                command.kd_thn = command.kd_thn.Trim();
                command.no_pol = command.no_pol.Trim();
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }
    }
}