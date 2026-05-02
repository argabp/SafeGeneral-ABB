using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaTreatyMasuks.Commands;
using ABB.Application.CancelPostingNotaTreatyMasuks.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingNotaTreatyMasuk
{
    public class CancelPostingNotaTreatyMasukController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingNotaTreatyMasuks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCancelPostingNotaTreatyMasuksQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingNotaTreatyMasukModel> model)
        {
            try
            {
                var command = new CancelPostingNotaTreatyMasukCommand()
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