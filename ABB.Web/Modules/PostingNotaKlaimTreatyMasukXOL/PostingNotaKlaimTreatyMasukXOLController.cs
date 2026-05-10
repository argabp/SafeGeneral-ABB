using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;
using ABB.Application.PostingNotaKlaimTreatyMasukXOLs.Commands;
using ABB.Application.PostingNotaKlaimTreatyMasukXOLs.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingNotaKlaimTreatyMasukXOL
{
    public class PostingNotaKlaimTreatyMasukXOLController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetPostingNotaKlaimTreatyMasukXOLs(GridRequest grid)
        {
            var result = await Mediator.Send(new GetPostingNotaKlaimTreatyMasukXOLsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<PostingNotaKlaimTreatyMasukXOLModel> model)
        {
            try
            {
                var command = new PostingNotaKlaimTreatyMasukXOLCommand()
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