using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.KasBanks.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.ListVoucher.Models;
using ABB.Web.Modules.ListVoucherKas.Models;
using ABB.Application.VoucherKass.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using MediatR;
using System.Drawing.Printing;
using ABB.Application.Cabangs.Queries;

namespace ABB.Web.Modules.ListVoucher
{
    public class ListVoucherController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IMediator _mediator;

        // Inject Mediator dan Report Generator Service
        public ListVoucherController(IMediator mediator, IReportGeneratorService reportGeneratorService)
        {
            _mediator = mediator;
            _reportGeneratorService = reportGeneratorService;
        }
        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            // --- INI BARU BENAR PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie(); 
            // ---------------------------------------------

            ViewBag.DatabaseName = Request.Cookies["DatabaseName"]; 
            var databaseName = Request.Cookies["DatabaseValue"]; 

            // Cek PS10 (Anti Gagal: potong spasi & huruf besar semua)
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            
            ViewBag.IsPusat = isPusat;

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var userCabang = cabangList.FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));

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
            var kodeCabangCookie = GetCleanCabangCookie();

            // Cek PS10 (Anti Gagal)
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // Filter cabang sesuai cookie user
            var filtered = result
               .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); // <-- WAJIB untuk ComboBox

                
            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> GetKasBankList(string tipe, string kodeCabangDropdown) // <--- Tambah parameter ini
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = GetCleanCabangCookie();
            
            // LOGIKA PENTING:
            // Kalau dropdown dikirim (saat PS10 pilih cabang), pakai cabang dropdown.
            // Kalau kosong, pakai cabang bawaan cookie.
            var targetCabang = !string.IsNullOrEmpty(kodeCabangDropdown) ? kodeCabangDropdown : kodeCabangCookie;

            var result = await _mediator.Send(new GetAllKasBankQuery
            {
                TipeKasBank = tipe,
                SearchKeyword = null
            });

            var dataFormatted = result
                .Where(x => x.KodeCabang == targetCabang) // <--- Filter pakai targetCabang, bukan cookie lagi
                .Select(x => new 
                {
                    Kode = x.Kode,
                    Keterangan = x.Keterangan,
                    TampilanLengkap = $"{x.Kode} - {x.Keterangan}" 
                });

            return Json(dataFormatted);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] ListVoucherFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;
                var kodeCabangCookie = GetCleanCabangCookie();

                // Convert string ke DateTime
                if (!DateTime.TryParse(model.tglAwal, out DateTime tglAwal))
                    throw new Exception("Tanggal Awal tidak valid");
                if (!DateTime.TryParse(model.tglAkhir, out DateTime tglAkhir))
                    throw new Exception("Tanggal Akhir tidak valid");

                IRequest<string> query;

                if (model.tipe == "KAS")
                {
                    query = new GetVoucherKasByTanggalRangeQuery
                    {
                        DatabaseName = databaseName,
                        TanggalAwal = tglAwal,
                        TanggalAkhir = tglAkhir,
                        UserLogin = user,
                        KodeKas = model.kodeKas,
                        KodeCabang = kodeCabangCookie,
                        // KodeCabang = model.KodeCabang,
                        
                        // Masukkan Keterangan yang dikirim dari JS ke Query
                        KeteranganKas = model.keteranganKas
                    };
                }
                else if (model.tipe == "BANK")
                {
                    query = new GetVoucherBankByTanggalRangeQuery
                    {
                        DatabaseName = databaseName,
                        KodeBank = model.kodeBank,
                        // KodeCabang = model.KodeCabang,
                        // Masukkan Keterangan yang dikirim dari JS ke Query
                        KeteranganBank = model.keterangan, 
                        
                        TanggalAwal = tglAwal,
                        TanggalAkhir = tglAkhir,
                        KodeCabang = kodeCabangCookie,
                        UserLogin = user
                    };
                }
                else
                {
                    throw new Exception("Tipe voucher tidak valid");
                }

                // Kirim ke handler untuk dapatkan HTML
                var reportTemplate = await _mediator.Send(query);

                if (string.IsNullOrWhiteSpace(reportTemplate))
                    throw new Exception("Data tidak ditemukan.");

                // Generate file PDF
                _reportGeneratorService.GenerateReport(
                    "ListVoucher.pdf",
                    reportTemplate,
                    user,
                    Orientation.Landscape, // ✅ perbaiki typo
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

    public class ListVoucherFilterDto
    {
        public string tipe { get; set; }
        // public string KodeCabang { get; set; }

        public string kodeBank { get; set; }
        public string keterangan { get; set; }

        public string kodeKas { get; set; }
        public string keteranganKas { get; set; }

        public string tglAwal { get; set; }
        public string tglAkhir { get; set; }
    }
}
