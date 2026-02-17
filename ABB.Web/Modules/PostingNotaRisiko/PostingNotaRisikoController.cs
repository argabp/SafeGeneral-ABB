using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
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
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "A1", Value = "A1" },
                new DropdownOptionDto() { Text = "A2", Value = "A2" },
                new DropdownOptionDto() { Text = "A3", Value = "A3" },
                new DropdownOptionDto() { Text = "B1", Value = "B1" },
                new DropdownOptionDto() { Text = "B2", Value = "B2" },
                new DropdownOptionDto() { Text = "C1", Value = "C1" },
                new DropdownOptionDto() { Text = "C2", Value = "C2" },
                new DropdownOptionDto() { Text = "K1", Value = "K1" },
                new DropdownOptionDto() { Text = "K2", Value = "K2" },
                new DropdownOptionDto() { Text = "K3", Value = "K3" },
                new DropdownOptionDto() { Text = "K4", Value = "K4" },
                new DropdownOptionDto() { Text = "K5", Value = "K5" }
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