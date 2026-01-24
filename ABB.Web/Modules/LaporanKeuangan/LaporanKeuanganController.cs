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
                string reportTemplate = "";
                string outputFileName = "";

                // PERCABANGAN LOGIKA BERDASARKAN TIPE LAPORAN
                switch (model.TipeLaporan)
                {
                    case "NERACA":
                        var queryNeraca = new GetLaporanNeracaQuery
                        {
                            JenisPeriode = model.JenisPeriode,
                            Bulan = model.Bulan,
                            Tahun = model.Tahun
                        };
                        reportTemplate = await Mediator.Send(queryNeraca);
                        outputFileName = "LaporanNeraca.pdf";
                        break;

                    case "LABARUGI":
                        // Kamu harus buat Class Query baru: GetLaporanLabaRugiQuery
                        var queryLR = new GetLaporanLabaRugiQuery 
                        {
                            JenisPeriode = model.JenisPeriode,
                            Bulan = model.Bulan,
                            Tahun = model.Tahun
                        };
                        reportTemplate = await Mediator.Send(queryLR);
                        outputFileName = "LaporanLabaRugi.pdf";
                        break;

                    case "ARUSKAS":
                        // Kamu harus buat Class Query baru: GetLaporanArusKasQuery
                        var queryAK = new GetLaporanArusKasQuery
                        {
                            JenisPeriode = model.JenisPeriode,
                            Bulan = model.Bulan,
                            Tahun = model.Tahun
                        };
                        reportTemplate = await Mediator.Send(queryAK);
                        outputFileName = "LaporanArusKas.pdf";
                        break;

                    default:
                        throw new Exception("Tipe Laporan tidak valid atau belum tersedia.");
                }

                // VALIDASI HASIL
                if (string.IsNullOrEmpty(reportTemplate))
                    throw new Exception("Data tidak ditemukan atau Template kosong.");

                // GENERATE PDF (Nama File Dinamis sesuai switch case diatas)
                _reportGeneratorService.GenerateReport(
                    outputFileName, 
                    reportTemplate,
                    user,
                    Orientation.Portrait,
                    5, 5, 5, 5,
                    PaperKind.A4
                );

                return Ok(new { Status = "OK", Data = user, Filename = outputFileName });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

    
    }

    // UPDATE DTO AGAR SESUAI DENGAN KIRIMAN JAVASCRIPT
    public class LaporanKeuanganFilterDto
    {
        public string TipeLaporan { get; set; }
        public string JenisPeriode { get; set; } // Baru
        public int Bulan { get; set; }           // Baru (int)
        public int Tahun { get; set; }           // Baru (int)
    }
}