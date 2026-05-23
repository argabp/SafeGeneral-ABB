using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakRekapitulasiProduksiTreatyXOLKeluars.Commands;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakRekapitulasiProduksiTreatyXOLKeluar.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiTreatyXOLKeluar
{
    public class CetakRekapitulasiProduksiTreatyXOLKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakRekapitulasiProduksiTreatyXOLKeluarController(IReportGeneratorService reportGeneratorService)
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

        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakRekapitulasiProduksiTreatyXOLKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakRekapitulasiProduksiTreatyXOLKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakRekapitulasiProduksiTreatyXOLKeluar.pdf", reportTemplate,
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

        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Per COB", Value = "1" },
                new DropdownOptionDto() { Text = "Per Ceding", Value = "2" },
                new DropdownOptionDto() { Text = "Per Broker", Value = "3" }
            };

            return Json(result);
        }
    }
}