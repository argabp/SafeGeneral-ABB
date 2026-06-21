using System;
using System.Threading.Tasks;
using ABB.Application.Common.Queries;
using ABB.Application.PolisInfoceProduksiUmum.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PolisInfoceProduksiUmum.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.PolisInfoceProduksiUmum
{
    public class PolisInfoceProduksiUmumController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new PolisInfoceProduksiUmumModel()
            {
                kd_cb = Request.Cookies["UserCabang"].Trim()
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetPolisInfoceProduksiUmum([FromBody] PolisInfoceProduksiUmumModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetPolisInfoceQuery()
                {
                    tgl_akhir = model.TanggalAkhir,
                    kd_cb = model.kd_cb,
                    DatabaseName =  Request.Cookies["DatabaseValue"]
                });
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName =  Request.Cookies["DatabaseValue"]
            });
            return Json(result);
        }
    }
}