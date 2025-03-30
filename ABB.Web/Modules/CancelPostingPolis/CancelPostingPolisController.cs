using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKomisiTambahans.Commands;
using ABB.Application.CancelPostingNotaKomisiTambahans.Queries;
using ABB.Application.CancelPostingPolis.Commands;
using ABB.Application.CancelPostingPolis.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingPolis
{
    public class CancelPostingPolisController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];


            return View();
        }

        public async Task<ActionResult> GetCancelPolicies([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetCancelPostingPolisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingPolisDto> model)
        {
            try
            {
                var command = new CancelPostingPolisCommand()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    Data = model
                };

                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok(new
                    { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}