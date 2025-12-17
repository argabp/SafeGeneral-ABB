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

        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        [HttpGet]
            public async Task<IActionResult> GetKasBankList(string tipe)
            {
                var databaseName = Request.Cookies["DatabaseValue"];

                var result = await _mediator.Send(new GetAllKasBankQuery
                {
                    TipeKasBank = tipe,
                    SearchKeyword = null
                });

                // --- PERBAIKAN DISINI ---
                // Kita format datanya sebelum dikirim ke JSON
                var dataFormatted = result.Select(x => new 
                {
                    Kode = x.Kode,
                    Keterangan = x.Keterangan,
                    // Gabungkan Kode dan Keterangan jadi satu string
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
                        UserLogin = user
                    };
                }
                else if (model.tipe == "BANK")
                {
                    query = new GetVoucherBankByTanggalRangeQuery
                    {
                        DatabaseName = databaseName,
                        KodeBank = model.kodeBank,
                        
                        // Masukkan Keterangan yang dikirim dari JS ke Query
                        KeteranganBank = model.keterangan, 
                        
                        TanggalAwal = tglAwal,
                        TanggalAkhir = tglAkhir,
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
        public string tipe { get; set; }       // "KAS" atau "BANK"
        public string kodeBank { get; set; }   // Hanya untuk BANK
        public string keterangan { get; set; }
        public string tglAwal { get; set; }    // Format "yyyy-MM-dd" atau sesuai input
        public string tglAkhir { get; set; }
    }
}
