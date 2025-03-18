using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.EntriNotas.Commands;
using ABB.Application.EntriNotas.Queries;
using ABB.Application.MataUangs.Commands;
using ABB.Application.MataUangs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriNota.Models;
using ABB.Web.Modules.MataUang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EntriNota
{
    public class EntriNotaController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetEntriNotas([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetEntriNotasQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailNotas([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string jns_tr, string jns_nt_msk, string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, 
            string no_nt_kel)
        {
            if (string.IsNullOrWhiteSpace(kd_cb) || string.IsNullOrWhiteSpace(jns_tr) || string.IsNullOrWhiteSpace(jns_nt_msk) ||
                string.IsNullOrWhiteSpace(kd_thn) || string.IsNullOrWhiteSpace(kd_bln) || string.IsNullOrWhiteSpace(no_nt_msk) ||
                string.IsNullOrWhiteSpace(jns_nt_kel) || string.IsNullOrWhiteSpace(no_nt_kel))
                return Ok();
            
            var ds = await Mediator.Send(new GetDetailNotasQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddDetailNota([FromBody] DetailNotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddDetailNotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Detail Nota"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetailNota([FromBody] DetailNotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditDetailNotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Detail Nota"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailNota([FromBody] DetailNotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailNotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Nota"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}