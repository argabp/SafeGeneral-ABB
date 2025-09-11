using System;
using System.Threading.Tasks;
using ABB.Application.ProfilRisikoHartaBendas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProfilRisikoHartaBenda.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.ProfilRisikoHartaBenda
{
    public class ProfilRisikoHartaBendaController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var model = new ProfilRisikoHartaBendaViewModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetProfilRisikoHartaBenda([FromBody] ProfilRisikoHartaBendaViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetProfilRisikoHartaBendasQuery()
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