using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CetakSchedulePolis.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Web.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakSchedulePolis.Models;
using DinkToPdf;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakSchedulePolis
{
    public class CetakSchedulePolisController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakSchedulePolisController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            ViewBag.JenisLaporan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Single", Value = "S" },
                new DropdownOptionDto() { Text = "Multi", Value = "M" },
                new DropdownOptionDto() { Text = "Lampiran", Value = "L" },
                new DropdownOptionDto() { Text = "Sertifikat", Value = "T" },
                new DropdownOptionDto() { Text = "Polis Kendaraan Leasing", Value = "P" },
            };
            ViewBag.Bahasa = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Indonesia", Value = "INA" },
                new DropdownOptionDto() { Text = "English", Value = "ENG" }
            };
            ViewBag.JenisLampiran = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "-", Value = "A" },
                new DropdownOptionDto() { Text = "Unconditional", Value = "U" },
                new DropdownOptionDto() { Text = "Kendaraan ( Listing )", Value = "K" },
                new DropdownOptionDto() { Text = "Kendaraan & Nama QQ( Listing )", Value = "Q" },
                new DropdownOptionDto() { Text = "Obyek / Aksesoris", Value = "O" },
                new DropdownOptionDto() { Text = "Kendaraan ( Detail )", Value = "D" },
                new DropdownOptionDto() { Text = "Kendaraan (Daftar Isi - Sertifikat)", Value = "I" },
                new DropdownOptionDto() { Text = "Daftar Isi", Value = "R" },
                new DropdownOptionDto() { Text = "Pre-printed (Sertifikat Kendaraan)", Value = "P" },
                new DropdownOptionDto() { Text = "Premi Tahunan", Value = "S" },
                new DropdownOptionDto() { Text = "Pisah Halaman (2 Lbr)", Value = "H" },
                new DropdownOptionDto() { Text = "Conditional", Value = "C" },
                new DropdownOptionDto() { Text = "Pisah Halaman Blank Form (2 Lbr)", Value = "B" },
                new DropdownOptionDto() { Text = "CO - Inward", Value = "Z" },
                new DropdownOptionDto() { Text = "Multi Currency", Value = "F" },
                new DropdownOptionDto() { Text = "Original", Value = "X" },
                new DropdownOptionDto() { Text = "Copy", Value = "Y" },
            };
            
            return View();
        }
        
        public async Task<ActionResult> GetAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAkseptasisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakSchedulePolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakSchedulePolisQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var result = await Mediator.Send(command);

                var reportOrientation = Orientation.Portrait;
                var right = 20;
                var left = 20;

                if (model.jenisLaporan == "L" && model.jenisLampiran != "D")
                {
                    reportOrientation = Orientation.Landscape;
                    right = 0;
                    left = 0;
                }

                if (model.jenisLaporan == "M" && model.jenisLampiran == "A")
                {
                    right = 10;
                    left = 10;
                }

                if (model.jenisLampiran == "D")
                {
                    right = 10;
                    left = 10;
                }

                var reportName = result.Item1 + ".pdf";
                _reportGeneratorService.GenerateReport(reportName, 
                    result.Item2, sessionId, reportOrientation, right, left);

                return Ok(new { Status = "OK", Data = sessionId, ReportName = reportName});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}