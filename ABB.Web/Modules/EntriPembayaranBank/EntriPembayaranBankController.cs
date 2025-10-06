using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.EntriPembayaranBanks.Commands;
using ABB.Application.EntriPembayaranBanks.Queries;
using ABB.Application.VoucherBanks.Queries;
using ABB.Web.Modules.Base;
using ABB.Application.MataUangs.Queries;
using ABB.Web.Modules.EntriPembayaranBank.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ABB.Application.KasBanks.Queries;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Application.MataUangs.Queries;


namespace ABB.Web.Modules.EntriPembayaranBank
{
    public class EntriPembayaranBankController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetEntriPembayaranBank([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllVoucherBankQuery() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add(string noVoucher)
        {
             var databaseName = Request.Cookies["DatabaseName"];
            var voucherDto = await Mediator.Send(new GetVoucherBankByIdQuery { NoVoucher = noVoucher });
            if (voucherDto == null) return NotFound();

            ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }
            };

            // kode akun
            var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{int.Parse(x.NoPerkiraan):N0} - {x.Keterangan}"
                }).ToList();

            ViewBag.DebetKreditOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Kredit", Value = "K" },
                new SelectListItem { Text = "Debit", Value = "D" }
                
            };
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
            {
                Value = x.kd_mtu.Trim(),
                Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
            }).ToList();
            
            var viewModel = new EntriPembayaranBankViewModel
            {
                VoucherHeader = voucherDto,
                NoVoucher = noVoucher // Langsung set di properti utama
            };
            
            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailPembayaran([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetAllEntriPembayaranBankQuery { NoVoucher = noVoucher });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // Action 'Save' sekarang menerima ViewModel utama
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriPembayaranBankViewModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (model.No > 0) // kalau ada No → update
            {
                var command = Mapper.Map<UpdatePembayaranBankCommand>(model);
                await Mediator.Send(command);
            }
            else // kalau No = 0 atau null → create baru
            {
                var command = Mapper.Map<CreatePembayaranBankCommand>(model);
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

        // Nota Produksi 
        public IActionResult PilihNota()
        {
            return PartialView("PilihNota");
        }

        // Action untuk mengisi data ke grid _PilihNota
        [HttpPost]
        public async Task<IActionResult> GetNotaProduksi([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // Anda perlu membuat GetNotaProduksiQuery di Application Layer
            var data = await Mediator.Send(new InquiryNotaProduksiQuery{ SearchKeyword = searchKeyword });
            return Json(data.ToDataSourceResult(request));
        }

        // delete
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeletePembayaranBankCommand command)
        {
            // Cek sederhana jika parameter tidak valid
            if (command == null || command.No <= 0 || string.IsNullOrEmpty(command.NoVoucher))
            {
                return Json(new { success = false, message = "Parameter tidak valid." });
            }

            // Langsung kirim command yang sudah ter-binding
            await Mediator.Send(command);

            return Json(new { success = true });
        }

        [HttpGet]
            public async Task<IActionResult> Cetak(string noVoucher)
            {
                if (string.IsNullOrEmpty(noVoucher))
                    return BadRequest("Nomor voucher tidak boleh kosong.");

                var voucher = await Mediator.Send(new GetVoucherBankByIdQuery { NoVoucher = noVoucher });
                if (voucher == null)
                    return NotFound($"Voucher dengan nomor {noVoucher} tidak ditemukan.");

                var details = await Mediator.Send(new GetAllEntriPembayaranBankQuery { NoVoucher = noVoucher });

                var viewModel = new EntriPembayaranBankViewModel
                {
                     VoucherHeader = voucher,
                };

                return View(viewModel); // View Razor khusus untuk cetak
            }

             [HttpGet]
            public async Task<IActionResult> GetKurs(string kodeMataUang, DateTime tanggalVoucher)
            {
                var databaseName = Request.Cookies["DatabaseName"];
                var kurs = await Mediator.Send(new GetKursMataUangQuery
                {
                    DatabaseName = databaseName,
                    KodeMataUang = kodeMataUang,
                    TanggalVoucher = tanggalVoucher
                });
                return Json(new { nilai_kurs = kurs });
            }

    }
}