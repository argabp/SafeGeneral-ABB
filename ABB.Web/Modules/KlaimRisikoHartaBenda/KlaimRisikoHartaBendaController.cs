using System;
using System.Threading.Tasks;
using ABB.Application.KlaimRisikoHartaBendas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KlaimRisikoHartaBenda.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.KlaimRisikoHartaBenda
{
    public class KlaimRisikoHartaBendaController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var model = new KlaimRisikoHartaBendaViewModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetKlaimRisikoHartaBenda([FromBody] KlaimRisikoHartaBendaViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetKlaimRisikoHartaBendasQuery()
                {
                    thn_uw = model.ThnUW
                });
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}