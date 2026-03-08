using System;
using System.Threading.Tasks;
using ABB.Application.CetakPLAReasuransis.Queries;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakPLAReasuransi.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakPLAReasuransi
{
    public class CetakPLAReasuransiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakPLAReasuransiController(IReportGeneratorService reportGeneratorService)
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
        
        
        public async Task<ActionResult> GetCetakPLAReasuransi(GridRequest grid)
        {
            var result = await Mediator.Send(new GetPLAReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakPLAReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakPLAReasuransiQuery>(model);

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakPLAReasuransi.pdf", reportTemplate, sessionId,
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