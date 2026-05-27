using System;
using System.Threading.Tasks;
using ABB.Application.CetakSlipKomisiFakultatifKeluars.Commands;
using ABB.Application.CetakSlipKomisiFakultatifKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakSlipKomisiFakultatifKeluar.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakSlipKomisiFakultatifKeluar
{
    public class CetakSlipKomisiFakultatifKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakSlipKomisiFakultatifKeluarController(IReportGeneratorService reportGeneratorService)
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
        
        public async Task<ActionResult> GetCetakSlipKomisiFakultatifKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCetakSlipKomisiFakultatifKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakSlipKomisiFakultatifKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakSlipKomisiFakultatifKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                // _reportGeneratorService.GenerateReport("CetakSlipKomisiFakultatifKeluar.pdf", reportTemplate, sessionId, Orientation.Landscape,
                //     right: 10, left: 10, bottom: 10, top: 10);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}