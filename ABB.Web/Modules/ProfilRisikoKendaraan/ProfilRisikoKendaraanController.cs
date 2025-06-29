using System.Threading.Tasks;
using ABB.Application.ProfilRisikoKendaraans.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProfilRisikoKendaraan.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.ProfilRisikoKendaraan
{
    public class ProfilRisikoKendaraanController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var model = new ProfilRisikoKendaraanViewModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetProfilRisikoKendaraan([FromBody] ProfilRisikoKendaraanViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetProfilRisikoKendaraansQuery()
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