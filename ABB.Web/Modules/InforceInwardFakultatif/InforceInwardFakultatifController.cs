using System.Threading.Tasks;
using ABB.Application.InforceInwardFakultatif.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.InforceInwardFakultatif.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.InforceInwardFakultatif
{
    public class InforceInwardFakultatifController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new InforceInwardFakultatifModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetFakultatif([FromBody] InforceInwardFakultatifModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetFakultatifQuery()
                {
                    tgl_akhir = model.TanggalAkhir
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