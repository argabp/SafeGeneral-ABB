using System;
using System.Threading.Tasks;
using ABB.Application.CetakSlipPremiFakultatifKeluars.Commands;
using ABB.Application.CetakSlipPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakSlipPremiFakultatifKeluar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakSlipPremiFakultatifKeluar
{
    public class CetakSlipPremiFakultatifKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakSlipPremiFakultatifKeluarController(IReportGeneratorService reportGeneratorService)
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
        
        public async Task<ActionResult> GetCetakSlipPremiFakultatifKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetCetakSlipPremiFakultatifKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakSlipPremiFakultatifKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<CetakSlipPremiFakultatifKeluarCommand>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakSlipPremiFakultatifKeluar.pdf", reportTemplate, sessionId,
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