using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.EntriPenyelesaianPiutangs.Commands;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;
using ABB.Application.VoucherBanks.Queries;
using ABB.Web.Modules.Base;
using ABB.Application.MataUangs.Queries;
using ABB.Web.Modules.EntriPenyelesaianPiutang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.KasBanks.Queries;
using ABB.Application.InquiryNotaProduksis.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
// tambahan cabang
using ABB.Application.Cabangs.Queries;


namespace ABB.Web.Modules.EntriPenyelesaianPiutang
{
    public class EntriPenyelesaianPiutangController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetEntriPenyelesaianPiutang([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllHeaderPenyelesaianUtangQuery() { SearchKeyword = searchKeyword });
            return Json(data.ToDataSourceResult(request));
        }
       

        // --- PERBAIKI ACTION INI (SEKARANG BISA UNTUK ADD & EDIT) ---
        public async Task<IActionResult> Add(string kodeCabang, string nomorBukti)
        {
            var databaseName = Request.Cookies["DatabaseName"];
            var viewModel = new EntriPenyelesaianPiutangViewModel();
              var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            // Cek apakah ini mode Edit (jika nomorBukti diisi)
            if (!string.IsNullOrEmpty(nomorBukti))
            {
                // Anda perlu membuat Query ini
                var dto = await Mediator.Send(new GetHeaderPenyelesaianUtangByIdQuery { KodeCabang = kodeCabang, NomorBukti = nomorBukti });
                if (dto == null) return NotFound();

                viewModel.PenyelesaianHeader = dto;

                // Ambil juga detail pembayarannya
                var detailDto = await Mediator.Send(new GetAllEntriPenyelesaianPiutangQuery { NoBukti = dto.NomorBukti });
                // viewModel.PembayaranItems = ... (mapping dari detailDto jika diperlukan)
            }

            // Siapkan data untuk semua DropDown
            ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }
            };
            var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" });
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.NoPerkiraan,
                Text = $"{x.NoPerkiraan} - {x.Keterangan}"
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
            
             ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
            {
                Value = c.kd_cb.Trim(),
                Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}"
            }).ToList();

            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailPembayaran([DataSourceRequest] DataSourceRequest request, string NoBukti)
        {
            var data = await Mediator.Send(new GetAllEntriPenyelesaianPiutangQuery { NoBukti = NoBukti });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveHeader([FromBody] HeaderPenyelesaianUtangDto model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Anda perlu membuat CreateHeaderPenyelesaianUtangCommand
            var command = Mapper.Map<CreateHeaderPenyelesaianUtangCommand>(model);
           // command.KodeUserInput = CurrentUser.UserId; // Mengisi user yang sedang login

            // 'command' akan mengembalikan NomorBukti yang baru dibuat
            var newNomorBukti = await Mediator.Send(command);

            // Pastikan Anda juga membuat mapping untuk DTO -> Command di ViewModel
            // profile.CreateMap<HeaderPenyelesaianUtangDto, CreateHeaderPenyelesaianUtangCommand>();

            return Json(new { success = true, nomorBukti = newNomorBukti });
        }

        // Action 'Save' sekarang menerima ViewModel utama
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriPenyelesaianPiutangViewModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (model.No > 0) // kalau ada No → update
            {
                var command = Mapper.Map<UpdatePenyelesaianPiutangCommand>(model);
                await Mediator.Send(command);
            }
            else // kalau No = 0 atau null → create baru
            {
                var command = Mapper.Map<CreatePenyelesaianPiutangCommand>(model);
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
        public async Task<IActionResult> DeleteDetail([FromBody] DeletePenyelesaianPiutangCommand command)
        {
            // Cek sederhana jika parameter tidak valid
            if (command == null || command.No <= 0 || string.IsNullOrEmpty(command.NoBukti))
            {
                return Json(new { success = false, message = "Parameter tidak valid." });
            }

            // Langsung kirim command yang sudah ter-binding
            await Mediator.Send(command);

            return Json(new { success = true });
        }


    }
}