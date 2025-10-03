using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.LaporanBulananCabang.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanBulananCabang.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LaporanBulananCabang
{
    public class LaporanBulananCabangController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanBulananCabangController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View(new LaporanBulananCabangViewModel()
            {
                kd_cb = Request.Cookies["UserCabang"]?.Trim() ?? string.Empty
            });
        }

        public async Task<JsonResult> GetCabang()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            dropdownData.AddRange(result);
            return Json(dropdownData);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] LaporanBulananCabangViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetLaporanBulananCabangQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("LaporanBulananCabang.pdf", reportTemplate, sessionId, Orientation.Landscape,
                    5, 5, 5, 5, PaperKind.Legal);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Premi Bruto", Value = "1" },
                new DropdownOptionDto() { Text = "Premi Netto", Value = "2" },
                new DropdownOptionDto() { Text = "Discount + Komisi", Value = "3" },
                new DropdownOptionDto() { Text = "Klaim", Value = "4" },
                new DropdownOptionDto() { Text = "Premi Rate", Value = "5" }
            };

            return Json(result);
        }
    }
}