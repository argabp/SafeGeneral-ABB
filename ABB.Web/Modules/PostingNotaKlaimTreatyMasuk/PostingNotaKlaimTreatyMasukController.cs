using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;
using ABB.Application.PostingNotaKlaimTreatyMasuks.Commands;
using ABB.Application.PostingNotaKlaimTreatyMasuks.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingNotaKlaimTreatyMasuk
{
    public class PostingNotaKlaimTreatyMasukController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetPostingNotaKlaimTreaties(GridRequest grid)
        {
            var result = await Mediator.Send(new GetPostingNotaKlaimTreatyMasuksQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<PostingNotaKlaimTreatyMasukModel> model)
        {
            try
            {
                var command = new PostingNotaKlaimTreatyMasukCommand()
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