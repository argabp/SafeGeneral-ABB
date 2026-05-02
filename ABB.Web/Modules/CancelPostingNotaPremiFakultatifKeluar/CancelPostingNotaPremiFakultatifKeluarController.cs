using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Commands;
using ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingNotaPremiFakultatifKeluar
{
    public class CancelPostingNotaPremiFakultatifKeluarController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingNotaPremiFakultatifKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCancelPostingNotaPremiFakultatifKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingNotaPremiFakultatifKeluarModel> model)
        {
            try
            {
                var command = new CancelPostingNotaPremiFakultatifKeluarCommand()
                {
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
        public async Task<ActionResult> PostingAccounting([FromBody] List<AccountingPostingNotaPremiFakultatifKeluarModel> model)
        {
            try
            {
                var command = new AccountingPostingNotaPremiFakultatifKeluarCommand()
                {
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