using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.ListingSpreadingOfRisks.Commands;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ListingSpreadingOfRisk.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ListingSpreadingOfRisk
{
    public class ListingSpreadingOfRiskController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public ListingSpreadingOfRiskController(IReportGeneratorService reportGeneratorService)
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

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
             
            return Json(cobs);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] ListingSpreadingOfRiskViewModel model)
        {
            try
            {
                var command = Mapper.Map<ListingSpreadingOfRiskCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("ListingSpreadingOfRisk.pdf", reportTemplate,
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