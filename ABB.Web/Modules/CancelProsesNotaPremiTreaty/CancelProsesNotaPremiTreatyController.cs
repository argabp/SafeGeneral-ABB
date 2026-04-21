using System;
using System.Threading.Tasks;
using ABB.Application.CancelProsesNotaPremiTreaties.Commands;
using ABB.Application.Common.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CancelProsesNotaPremiTreaty.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelProsesNotaPremiTreaty
{
    public class CancelProsesNotaPremiTreatyController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new CancelProsesNotaPremiTreatyViewModel()
            {
                tgl_proses = DateTime.Now
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] CancelProsesNotaPremiTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<CancelProsesNotaPremiTreatyCommand> (model);
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