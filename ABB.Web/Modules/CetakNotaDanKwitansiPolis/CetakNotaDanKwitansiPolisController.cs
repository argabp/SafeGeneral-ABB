using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.CetakNotaDanKwitansiPolis.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.CetakNotaDanKwitansiPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CetakNotaDanKwitansiPolis
{
    public class CetakNotaDanKwitansiPolisController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public CetakNotaDanKwitansiPolisController(IReportGeneratorService reportGeneratorService)
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
                new DropdownOptionDto() { Text = "Nota Debet/Kredit", Value = "1" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit (Share HP)", Value = "W" },
                new DropdownOptionDto() { Text = "Slip Komisi", Value = "2" },
                new DropdownOptionDto() { Text = "Kwitansi", Value = "3" },
                new DropdownOptionDto() { Text = "Kwitansi (English)", Value = "7" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit (English)", Value = "8" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Preprinted", Value = "C" },
                new DropdownOptionDto() { Text = "Kwitansi Preprinted", Value = "E" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Angsuran", Value = "A" },
                new DropdownOptionDto() { Text = "Kwitansi Angsuran", Value = "B" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Angsuran Preprinted", Value = "K" },
                new DropdownOptionDto() { Text = "Kwitansi Angsuran Preprinted", Value = "L" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Non Diskon", Value = "M" },
                new DropdownOptionDto() { Text = "Kwitansi Non Diskon", Value = "N" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Non Diskon Preprinted", Value = "O" },
                new DropdownOptionDto() { Text = "Kwitansi Non Diskon Preprinted", Value = "P" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Original", Value = "X" },
                new DropdownOptionDto() { Text = "Nota Debet/Kredit Copy", Value = "Y" }
            };
            
            ViewBag.MataUang = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return View();
        }
        
        public async Task<ActionResult> GetAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAkseptasisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] CetakNotaDanKwitansiPolisViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetCetakNotaDanKwitansiPolisQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("CetakNotaDanKwitansiPolis.pdf", reportTemplate, sessionId, right: 0, left: 0, top: 0, bottom: 0);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}