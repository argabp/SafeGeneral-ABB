using System.Threading.Tasks;
using ABB.Application.InforceInwardTreatyNonProporsional.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.InforceOutwardTreatyNonProporsional.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.InforceOutwardTreatyNonProporsional
{
    public class InforceOutwardTreatyNonProporsionalController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new InforceOutwardTreatyNonProporsionalModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetTreatyNonProporsional([FromBody] InforceOutwardTreatyNonProporsionalModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetTreatyNonProporsionalQuery()
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