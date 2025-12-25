using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.JurnalMemorials104.Commands;
using ABB.Application.CancelPostingJurnalMemorial104.Commands;
using ABB.Application.JurnalMemorials104.Queries;
using ABB.Application.CancelPostingJurnalMemorial104.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingJurnalMemorial104
{
    public class CancelPostingJurnalMemorial104Controller : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        [HttpPost]
            public async Task<ActionResult> GetCancelPostingJurnalMemorial104(
                [DataSourceRequest] DataSourceRequest request,
                string searchKeyword)
            {
                var userCabang = Request.Cookies["UserCabang"];

                var ds = await Mediator.Send(new GetAllCancelJurnalMemorial104Query
                {
                    SearchKeyword = searchKeyword,
                    KodeCabang = userCabang,
                    FlagGL = true
                });

                return Json(ds.ToDataSourceResult(request));
            }
        
        public async Task<ActionResult> CancelPosting([FromBody] List<JurnalMemorial104Dto> model)
        {
           try
            {
                if (model == null || !model.Any())
                {
                    return Ok(new { Status = "ERROR", Message = "Tidak ada data yang dipilih." });
                }

                // Ambil User ID
                string userId = CurrentUser.UserId ?? "SYSTEM";
                if (userId.Length > 25) userId = userId.Substring(0, 25);

                var command = new CancelPostingJurnalMemorial104Command()
                {
                    KodeCabang = Request.Cookies["UserCabang"],
                    KodeUserUpdate = userId,
                    NoVouchers = model.Select(m => m.NoVoucher).ToList()
                };

                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok(new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}