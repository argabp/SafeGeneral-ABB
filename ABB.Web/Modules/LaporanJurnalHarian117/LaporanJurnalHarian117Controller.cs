using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanJurnalHarian117s117.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Application.JenisTransaksis.Queries;


namespace ABB.Web.Modules.LaporanJurnalHarian117
{
    public class LaporanJurnalHarian117Controller : AuthorizedBaseController
    {

         private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanJurnalHarian117Controller(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public async Task<IActionResult> Index()
        {
           
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });

           
            var userCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
           
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;

           
            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;
            return View();
        }

         [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            var filtered = result
                .Where(c => c.kd_cb?.Trim() == kodeCabangCookie.Trim())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); 
            ViewBag.UserCabang = kodeCabangCookie;

            return Json(filtered);
        }

         [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanJurnalHarian117FilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan.");

                // Sesuaikan QUERY dengan data yang datang dari JS
                var query = new GetLaporanJurnalHarian117Query
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    JenisTransaksi = model.JenisTransaksi,
                    UserLogin = user
                };

                var reportTemplate = await Mediator.Send(query);

                if (reportTemplate == null || !reportTemplate.Any())
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanJurnalHarian117.pdf",
                    reportTemplate,
                    user,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                    PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpGet]
            public async Task<IActionResult> GetJenisTransaksi()
        {
            var list = await Mediator.Send(new GetAllJenisTransaksiQuery());

            var result = list.Select(x => new
            {
                NamaJenisTransaksi = $"{x.nama.Trim()} ({x.kode.Trim()})",
                KodeJenisTransaksi = x.kode.Trim()
            }).ToList();

            return Json(result);
        }
    }

      public class LaporanJurnalHarian117FilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string JenisTransaksi { get; set; }
    }
}