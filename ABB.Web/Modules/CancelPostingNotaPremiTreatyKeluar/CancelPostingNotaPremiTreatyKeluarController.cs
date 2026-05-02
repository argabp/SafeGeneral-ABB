using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiTreatyKeluars.Commands;
using ABB.Application.CancelPostingNotaPremiTreatyKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingNotaPremiTreatyKeluar
{
    public class CancelPostingNotaPremiTreatyKeluarController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingNotaPremiTreatyKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCancelPostingNotaPremiTreatyKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingNotaPremiTreatyKeluarModel> model)
        {
            try
            {
                var command = new CancelPostingNotaPremiTreatyKeluarCommand()
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