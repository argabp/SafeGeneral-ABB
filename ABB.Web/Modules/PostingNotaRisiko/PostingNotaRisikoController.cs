using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.HasilCSM.Queries;
using ABB.Application.PostingNotaRisiko.Commands;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PostingNotaRisiko.Models;
using ABB.Web.Modules.ProsesCSM.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingNotaRisiko
{
    public class PostingNotaRisikoController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new PostingNotaRisikoViewModel();
            
            return View(model);
        }

        public JsonResult GetTipeTransaksi()
        {
            var result = new List<TipeTransaksi>()
            {
                new TipeTransaksi() { Text = "A1", Value = "A1" },
                new TipeTransaksi() { Text = "A2", Value = "A2" },
                new TipeTransaksi() { Text = "A3", Value = "A3" },
                new TipeTransaksi() { Text = "B1", Value = "B1" },
                new TipeTransaksi() { Text = "B2", Value = "B2" },
                new TipeTransaksi() { Text = "C1", Value = "C1" },
                new TipeTransaksi() { Text = "C2", Value = "C2" },
                new TipeTransaksi() { Text = "K1", Value = "K1" },
                new TipeTransaksi() { Text = "K2", Value = "K2" },
                new TipeTransaksi() { Text = "K3", Value = "K3" },
                new TipeTransaksi() { Text = "K4", Value = "K4" },
                new TipeTransaksi() { Text = "K5", Value = "K5" }
            };

            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Posting([FromBody] PostingNotaRisikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<PostingNotaRisikoCommand> (model);
                await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = "Success Posting Data" });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
        
        public async Task<JsonResult> GetPeriodeProses()
        {
            var periodeProcess = await Mediator.Send(new GetPeriodeProsesQuery());
            
            return Json(periodeProcess);
        }
    }
}