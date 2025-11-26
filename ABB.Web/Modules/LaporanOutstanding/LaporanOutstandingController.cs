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



        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var databaseName = Request.Cookies["DatabaseValue"]; 
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

            // Filter cabang sesuai cookie user
            var filtered = result
                .Where(c => c.kd_cb?.Trim() == kodeCabangCookie.Trim())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); // <-- WAJIB untuk ComboBox

            // kirim ke View kalau ingin dipakai
            ViewBag.UserCabang = kodeCabangCookie;

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

        [HttpGet]
            public async Task<IActionResult> CetakLaporan(
                string KodeCabang,
                string JenisAwal,
                string JenisAkhir,
                string BulanAwal,
                string BulanAkhir,
                string Tahun)
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanOutstandingQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = KodeCabang,
                    JenisAwal = JenisAwal,
                    JenisAkhir = JenisAkhir,
                    BulanAwal = BulanAwal,
                    BulanAkhir = BulanAkhir,
                    Tahun = Tahun,
                    UserLogin = user
                };

                var data = await Mediator.Send(query);

                if (data == null || !data.Any())
                    return Content("<h3 style='text-align:center;'>Data tidak ditemukan</h3>", "text/html");

                return View("CetakLaporan", data);
            }

            [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanOutstandingFilterDto model)
        {
            try
            {
                // var command = Mapper.Map<GetLaporanOutstandingQuery>(model);
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanOutstandingQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    JenisAwal = model.JenisAwal,
                    JenisAkhir = model.JenisAkhir,
                    BulanAwal = model.BulanAwal,
                    BulanAkhir = model.BulanAkhir,
                    Tahun = model.Tahun,
                    UserLogin = user
                };

                var data = await Mediator.Send(query);
                if (data == null || !data.Any())
                    throw new Exception("Data tidak ditemukan.");

                var userId = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("User ID tidak ditemukan. Tidak dapat menyimpan laporan.");                
                
                // string htmlContent = "<p>Ganti dengan hasil render View 'CetakLaporan' menjadi string HTML</p>";
           
                var reportTemplate = await Mediator.Send(query);
                // Generate file PDF
                _reportGeneratorService.GenerateReport(
                    "LaporanOutstanding.pdf",
                    reportTemplate, // <-- Tipe data sekarang 'string', sesuai harapan GenerateReport
                    userId,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                   PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = userId });
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
        public string JenisAwal { get; set; }
        public string JenisAkhir { get; set; }
        public string BulanAwal { get; set; }
        public string BulanAkhir { get; set; }
        public string Tahun { get; set; }
    }
}