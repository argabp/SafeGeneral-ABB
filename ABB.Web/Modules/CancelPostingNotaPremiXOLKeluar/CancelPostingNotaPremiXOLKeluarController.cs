using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiXOLKeluars.Commands;
using ABB.Application.CancelPostingNotaPremiXOLKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingNotaPremiXOLKeluar
{
    public class CancelPostingNotaPremiXOLKeluarController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingNotaPremiXOLKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCancelPostingNotaPremiXOLKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingNotaPremiXOLKeluarModel> model)
        {
            try
            {
                var command = new CancelPostingNotaPremiXOLKeluarCommand()
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
        public async Task<ActionResult> PostingAccounting([FromBody] List<AccountingPostingNotaPremiXOLKeluarModel> model)
        {
            try
            {
                var command = new AccountingPostingNotaPremiXOLKeluarCommand()
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