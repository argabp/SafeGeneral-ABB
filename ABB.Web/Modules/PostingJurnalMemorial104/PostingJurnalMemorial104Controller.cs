using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.JurnalMemorials104.Commands;
using ABB.Application.PostingJurnalMemorial104.Commands;
using ABB.Application.JurnalMemorials104.Queries;
using ABB.Application.PostingJurnalMemorial104.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingJurnalMemorial104
{
    public class PostingJurnalMemorial104Controller : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
         [HttpPost] 
        public async Task<ActionResult> GetPostingJurnalMemorial104([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
           
             var kodeCabang = Request.Cookies["UserCabang"];
            var ds = await Mediator.Send(new GetAllJurnalMemorial104ByFlagQuery
            {
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                DatabaseName = Request.Cookies["DatabaseName"]
            });
            // -------------------------

            return Json(ds.ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> Posting([FromBody] List<JurnalMemorial104Dto> model)
        {
            try
            {
                
                var command = new PostingJurnalMemorial104Command()
                {
                    Data = model.Select(m => m.NoVoucher).ToList(),
                      UserId = CurrentUser.UserId 
                };
                // -------------------------

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