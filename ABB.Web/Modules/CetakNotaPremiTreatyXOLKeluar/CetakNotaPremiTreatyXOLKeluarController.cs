using System;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiTreatyXOLKeluars.Commands;
using ABB.Application.CetakNotaPremiTreatyXOLKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaPremiTreatyXOLKeluar.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaPremiTreatyXOLKeluar
{
    public class CetakNotaPremiTreatyXOLKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaPremiTreatyXOLKeluarController(IReportGeneratorService reportGeneratorService)
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
        
        public async Task<ActionResult> GetCetakNotaPremiTreatyXOLKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCetakNotaPremiTreatyXOLKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaPremiTreatyXOLKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakNotaPremiTreatyXOLKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaPremiTreatyXOLKeluar.pdf", reportTemplate, sessionId, Orientation.Landscape,
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