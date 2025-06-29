using System.Threading.Tasks;
using ABB.Application.KlaimRisikoKendaraans.Queries;
using ABB.Application.ProfilRisikoHartaBendas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KlaimRisikoKendaraan.Models;
using ABB.Web.Modules.ProfilRisikoHartaBenda.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.KlaimRisikoKendaraan
{
    public class KlaimRisikoKendaraanController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var model = new KlaimRisikoKendaraanViewModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetKlaimRisikoKendaraan([FromBody] KlaimRisikoKendaraanViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetKlaimRisikoKendaraansQuery()
                {
                    thn_uw = model.ThnUW
                });
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch
            {
                return Json(new { Error = "Connection Timeout" });
            }
        }
    }
}