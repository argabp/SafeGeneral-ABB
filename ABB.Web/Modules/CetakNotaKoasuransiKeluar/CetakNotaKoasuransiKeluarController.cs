using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKoasuransiKeluars.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaKoasuransiKeluar.Models;
using DinkToPdf;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaKoasuransiKeluar
{
    public class CetakNotaKoasuransiKeluarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaKoasuransiKeluarController(IReportGeneratorService reportGeneratorService)
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
        
        public async Task<ActionResult> GetCetakNotaKoasuransiKeluars([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetCetakNotaKoasuransiKeluarsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaKoasuransiKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakNotaKoasuransiKeluarQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaKoasuransiKeluar.pdf", reportTemplate, sessionId,
                    Orientation.Portrait, 5, 5, 5, 5);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}