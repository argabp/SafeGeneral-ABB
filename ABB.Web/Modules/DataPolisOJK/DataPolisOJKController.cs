using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.DataPolisOJKs.Queries;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DataPolisOJK.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.DataPolisOJK
{
    public class DataPolisOJKController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            ViewBag.KodeCabang = Request.Cookies["UserCabang"].Trim();
            
            var model = new DataPolisOJKViewModel();
            
            return View(model);
        }

        public async Task<JsonResult> GetCabang()
        {
            var ds = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds);
        }

        [HttpPost]
        public async Task<ActionResult> GetDataPolisOJK([FromBody] DataPolisOJKViewModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetDataPolisOJKQueries()
                {
                    kd_cb = model.KodeCabang,
                    jenis_laporan = model.JenisLaporan,
                    tgl_akh = model.TanggalAkhir,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch
            {
                return Json(new { Error = "Connection Timeout" });
            }
        }
        
        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Posisi Pertanggungan", Value = "1" },
                new DropdownOptionDto() { Text = "Mutasi Pertanggungan", Value = "2" }
            };

            return Json(result);
        }
    }
}