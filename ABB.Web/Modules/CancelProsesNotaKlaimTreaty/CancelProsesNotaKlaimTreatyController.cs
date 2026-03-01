using System;
using System.Threading.Tasks;
using ABB.Application.CancelProsesNotaKlaimTreaties.Commands;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CancelProsesNotaKlaimTreaty.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelProsesNotaKlaimTreaty
{
    public class CancelProsesNotaKlaimTreatyController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new CancelProsesNotaKlaimTreatyViewModel()
            {
                tgl_proses = DateTime.Now
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] CancelProsesNotaKlaimTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<CancelProsesNotaKlaimTreatyCommand> (model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}