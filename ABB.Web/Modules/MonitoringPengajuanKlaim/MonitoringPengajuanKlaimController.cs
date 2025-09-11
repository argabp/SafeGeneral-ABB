using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.MonitoringPengajuanKlaims.Queries;
using ABB.Application.OutstandingKlaimProduksiUmum.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.MonitoringPengajuanKlaim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.MonitoringPengajuanKlaim
{
    public class MonitoringPengajuanKlaimController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new MonitoringPengajuanKlaimViewModel();
            
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
        public async Task<ActionResult> GetMonitoringPengajuanKlaim([FromBody] MonitoringPengajuanKlaimViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetMonitoringPengajuanKlaimQuery()
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