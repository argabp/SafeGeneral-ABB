using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Models;
using ABB.Application.PostingNotaKlaimReasuransis.Commands;
using ABB.Application.PostingNotaKlaimReasuransis.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingNotaKlaimReasuransi
{
    public class PostingNotaKlaimReasuransiController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetPostingNotaKlaimReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetPostingNotaKlaimReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<PostingNotaKlaimReasuransiModel> model)
        {
            try
            {
                var command = new PostingNotaKlaimReasuransiCommand()
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