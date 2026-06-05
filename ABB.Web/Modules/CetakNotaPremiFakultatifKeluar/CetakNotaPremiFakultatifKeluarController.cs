using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifKeluars.Commands;
using ABB.Application.CetakNotaPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaPremiFakultatifKeluar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaPremiFakultatifKeluar
{
    public class CetakNotaPremiFakultatifKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaPremiFakultatifKeluarController(IReportGeneratorService reportGeneratorService)
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
                new DropdownOptionDto() { Text = "Non Angsuran", Value = "1" },
                new DropdownOptionDto() { Text = "Angsuran", Value = "2" }
            };

            return View();
        }
        
        public async Task<ActionResult> GetCetakNotaPremiFakultatifKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCetakNotaPremiFakultatifKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaPremiFakultatifKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakNotaPremiFakultatifKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaPremiFakultatifKeluar.pdf", reportTemplate, sessionId,
                    right: 10, left: 10, bottom: 10, top: 10);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}