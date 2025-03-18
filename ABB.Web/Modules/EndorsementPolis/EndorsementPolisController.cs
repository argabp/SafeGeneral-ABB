using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.EndorsementPolis.Commands;
using ABB.Application.EndorsementPolis.Queries;
using ABB.Application.ReopenPolis.Commands;
using ABB.Application.ReopenPolis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EndorsementPolis.Models;
using ABB.Web.Modules.ReopenPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EndorsementPolis
{
    public class EndorsementPolisController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetEndorsementPolis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetEndorsementPolisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> EndorsementPolisNormal([FromBody] EndorsementPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesEndorsementNormalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> EndorsementPolisCancel([FromBody] EndorsementPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesEndorsementCancelCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> EndorsementPolisNoPremium([FromBody] EndorsementPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesEndorsementNoPremiumCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
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