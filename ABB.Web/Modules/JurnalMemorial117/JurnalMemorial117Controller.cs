using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.JurnalMemorial117.Models; // Pastikan namespace ViewModel benar
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

// Import Command & Query (Sesuaikan namespace Application Anda)
using ABB.Application.JurnalMemorial117.Commands;
using ABB.Application.JurnalMemorial117.Queries;
using ABB.Application.Cabangs.Queries;
using ABB.Application.Coas.Queries;
using ABB.Application.MataUangs.Queries;
using ABB.Application.InquiryNotaProduksis.Queries;

namespace ABB.Web.Modules.JurnalMemorial117
{
    public class JurnalMemorial117Controller : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(kodeCabangCookie))
            {
                await HttpContext.SignOutAsync("Identity.Application");
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        // --- GRID UTAMA (HEADER) ---
        [HttpPost]
        public async Task<ActionResult> GetJurnalMemorial117([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var kodeCabang = Request.Cookies["UserCabang"];
            // Asumsi ada query GetAllJurnalMemorial117Query
            var data = await Mediator.Send(new GetAllJurnalMemorial117Query() 
            { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang
            });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // --- FORM ADD / EDIT ---
        public async Task<IActionResult> Add(string kodeCabang, string noVoucher)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var userCabang = Request.Cookies["UserCabang"];
            var viewModel = new JurnalMemorial117ViewModel(); // Sesuaikan nama ViewModel

            // Load Master Data untuk Dropdown
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            var akunList = await Mediator.Send(new GetAllCoaQuery());

            // MODE ADD (NoVoucher kosong)
            if (string.IsNullOrEmpty(noVoucher))
            {
                viewModel.JurnalHeader = new JurnalMemorial117Dto();
                
                var now = DateTime.Now;
                var namaCabang = cabangList.FirstOrDefault(c => c.kd_cb.Trim() == userCabang?.Trim())?.nm_cb?.Trim();
                
                ViewBag.DisplayCabang = $"{userCabang} - {namaCabang}";

                // Generate No Voucher Baru (Opsional, jika ada logic auto number)
                // var nextNoVoucher = await Mediator.Send(new GetNextNoVoucherJurnalQuery { ... });
                
                viewModel.JurnalHeader.KodeCabang = userCabang;
                viewModel.JurnalHeader.Tanggal = now;
                // viewModel.JurnalHeader.NoVoucher = nextNoVoucher; // Jika auto number
            }
            // MODE EDIT
            else
            {
                var headerDto = await Mediator.Send(new GetJurnalMemorial117ByIdQuery { KodeCabang = kodeCabang, NoVoucher = noVoucher });
                if (headerDto == null) return NotFound();
                
                viewModel.JurnalHeader = headerDto;

                // Load Detail Existing
                var detailListDto = await Mediator.Send(new GetDetailJurnalMemorial117Query { NoVoucher = headerDto.NoVoucher });
                viewModel.JurnalItems = Mapper.Map<List<JurnalMemorial117Item>>(detailListDto);
            }

            // --- ViewBag Dropdowns ---
            ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
            {
                Value = c.kd_cb.Trim(),
                Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}"
            }).ToList();

            ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
            {
                Value = x.kd_mtu.Trim(),
                Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
            }).ToList();

            ViewBag.KodeAkunOptions = akunList.Select(x => new SelectListItem
            {
                Value = x.Kode.Trim(),
                Text = $"{x.Kode.Trim()} - {x.Nama.Trim()}"
            }).ToList();

            return PartialView(viewModel);
        }

        // --- GRID DETAIL (BACA DATA) ---
        [HttpPost]
        public async Task<ActionResult> GetDetailJurnal([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetDetailJurnalMemorial117Query { NoVoucher = noVoucher });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // --- SIMPAN HEADER ---
        [HttpPost]
        public async Task<IActionResult> SaveHeader([FromBody] JurnalMemorial117Dto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingData = await Mediator.Send(new GetJurnalMemorial117ByIdQuery 
            { 
                KodeCabang = model.KodeCabang, 
                NoVoucher = model.NoVoucher 
            });

            string finalNoVoucher;

            if (existingData != null) // UPDATE
            {
                var command = Mapper.Map<UpdateJurnalMemorial117HeaderCommand>(model);
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
                finalNoVoucher = model.NoVoucher;
            }
            else // CREATE
            {
                var command = Mapper.Map<CreateJurnalMemorial117HeaderCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                finalNoVoucher = await Mediator.Send(command);
            }

            return Json(new { success = true, noVoucher = finalNoVoucher });
        }

        // --- SIMPAN DETAIL (ADD/UPDATE SATUAN) ---
        [HttpPost]
        public async Task<IActionResult> SaveDetail([FromBody] JurnalMemorial117ViewModel model)
        {
            // Validasi manual field detail yang wajib
            if (string.IsNullOrEmpty(model.NoVoucher) || string.IsNullOrEmpty(model.KodeAkun))
                return BadRequest("Data tidak lengkap.");

            if (model.No > 0) // UPDATE Detail
            {
                var command = Mapper.Map<UpdateJurnalMemorial117DetailCommand>(model);
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else // CREATE Detail Baru
            {
                var command = Mapper.Map<CreateJurnalMemorial117DetailCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

        // --- DELETE DETAIL ---
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteJurnalMemorial117DetailCommand command)
        {
            if (command == null || command.No <= 0 || string.IsNullOrEmpty(command.NoVoucher))
            {
                return Json(new { success = false, message = "Parameter tidak valid." });
            }

            await Mediator.Send(command);
            return Json(new { success = true });
        }

        // --- DELETE HEADER ---
        [HttpPost]
        public async Task<IActionResult> DeleteHeader([FromBody] DeleteJurnalMemorial117HeaderCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result)
                    return Json(new { success = true, message = "Data berhasil dihapus." });
                else
                    return Json(new { success = false, message = "Data tidak ditemukan." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // --- FITUR PILIH NOTA ---
        public async Task<IActionResult> PilihNota()
        {
            // Logic sama, hanya load view partial
            return PartialView("PilihNota");
        }

        // [HttpPost]
        // public async Task<IActionResult> GetNotaProduksi([DataSourceRequest] DataSourceRequest request, string searchKeyword, string jenisAsset)
        // {
        //     if (string.IsNullOrEmpty(searchKeyword) && string.IsNullOrEmpty(jenisAsset))
        //     {
        //         return Json(await new List<InquiryNotaProduksiDto>().ToDataSourceResultAsync(request));
        //     }

        //     var data = await Mediator.Send(new GetNotaUntukPembayaranQuery()
        //     {
        //         SearchKeyword = searchKeyword,
        //         JenisAsset = jenisAsset
        //     });

        //     return Json(await data.ToDataSourceResultAsync(request));
        // }

        // --- SIMPAN DETAIL DARI NOTA (MULTIPLE) ---
        [HttpPost]
        public async Task<ActionResult> SimpanNota([FromBody] CreateJurnalMemorial117DetailNotaCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.NoVoucher))
                    throw new Exception("Nomor voucher tidak boleh kosong.");

                if (command.Data == null || !command.Data.Any())
                    throw new Exception("Tidak ada data nota yang dikirim.");

                command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
                return Ok(new { Status = "OK", Message = "Data berhasil disimpan" });
            }
            catch (Exception e)
            {
                return Ok(new { Status = "ERROR", Message = e.Message });
            }
        }

        // --- HELPER: GET KURS ---
        [HttpGet]
        public async Task<IActionResult> GetKurs(string kodeMataUang, DateTime tanggalVoucher)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
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