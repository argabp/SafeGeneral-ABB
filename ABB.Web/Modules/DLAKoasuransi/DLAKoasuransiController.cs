using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.DLAKoasuransi.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DLAKoasuransi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DLAKoasuransi
{
    public class DLAKoasuransiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public DLAKoasuransiController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Bahasa = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Indonesia", Value = "INA" },
                new DropdownOptionDto() { Text = "English", Value = "ENG" }
            };
            
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];

            return View();
        }
        
        public async Task<ActionResult> GetDLAKoasuransi([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKoasuransiQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] DLAKoasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetDLAKoasuransiQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("DLAKoasuransi.pdf", reportTemplate, sessionId);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}