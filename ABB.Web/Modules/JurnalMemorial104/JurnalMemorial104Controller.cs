using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.JurnalMemorial104.Models; // Pastikan namespace ViewModel benar
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

// Import Command & Query (Sesuaikan namespace Application Anda)
using ABB.Application.JurnalMemorials104.Commands;
using ABB.Application.JurnalMemorials104.Queries;
using ABB.Application.Cabangs.Queries;
using ABB.Application.Coas.Queries;
using ABB.Application.MataUangs.Queries;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Application.MataUangs.Queries;

namespace ABB.Web.Modules.JurnalMemorial104
{
    public class JurnalMemorial104Controller : AuthorizedBaseController
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

      
        [HttpPost]
        public async Task<ActionResult> GetJurnalMemorial104([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllJurnalMemorial104Query() 
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
            var viewModel = new JurnalMemorial104ViewModel(); // Sesuaikan nama ViewModel

            // Load Master Data untuk Dropdown
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            var akunList = await Mediator.Send(new GetAllCoaQuery());

            // MODE ADD (NoVoucher kosong)
            if (string.IsNullOrEmpty(noVoucher))
            {
                viewModel.JurnalHeader = new JurnalMemorial104Dto();
                
                var now = DateTime.Now;
                var namaCabang = cabangList.FirstOrDefault(c => c.kd_cb.Trim() == userCabang?.Trim())?.nm_cb?.Trim();
                
                ViewBag.DisplayCabang = $"{userCabang} - {namaCabang}";

                var nextNoVoucher = await Mediator.Send(new GetNextNoVoucherJurnalQuery 
                { 
                    KodeCabang = userCabang,
                    Bulan = now.Month,
                    Tahun = now.Year
                });

                // Generate No Voucher Baru (Opsional, jika ada logic auto number)
                // var nextNoVoucher = await Mediator.Send(new GetNextNoVoucherJurnalQuery { ... });
                
                viewModel.JurnalHeader.KodeCabang = userCabang;
                viewModel.JurnalHeader.Tanggal = now;
                viewModel.JurnalHeader.NoVoucher = nextNoVoucher;
                // viewModel.JurnalHeader.NoVoucher = nextNoVoucher; // Jika auto number
            }
            // MODE EDIT
            else
            {
                var headerDto = await Mediator.Send(new GetJurnalMemorial104ByIdQuery { KodeCabang = kodeCabang, NoVoucher = noVoucher });
                if (headerDto == null) return NotFound();
                
                viewModel.JurnalHeader = headerDto;

                // Load Detail Existing
                var detailListDto = await Mediator.Send(new GetDetailJurnalMemorial104Query { NoVoucher = headerDto.NoVoucher });
                viewModel.JurnalItems = Mapper.Map<List<JurnalMemorial104Item>>(detailListDto);
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

         [HttpPost]
        public async Task<ActionResult> GetDetailJurnal([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetDetailJurnalMemorial104Query { NoVoucher = noVoucher });
            return Json(await data.ToDataSourceResultAsync(request));
        }

         [HttpPost]
        public async Task<IActionResult> SaveHeader([FromBody] JurnalMemorial104Dto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingData = await Mediator.Send(new GetJurnalMemorial104ByIdQuery 
            { 
                KodeCabang = model.KodeCabang, 
                NoVoucher = model.NoVoucher 
            });

            string finalNoVoucher;

            if (existingData != null) // UPDATE
            {
                var command = Mapper.Map<UpdateJurnalMemorial104HeaderCommand>(model);
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
                finalNoVoucher = model.NoVoucher;
            }
            else // CREATE
            {
                var command = Mapper.Map<CreateJurnalMemorial104HeaderCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                finalNoVoucher = await Mediator.Send(command);
            }

            return Json(new { success = true, noVoucher = finalNoVoucher });
        }

         [HttpPost]
        public async Task<IActionResult> SaveDetail([FromBody] JurnalMemorial104ViewModel model)
        {
            // Validasi manual field detail yang wajib
            if (string.IsNullOrEmpty(model.NoVoucher) || string.IsNullOrEmpty(model.KodeAkun))
                return BadRequest("Data tidak lengkap.");

            if (model.No > 0) // UPDATE Detail
            {
                var command = Mapper.Map<UpdateJurnalMemorial104DetailCommand>(model);
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else // CREATE Detail Baru
            {
                var command = Mapper.Map<CreateJurnalMemorial104DetailCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

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

         [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteJurnalMemorial104DetailCommand command)
        {
            if (command == null || command.No <= 0 || string.IsNullOrEmpty(command.NoVoucher))
            {
                return Json(new { success = false, message = "Parameter tidak valid." });
            }

            await Mediator.Send(command);
            return Json(new { success = true });
        }

         [HttpPost]
        public async Task<IActionResult> DeleteHeader([FromBody] DeleteJurnalMemorial104HeaderCommand command)
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

        [HttpGet]
        public async Task<IActionResult> Lihat(string kodeCabang, string noVoucher)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var viewModel = new JurnalMemorial104ViewModel();

            // Load Master Data (untuk mengisi dropdown value -> text)
            // Sebenarnya di mode lihat kita bisa pakai textbox biasa, tapi pakai combo readonly juga oke
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            var akunList = await Mediator.Send(new GetAllCoaQuery());

            // Load Data Header
            var headerDto = await Mediator.Send(new GetJurnalMemorial104ByIdQuery { KodeCabang = kodeCabang, NoVoucher = noVoucher });
            if (headerDto == null) return NotFound();
            
            viewModel.JurnalHeader = headerDto;

            // Load Data Detail
            // Kita tidak perlu load ke viewModel.JurnalItems jika grid me-load via AJAX,
            // tapi method ini tetap dipanggil untuk konsistensi
            var detailListDto = await Mediator.Send(new GetDetailJurnalMemorial104Query { NoVoucher = headerDto.NoVoucher });
            viewModel.JurnalItems = Mapper.Map<List<JurnalMemorial104Item>>(detailListDto);


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

            return PartialView("Lihat", viewModel); // Render View 'Lihat.cshtml'
        }

        [HttpGet]
        public async Task<IActionResult> GetNextNoVoucher(string kodeCabang, int bulan, int tahun)
        {
            // Pastikan Query ini ada dan namespace-nya sudah di-using
            var nextNo = await Mediator.Send(new GetNextNoVoucherJurnalQuery 
            { 
                KodeCabang = kodeCabang, 
                Bulan = bulan, 
                Tahun = tahun 
            });
            
            return Json(new { success = true, noVoucher = nextNo });
        }


        // --- TAMBAHKAN CLASS REQUEST INI DI BAGIAN BAWAH ATAU ATAS CONTROLLER ---
        public class CopyJurnalRequest
        {
            public string KodeCabang { get; set; }
            public string NoVoucherLama { get; set; }
            public DateTime TanggalBaru { get; set; }
        }

        // --- TAMBAHKAN ACTION INI DI DALAM KELAS JurnalMemorial104Controller ---
        [HttpPost]
        public async Task<IActionResult> CopyJurnal([FromBody] CopyJurnalRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NoVoucherLama) || string.IsNullOrEmpty(request.KodeCabang))
                    return Json(new { success = false, message = "Data sumber tidak valid." });

                // 1. Ambil Header Lama
                var headerLama = await Mediator.Send(new GetJurnalMemorial104ByIdQuery 
                { 
                    KodeCabang = request.KodeCabang, 
                    NoVoucher = request.NoVoucherLama 
                });

                if (headerLama == null)
                    return Json(new { success = false, message = "Voucher lama tidak ditemukan." });

                // 2. Ambil Detail Lama
                var detailLama = await Mediator.Send(new GetDetailJurnalMemorial104Query 
                { 
                    NoVoucher = request.NoVoucherLama 
                });

                // 3. Generate Nomor Voucher Baru sesuai Tanggal Baru
                var nextNoVoucher = await Mediator.Send(new GetNextNoVoucherJurnalQuery 
                { 
                    KodeCabang = request.KodeCabang,
                    Bulan = request.TanggalBaru.Month,
                    Tahun = request.TanggalBaru.Year
                });

                // 4. Siapkan & Simpan Header Baru
                headerLama.NoVoucher = nextNoVoucher;
                headerLama.Tanggal = request.TanggalBaru;
                headerLama.TanggalInput = DateTime.Now;
                headerLama.TanggalUpdate = null; // Reset update
                headerLama.FlagGL = false; // Flag posting direset jadi belum diposting

                var createHeaderCommand = Mapper.Map<CreateJurnalMemorial104HeaderCommand>(headerLama);
                createHeaderCommand.KodeUserInput = CurrentUser.UserId; // User yang copy
                await Mediator.Send(createHeaderCommand);

                // 5. Siapkan & Simpan Detail Baru (Looping)
                if (detailLama != null && detailLama.Any())
                {
                    foreach (var item in detailLama)
                    {
                        // MAPPING MANUAL: Pindahkan data lama ke Command Baru satu per satu
                        // Ini menghindari error AutoMapper "Missing type map"
                        var createDetailCommand = new CreateJurnalMemorial104DetailCommand
                        {
                            NoVoucher = nextNoVoucher, // Gunakan No Voucher yang baru di-generate
                            
                            // Copy data transaksi dari item lama
                            KodeAkun = item.KodeAkun,
                            NoNota = item.NoNota,
                            KodeMataUang = item.KodeMataUang,
                            NilaiDebet = item.NilaiDebet,
                            NilaiKredit = item.NilaiKredit,
                            NilaiDebetRp = item.NilaiDebetRp,
                            NilaiKreditRp = item.NilaiKreditRp,
                            
                            // Penting: Pastikan properti ini namanya sesuai dengan di DTO kamu. 
                            // Kalau di DTO namanya cuma "Keterangan", ganti jadi item.Keterangan
                            KeteranganDetail = item.KeteranganDetail, 
                            
                            KodeUserInput = CurrentUser.UserId
                        };
                        
                        // Tembak ke database
                        await Mediator.Send(createDetailCommand);
                    }
                }

                return Json(new { success = true, noVoucherBaru = nextNoVoucher });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }

    
}