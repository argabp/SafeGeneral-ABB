using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.HasilCSM.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.HasilCSM.Models;
using ABB.Web.Modules.ProsesCSM.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.HasilCSM
{
    public class HasilCSMController : AuthorizedBaseController
    {
        
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new HasilCSMViewModel();
            
            return View(model);
        }

        public JsonResult GetTipeTransaksi()
        {
            var result = new List<TipeTransaksi>()
            {
                new TipeTransaksi() { Text = "A1", Value = "A1" },
                new TipeTransaksi() { Text = "A2", Value = "A2" },
                new TipeTransaksi() { Text = "A3", Value = "A3" },
                new TipeTransaksi() { Text = "B1", Value = "B1" },
                new TipeTransaksi() { Text = "B2", Value = "B2" },
                new TipeTransaksi() { Text = "C1", Value = "C1" },
                new TipeTransaksi() { Text = "C2", Value = "C2" },
                new TipeTransaksi() { Text = "K1", Value = "K1" },
                new TipeTransaksi() { Text = "K2", Value = "K2" },
                new TipeTransaksi() { Text = "K3", Value = "K3" },
                new TipeTransaksi() { Text = "K4", Value = "K4" },
                new TipeTransaksi() { Text = "K5", Value = "K5" }
            };

            return Json(result);
        }

        public JsonResult GetKodeMetode()
        {
            var result = new List<KodeMetode>()
            {
                new KodeMetode() { Text = "PAA", Value = "1" },
                new KodeMetode() { Text = "GMM", Value = "2" }
            };

            return Json(result);
        }

        public JsonResult GetPerhitungan()
        {
            var result = new List<Perhitungan>()
            {
                new Perhitungan() { Text = "Cadangan", Value = "C" },
                new Perhitungan() { Text = "Pendapatan", Value = "P" }
            };

            return Json(result);
        }

        public JsonResult GetPengukuran()
        {
            var result = new List<Pengukuran>()
            {
                new Pengukuran() { Text = "Initial", Value = "I" },
                new Pengukuran() { Text = "Subsequent", Value = "S" }
            };

            return Json(result);
        }
        
        // public async Task<ActionResult> GetViewSourceData([DataSourceRequest] DataSourceRequest request, HasilCSMViewModel model)
        // {
        //     var command = Mapper.Map<GetViewSourceDataHasilQuery>(model);
        //     var ds = await Mediator.Send(command);
        //     return Json(ds.AsQueryable().ToDataSourceResult(request));
        // }
        
        [HttpPost]
        public async Task<ActionResult> GetViewSourceDataHasil([FromBody] HasilCSMViewModel model)
        {

            if (model.KodeMetode == "1" && model.Perhitungan == "C")
            {
                var command = Mapper.Map<GetLiabilityPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            } 
            
            if (model.KodeMetode == "2" && model.Perhitungan == "C")
            {
                var command = Mapper.Map<GetLiabilityGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Perhitungan == "P" && model.Pengukuran == "I")
            {
                var command = Mapper.Map<GetReleaseIntialPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Perhitungan == "P" && model.Pengukuran == "S")
            {
                var command = Mapper.Map<GetReleaseSubsequentPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Perhitungan == "P" && model.Pengukuran == "I")
            {
                var command = Mapper.Map<GetReleaseIntialGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Perhitungan == "P" && model.Pengukuran == "S")
            {
                var command = Mapper.Map<GetReleaseSubsequentGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }

            return Ok();
        }
        
        public async Task<JsonResult> GetPeriodeProses()
        {
            var periodeProcess = await Mediator.Send(new GetPeriodeProsesQuery());
            
            return Json(periodeProcess);
        }
    }
}