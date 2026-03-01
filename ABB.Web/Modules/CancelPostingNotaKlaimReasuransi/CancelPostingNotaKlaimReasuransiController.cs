using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKlaimReasuransis.Commands;
using ABB.Application.CancelPostingNotaKlaimReasuransis.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingNotaKlaimReasuransi
{
    public class CancelPostingNotaKlaimReasuransiController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCancelPostingNotaKlaimReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCancelPostingNotaKlaimReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> Cancel([FromBody] List<CancelPostingNotaKlaimReasuransiModel> model)
        {
            try
            {
                var command = new CancelPostingNotaKlaimReasuransiCommand()
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
        public async Task<ActionResult> Posting([FromBody] List<CancelPostingNotaKlaimReasuransiModel> model)
        {
            try
            {
                var command = new PostingAccountingNotaKlaimReasuransis()
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