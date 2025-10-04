using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKomisiTambahans.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaKomisiTambahan.Models;
using DinkToPdf;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaKomisiTambahan
{
    public class CetakNotaKomisiTambahanController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaKomisiTambahanController(IReportGeneratorService reportGeneratorService)
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
        
        public async Task<ActionResult> GetCetakNotaKomisiTambahans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetCetakNotaKomisiTambahansQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaKomisiTambahanViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakNotaKomisiTambahanQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                command.jns_lap = "1";

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaKomisiTambahan.pdf", reportTemplate, sessionId,
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