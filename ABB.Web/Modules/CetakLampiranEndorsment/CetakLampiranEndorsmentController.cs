using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CetakLampiranEndorsments.Queries;
using ABB.Application.CetakNotaDanKwitansiPolis.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakLampiranEndorsment.Models;
using ABB.Web.Modules.CetakNotaDanKwitansiPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakLampiranEndorsment
{
    public class CetakLampiranEndorsmentController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakLampiranEndorsmentController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetCetakLampiranEndorsments([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetCetakLampiranEndorsmentsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakLampiranEndorsmentViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakLampiranEndorsmentQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakLampiranEndorsment.pdf", reportTemplate, sessionId, 
                    right: 15, left: 15, top: 15, bottom: 15);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}