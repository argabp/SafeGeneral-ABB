using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.RekapitulasiProduksi.Quries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RekapitulasiProduksi.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RekapitulasiProduksi
{
    public class RekapitulasiProduksiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public RekapitulasiProduksiController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View(new RekapitulasiProduksiViewModel()
            {
                kd_cb = Request.Cookies["UserCabang"]?.Trim() ?? string.Empty
            });
        }

        public async Task<JsonResult> GetCabang()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            dropdownData.AddRange(result);
            return Json(dropdownData);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] RekapitulasiProduksiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetRekapitulasiProduksiQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");

                command.kd_rk_sb_bis = string.Empty;
                command.kd_grp_sb_bis = string.Empty;
                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("RekapitulasiProduksi.pdf", reportTemplate, sessionId, Orientation.Landscape);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Summary Cabang Polis", Value = "7" },
                new DropdownOptionDto() { Text = "Per Group Cabang Polis", Value = "8" }
            };

            return Json(result);
        }
    }
}