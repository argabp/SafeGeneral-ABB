using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakSOATreatyKeluars.Commands;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakSOATreatyKeluar.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakSOATreatyKeluar
{
    public class CetakSOATreatyKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakSOATreatyKeluarController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }

        public async Task<JsonResult> GetCabang()
        {
            var dropdownData = new List<DropdownOptionDto>();
            var result = await Mediator.Send(new GetCabangPSTQuery());
            dropdownData.AddRange(result);
            return Json(dropdownData);
        }
        
        public JsonResult GetKuartalTransaksi()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "I (Pertama)", Value = "1" },
                new DropdownOptionDto() { Text = "II (Kedua)", Value = "2" },
                new DropdownOptionDto() { Text = "III (Ketiga)", Value = "3" },
                new DropdownOptionDto() { Text = "IV (Keempat)", Value = "4" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetKodePersh()
        {
            var dropdownOptions = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };
            
            return Json(dropdownOptions);
        }
        
        public async Task<JsonResult> GetJenisLaporan()
        {
            var dropdownOptions = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Gabungan", Value = "1" },
                new DropdownOptionDto() { Text = "Per U/Y", Value = "2" }
            };
            
            return Json(dropdownOptions);
        }
        
        public async Task<JsonResult> GetKodeRekananPersh(string kd_grp_pas, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp_pas,
                kd_cb = kd_cb
            });

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakSOATreatyKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakSOATreatyKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakSOATreatyKeluar.pdf", reportTemplate,
                    sessionId, Orientation.Landscape,
                    5, 5, 5, 5, PaperKind.Legal);

                return Ok(new { Status = "OK", Data = sessionId });
            }
            catch (Exception e)
            {
                return Ok(new
                    { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}