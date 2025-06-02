using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.PostingKomisiTambahans.Commands;
using ABB.Application.PostingKomisiTambahans.Queries;
using ABB.Application.PostingPolicies.Commands;
using ABB.Application.PostingPolicies.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingKomisiTambahan
{
    public class PostingKomisiTambahanController : AuthorizedBaseController
    {

        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<ActionResult> GetPostingPolicies([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetPostingPolisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<PostingKomisiTambahanDto> model)
        {
            try
            {
                var command = new PostingKomisiTambahanCommand()
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