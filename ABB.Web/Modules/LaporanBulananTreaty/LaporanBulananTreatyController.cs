using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.LaporanBulananTreatys.Commands;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanBulananTreaty.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LaporanBulananTreaty
{
    public class LaporanBulananTreatyController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanBulananTreatyController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            ViewBag.KodeCabang = "PS10";

            return View();
        }

        public async Task<JsonResult> GetCabang()
        {
            var dropdownData = new List<DropdownOptionDto>();
            var result = await Mediator.Send(new GetCabangPSTQuery());
            dropdownData.AddRange(result);
            return Json(dropdownData);
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
             
            return Json(cobs);
        }
        
        public async Task<JsonResult> GetJenisLaporan()
        {
            var dropdownOptions = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Treaty Masuk", Value = "2" },
                new DropdownOptionDto() { Text = "Treaty Keluar", Value = "1" }
            };
            
            return Json(dropdownOptions);
        }
        
        public async Task<JsonResult> GetKodePersh()
        {
            var dropdownOptions = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
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
        public async Task<ActionResult> GenerateReport([FromBody] LaporanBulananTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<LaporanBulananTreatyCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("LaporanBulananTreaty.pdf", reportTemplate,
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