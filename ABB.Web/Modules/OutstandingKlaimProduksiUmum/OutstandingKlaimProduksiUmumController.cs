using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.OutstandingKlaimProduksiUmum.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.OutstandingKlaimProduksiUmum.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.OutstandingKlaimProduksiUmum
{
    public class OutstandingKlaimProduksiUmumController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new OutstandingKlaimProduksiUmumModel();
            
            return View(model);
        }

        public async Task<JsonResult> GetCabang()
        {
            var kodeCabangs = new List<CabangDto>();
            
            kodeCabangs.Add(new CabangDto()
            {
                kd_cb = string.Empty,
                nm_cb = string.Empty
            });
            
            var ds = await Mediator.Send(new GetCabangQuery());
            kodeCabangs.AddRange(ds);
            return Json(kodeCabangs);
        }

        [HttpPost]
        public async Task<ActionResult> GetOutstandingKlaimProduksiUmum([FromBody] OutstandingKlaimProduksiUmumModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetOutstandingKlaimQuery()
                {
                    kd_cb = model.KodeCabang,
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