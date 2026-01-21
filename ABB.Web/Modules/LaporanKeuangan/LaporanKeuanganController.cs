using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Application.LaporanKeuangan.Queries;

namespace ABB.Web.Modules.LaporanKeuangan
{
    public class LaporanKeuanganController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        // Constructor Bersih (Hanya IReportGeneratorService)
        public LaporanKeuanganController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanKeuanganFilterDto model)
        {
            try
            {
                var user = CurrentUser.UserId;

                if (!DateTime.TryParse(model.PerTanggal, out DateTime tanggalParsed))
                    throw new Exception("Format tanggal tidak valid.");

                var query = new GetLaporanNeracaQuery
                {
                    PerTanggal = tanggalParsed
                };

                // SEKARANG INI ISINYA SUDAH STRING HTML (Bukan List lagi)
                string reportTemplate = await Mediator.Send(query);

                if (string.IsNullOrEmpty(reportTemplate))
                    throw new Exception("Data tidak ditemukan.");

                // Langsung kirim ke Generator (Sama kayak Jurnal Harian)
                _reportGeneratorService.GenerateReport(
                    "LaporanNeraca.pdf",
                    reportTemplate,    // String HTML
                    user,
                    Orientation.Portrait,
                    5, 5, 5, 5,
                    PaperKind.A4
                );

                return Ok(new { Status = "OK", Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }

    public class LaporanKeuanganFilterDto
    {
        public string TipeLaporan { get; set; }
        public string PerTanggal { get; set; }
    }
}