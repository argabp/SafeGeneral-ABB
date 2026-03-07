using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.PenyelesaianKlaimReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PenyelesaianKlaimReasuransi.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PenyelesaianKlaimReasuransi
{
    public class PenyelesaianKlaimReasuransiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public PenyelesaianKlaimReasuransiController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View(new PenyelesaianKlaimReasuransiViewModel()
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

        public async Task<JsonResult> GetCOB()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetCobPSTQuery());

            dropdownData.AddRange(result);
            return Json(dropdownData);
        }

        public async Task<JsonResult> GetJenisLaporan()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "Rincian",
                    Value = "P"
                },
                new DropdownOptionDto()
                {
                    Text = "Rekap",
                    Value = "R"
                }
            };
            return Json(dropdownData);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] PenyelesaianKlaimReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetPenyelesaianKlaimReasuransiQuery>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("PenyelesaianKlaimReasuransi.pdf", reportTemplate, sessionId, Orientation.Landscape,
                    0, 0, 10, 10);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}