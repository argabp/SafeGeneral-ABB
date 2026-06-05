using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakRekapitulasiProduksiPremiFakultatifKeluars.Commands;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakRekapitulasiProduksiPremiFakultatifKeluar.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakRekapitulasiProduksiPremiFakultatifKeluar
{
    public class CetakRekapitulasiProduksiPremiFakultatifKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakRekapitulasiProduksiPremiFakultatifKeluarController(IReportGeneratorService reportGeneratorService)
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
        public async Task<ActionResult> GenerateReport([FromBody] CetakRekapitulasiProduksiPremiFakultatifKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakRekapitulasiProduksiPremiFakultatifKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("CetakRekapitulasiProduksiPremiFakultatifKeluar.pdf", reportTemplate, sessionId, Orientation.Landscape,
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
                new DropdownOptionDto() { Text = "Per COB", Value = "1" }
            };

            return Json(result);
        }
    }
}