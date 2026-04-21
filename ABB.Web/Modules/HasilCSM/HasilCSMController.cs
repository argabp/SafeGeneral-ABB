using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.HasilCSM.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.HasilCSM.Models;
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

        public JsonResult GetKodeMetode()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAA", Value = "1" },
                new DropdownOptionDto() { Text = "GMM", Value = "2" }
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
                new Pengukuran() { Text = "Subsequent", Value = "S" },
                new Pengukuran() { Text = "Liability", Value = "L" }
            };

            return Json(result);
        }

        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Detil", Value = "D" },
                new DropdownOptionDto() { Text = "Rekap", Value = "R" }
            };

            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GetViewSourceDataHasil([FromBody] HasilCSMViewModel model)
        {
            if (model.KodeMetode == "1" && model.Pengukuran == "I" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetReleaseIntialPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Pengukuran == "I" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetReleaseIntialPAARekapQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "I" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetReleaseIntialGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "I" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetReleaseIntialGMMRekapQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Pengukuran == "S" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetReleaseSubsequentPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Pengukuran == "S" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetReleaseSubsequentPAARekapQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "S" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetReleaseSubsequentGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "S" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetReleaseSubsequentGMMRekapQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Pengukuran == "L" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetLiabilityPAAQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "1" && model.Pengukuran == "L" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetLiabilityPAARekapQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "L" && model.JenisLaporan == "D")
            {
                var command = Mapper.Map<GetLiabilityGMMQuery>(model);
                var ds = await Mediator.Send(command);
                return Json(JsonConvert.DeserializeObject(ds));
            }
            
            if (model.KodeMetode == "2" && model.Pengukuran == "L" && model.JenisLaporan == "R")
            {
                var command = Mapper.Map<GetLiabilityGMMRekapQuery>(model);
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