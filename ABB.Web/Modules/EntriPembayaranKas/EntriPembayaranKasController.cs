using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.EntriPembayaranKass.Commands;
using ABB.Application.VoucherKass.Queries;
using ABB.Application.EntriPembayaranKass.Queries;
using ABB.Application.MataUangs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriPembayaranKas.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Application.MataUangs.Queries;



namespace ABB.Web.Modules.EntriPembayaranKas
{
    public class EntriPembayaranKasController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetEntriPembayaranKas([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllVoucherKasQuery() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailPembayaran([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetAllEntriPembayaranKasQuery { NoVoucher = noVoucher });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        //  [HttpGet]
        // public async Task<IActionResult> GetDetailPembayaran([DataSourceRequest] DataSourceRequest request, string noVoucher)
        // {
        //     if (string.IsNullOrEmpty(noVoucher))
        //         return Json(new DataSourceResult { Data = Enumerable.Empty<object>(), Total = 0 });

        //     var query = _context.EntriPembayaranKas
        //         .Where(x => x.NoVoucher == noVoucher)
        //         .OrderBy(x => x.No) // urut sesuai nomor
        //         .Select(x => new EntriPembayaranKasDto
        //         {
        //             No = x.No,
        //             FlagPembayaran = x.FlagPembayaran,
        //             JenisSumberNora = x.JenisSumberNora,
        //             NoNota4 = x.NoNota4,
        //             TglBayar = x.TglBayar,
        //             TotalBayar = x.TotalBayar
        //         });

        //     var result = await query.ToDataSourceResultAsync(request);
        //     return Json(result);
        // }



        public async Task<IActionResult> Add(string noVoucher)
        {

            var databaseName = Request.Cookies["DatabaseName"];

            var voucherDto = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = noVoucher });
            if (voucherDto == null) return NotFound();

            // u/ flagpembayaran
            ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }
                

            };

            // u/ debetkredit
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

            var viewModel = new EntriPembayaranKasViewModel
            {
                VoucherKasHeader = voucherDto,
                NoVoucher = noVoucher // Langsung set di properti utama
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriPembayaranKasViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.No > 0) // kalau ada No → update
            {
                var command = Mapper.Map<UpdatePembayaranKasCommand>(model);
                await Mediator.Send(command);
            }
            else // kalau No = 0 atau null → create baru
            {
                var command = Mapper.Map<CreatePembayaranKasCommand>(model);
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }


        // Action untuk menghapus data
        [HttpPost]
        public async Task<IActionResult> DeleteDetail(string NoVoucher, int No)
        {
            if (No <= 0 || string.IsNullOrEmpty(NoVoucher))
            {
                return Json(new { success = false, message = "Parameter tidak valid." });
            }

            await Mediator.Send(new DeletePembayaranKasCommand
            {
                No = No,               // Id detail yang mau dihapus
                NoVoucher = NoVoucher  // Nomor voucher induknya
            });

            return Json(new { success = true });
        }
        // Nota Produksi 
        public IActionResult PilihNota()
        {
            return PartialView("PilihNota");
        }

        // Action untuk mengisi data ke grid _PilihNota
        [HttpPost]
        public async Task<IActionResult> GetNotaProduksi([DataSourceRequest] DataSourceRequest request)
        {
            // Anda perlu membuat GetNotaProduksiQuery di Application Layer
            var data = await Mediator.Send(new InquiryNotaProduksiQuery());
            return Json(data.ToDataSourceResult(request));
        }  

        [HttpGet]
            public async Task<IActionResult> Cetak(string noVoucher)
            {
                if (string.IsNullOrEmpty(noVoucher))
                    return BadRequest("Nomor voucher tidak boleh kosong.");

                var voucher = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = noVoucher });
                if (voucher == null)
                    return NotFound($"Voucher dengan nomor {noVoucher} tidak ditemukan.");

                var details = await Mediator.Send(new GetAllEntriPembayaranKasQuery { NoVoucher = noVoucher });

                var viewModel = new EntriPembayaranKasViewModel
                {
                     VoucherKasHeader = voucher,
                     Details = details.ToList() 
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