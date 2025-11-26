using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.UpdateSettledKlaims.Commands;
using ABB.Application.UpdateSettledKlaims.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.UpdateSettledKlaim
{
    public class UpdateSettledKlaimController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetUpdateSettledKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetUpdateSettledKlaimQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
            });


            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                data.nomor_register_klaim = data.kd_cb.Trim() + "." + data.kd_cob.Trim() +
                                      data.kd_scob.Trim() + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();

                counter++;
            }

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] List<UpdateSettledKlaimModel> model)
        {
            try
            {
                var command = new UpdateSettledKlaimCommand()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    Data = model
                };

                await Mediator.Send(command);

                return Ok(new { Status = "OK"});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}