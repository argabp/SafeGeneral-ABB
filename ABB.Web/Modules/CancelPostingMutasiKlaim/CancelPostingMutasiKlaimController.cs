using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CancelPostingMutasiKlaims.Commands;
using ABB.Application.CancelPostingMutasiKlaims.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingMutasiKlaim
{
    public class CancelPostingMutasiKlaimController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingMutasiKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetCancelPostingMutasiKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
            });


            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                data.nomor_klaim = data.kd_cb.Trim() + "." + data.kd_cob.Trim() + data.kd_scob.Trim() 
                                   + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim() + "." + data.no_mts + "." + data.no_dla;

                counter++;
            }

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingMutasiKlaimModel> model)
        {
            try
            {
                var command = new CancelPostingMutasiKlaimCommand()
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
        
        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<CancelPostingMutasiKlaimModel> model)
        {
            try
            {
                var command = new PostingAccountingCommand()
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