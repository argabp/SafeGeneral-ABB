using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.CetakDLAReasuransis.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakDLAReasuransi.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakDLAReasuransi
{
    public class CetakDLAReasuransiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakDLAReasuransiController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.JenisLaporan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Gabungan", Value = "1" },
                new DropdownOptionDto() { Text = "Per Mutasi", Value = "2" }
            };

            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        
        public async Task<ActionResult> GetCetakDLAReasuransi(GridRequest grid)
        {
            var result = await Mediator.Send(new GetDLAReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakDLAReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakDLAReasuransiQuery>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakDLAReasuransi.pdf", reportTemplate, sessionId,
                    Orientation.Portrait, 10, 10, 10, 10);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}