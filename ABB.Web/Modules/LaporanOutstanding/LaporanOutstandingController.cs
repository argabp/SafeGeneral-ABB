using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanOutstandings.Queries;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ABB.Web.Modules.LaporanOutstanding
{
    public class LaporanOutstandingController : AuthorizedBaseController
    {

        private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanOutstandingController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
            // _viewRenderService = viewRenderService; // Injeksi
        }

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

       public async Task<IActionResult> Index()
        {
            var databaseName = Request.Cookies["DatabaseValue"]; 
            
            // --- 2. GUNAKAN FUNGSI PEMBERSIH DI SINI ---
            var kodeCabangCookie = GetCleanCabangCookie();

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(kodeCabangCookie))
            {
                await HttpContext.SignOutAsync("Identity.Application");
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            // --- 3. TAMBAHKAN LOGIKA PS10 DI SINI ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            ViewBag.IsPusat = isPusat;
            // ----------------------------------------

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
            
            // --- 4. GUNAKAN FUNGSI PEMBERSIH DI SINI ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 5. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); 

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // --- 6. FILTER PAKAI isPusat || ---
            var filtered = result
                .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); 

            return Json(filtered);
        }

        [HttpGet]
            public async Task<IActionResult> GetJenisAssetList()
        {
            var list = await Mediator.Send(new GetDistinctJenisAssetQuery());

            var result = list.Select(x => new
            {
                NamaJenisAsset = x,
                KodeJenisAsset = x
            }).ToList();

            return Json(result);
        }



        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanOutstandingFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan.");

                // Sesuaikan QUERY dengan data yang datang dari JS
                var query = new GetLaporanOutstandingQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    TglProduksiAwal = model.TglProduksiAwal,
                    TglProduksiAkhir = model.TglProduksiAkhir,
                    TglPelunasan = model.TglPelunasan,
                    JenisLaporan = model.JenisLaporan,
                    JenisTransaksi = model.JenisTransaksi,
                    SelectedCodes = model.SelectedCodes,
                    UserLogin = user
                };

                var reportTemplate = await Mediator.Send(query);

                if (reportTemplate == null || !reportTemplate.Any())
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanOutstanding.pdf",
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
    }

    public class LaporanOutstandingFilterDto
    {
        public string KodeCabang { get; set; }
        public string TglProduksiAwal { get; set; }
        public string TglProduksiAkhir { get; set; }
        public string TglPelunasan { get; set; }
        public string JenisLaporan { get; set; }
        public string JenisTransaksi { get; set; }
        public List<string> SelectedCodes { get; set; }
    }
}