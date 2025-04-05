using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.NomorRegistrasiPolis.Commands;
using ABB.Application.NomorRegistrasiPolis.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NomorRegistrasiPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NomorRegistrasiPolis
{
    public class NomorRegistrasiPolisController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt)
        {
            var command = new GetNomorRegistrasiPolisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt
            };
            
            var data = await Mediator.Send(command);

            data.kd_cb = data.kd_cb.Trim();
            data.kd_cob = data.kd_cob.Trim();
            data.kd_scob = data.kd_scob.Trim();
            
            return PartialView(Mapper.Map<NomorRegistrasiPolisViewModel>(data));
        }
        
        public async Task<ActionResult> GetNomorRegistrasiPolis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAllNomorRegistrasiPolisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNomorRegistrasiPolis([FromBody] NomorRegistrasiPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNomorRegistrasiPolisCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Nota"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
        
        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var result = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
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