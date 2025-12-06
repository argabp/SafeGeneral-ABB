using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.LaporanKerugianSementara.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanKerugianSementara.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LaporanKerugianSementara
{
    public class LaporanKerugianSementaraController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanKerugianSementaraController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Laporan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Cetak LKS", Value = "B" }
            };
            
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
        public async Task<ActionResult> GetLaporanKerugian([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLaporanKerugianQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            
            var tipe_mutasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PLA", Value = "P" },
                new DropdownOptionDto() { Text = "DLA", Value = "D" },
                new DropdownOptionDto() { Text = "Beban", Value = "B" },
                new DropdownOptionDto() { Text = "Recovery", Value = "R" }
            };
            
            foreach (var data in ds)
            {
                data.nomor_berkas = "K." + data.kd_cb.Trim() + "." +
                                    data.kd_scob.Trim() + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                data.nm_tipe_mts = tipe_mutasi.FirstOrDefault(w => w.Value == data.tipe_mts)?.Text;
            }

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] LaporanKerugianSementaraViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetLaporanKerugianSementaraQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);

                _reportGeneratorService.GenerateReport("LaporanKerugianSementara.pdf", reportTemplate, sessionId);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}