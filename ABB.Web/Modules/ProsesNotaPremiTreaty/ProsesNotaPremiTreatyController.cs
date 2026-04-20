using System;
using System.Threading.Tasks;
using ABB.Application.Common.Queries;
using ABB.Application.ProsesNotaPremiTreaties.Commands;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesNotaPremiTreaty.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesNotaPremiTreaty
{
    public class ProsesNotaPremiTreatyController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new ProsesNotaPremiTreatyViewModel()
            {
                tgl_proses = DateTime.Now
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Proses([FromBody] ProsesNotaPremiTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesNotaPremiTreatyCommand> (model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
            
            return Json(cobs);
        }
    }
}