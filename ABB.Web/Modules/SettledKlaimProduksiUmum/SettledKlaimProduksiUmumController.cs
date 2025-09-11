using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.OutstandingKlaimProduksiUmum.Queries;
using ABB.Application.SettledKlaimProduksiUmum.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.SettledKlaimProduksiUmum.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.SettledKlaimProduksiUmum
{
    public class SettledKlaimProduksiUmumController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new SettledKlaimProduksiUmumModel();
            
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
        public async Task<ActionResult> GetSettledKlaimProduksiUmum([FromBody] SettledKlaimProduksiUmumModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetSettledKlaimQuery()
                {
                    kd_cb = model.KodeCabang,
                    tgl_awal = model.TanggalAwal,
                    tgl_akhir = model.TanggalAkhir
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