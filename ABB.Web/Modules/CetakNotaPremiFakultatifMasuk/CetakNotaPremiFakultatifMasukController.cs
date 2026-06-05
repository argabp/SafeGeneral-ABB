using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifMasuks.Commands;
using ABB.Application.CetakNotaPremiFakultatifMasuks.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaPremiFakultatifMasuk.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaPremiFakultatifMasuk
{
    public class CetakNotaPremiFakultatifMasukController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaPremiFakultatifMasukController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            ViewBag.JenisLaporan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Nota Debet/Kredit", Value = "W" }
            };
            
            ViewBag.MataUang = await Mediator.Send(new GetMataUangPSTQuery());

            return View();
        }
        
        public async Task<ActionResult> GetCetakNotaPremiFakultatifMasuks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCetakNotaPremiFakultatifMasuksQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaPremiFakultatifMasukViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakNotaPremiFakultatifMasukCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaPremiFakultatifMasuk.pdf", reportTemplate, sessionId,
                    right: 0, left: 0, bottom: 0, top: 0);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}