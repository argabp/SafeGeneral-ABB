using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.VoucherKass.Queries;
// tambahan cabang
using ABB.Application.Cabangs.Queries;
// tambahan mata uang
using ABB.Application.MataUangs.Queries;
// tambahan untuk kode akun
using ABB.Application.KasBanks.Queries;

using ABB.Application.EntriPembayaranKass.Queries;

using ABB.Web.Modules.Base;
using ABB.Web.Modules.VoucherKas.Models;
using ABB.Web.Modules.EntriPembayaranKas.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ABB.Web.Modules.VoucherKas
{
    public class VoucherKasController : AuthorizedBaseController
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
        public async Task<ActionResult> GetVoucherKas([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var kodeCabang = Request.Cookies["UserCabang"];

            var data = await Mediator.Send(new GetAllVoucherKasQuery() { 
                SearchKeyword = searchKeyword ,
                KodeCabang = kodeCabang
                
            });
            return Json(await data.ToDataSourceResultAsync(request));
        }

         [HttpGet]
        public async Task<IActionResult> GetKurs(string kodeMataUang, DateTime tanggalVoucher)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            // if (string.IsNullOrEmpty(databaseName))
            // {
            //     return RedirectToAction("Logout", "Account");
            // }
            var kurs = await Mediator.Send(new GetKursMataUangQuery
            {
                DatabaseName = databaseName,
                KodeMataUang = kodeMataUang,
                TanggalVoucher = tanggalVoucher
            });
            return Json(new { nilai_kurs = kurs });
        }

        [HttpGet]
        public async Task<IActionResult> GetNextVoucherNumber(DateTime? tanggalVoucher, string kodeCabang) 
        {
            var dateToUse = tanggalVoucher ?? DateTime.Now; 

            var nextNumber = await Mediator.Send(new GetNextVoucherNumberQuery 
            { 
                Bulan = dateToUse.Month, 
                Tahun = dateToUse.Year,
                KodeCabang = kodeCabang
                // Parameter lain tidak perlu dikirim ke Query
            });
            
            return Json(new { success = true, nextNumber = nextNumber });
        }

        // TAMBAHKAN ACTION INI UNTUK AJAX
        [HttpGet]
        public async Task<IActionResult> GetAkunByKas(string kodeKas)
        {
            // Query ini asumsinya sama dengan GetKasBankByIdQuery yang dipakai di VoucherBank
            var kasData = await Mediator.Send(new GetKasBankByIdQuery 
            { 
                Kode = kodeKas 
            });

            if (kasData != null)
            {
                return Json(new { success = true, kodeAkun = kasData.NoPerkiraan });
            }

            return Json(new { success = false });
        }

         // Action untuk menampilkan form Add
        public async Task<IActionResult> Add()
        {
             var databaseName = Request.Cookies["DatabaseValue"];
                var viewModel = new VoucherKasViewModel();

                var userCabang = Request.Cookies["UserCabang"];
                //  kodecabang
                var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });

               var namaCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), userCabang?.Trim(), StringComparison.OrdinalIgnoreCase))
                ?.nm_cb?.Trim();

                ViewBag.DisplayCabang = $"{userCabang} - {namaCabang}";

                 ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
                    {
                        Value = c.kd_cb.Trim(),
                        Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}",
                    
                    }).ToList();
              
                
                viewModel.KodeCabang = userCabang;
               

                 


                // u/ debetkredit
                ViewBag.DebetKreditOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Pilih..", Value = "" },
                    new SelectListItem { Text = "Kredit", Value = "K" },
                    new SelectListItem { Text = "Debit", Value = "D" }
                    
                };

                // select mata uang
                var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
                ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
                {
                    Value = x.kd_mtu.Trim(),
                    Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
                }).ToList();

                // --- UPDATE BAGIAN INI (Ganti logic Akun jadi Kas) ---
            
                // 1. Ambil List KAS
                var kasList = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS", KodeCabang = userCabang });
                
                ViewBag.KodeKasOptions = kasList.Select(x => new SelectListItem
                {
                    Value = x.Kode,
                    Text = $"{x.Kode} - {x.Keterangan}"
                }).ToList();

                // kodeakun
                var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS", KodeCabang = userCabang });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{x.NoPerkiraan} - {x.Keterangan}"
                }).ToList();
                ViewBag.Kode = akunlist.FirstOrDefault()?.Kode;

                // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };

                

            
            // var model = new VoucherKasViewModel();
            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] VoucherKasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // LOGIC BARU: Cek berdasarkan ID
            // Jika ID > 0 artinya data sudah ada di DB (Update)
            // Jika ID == 0 artinya data baru (Insert)
            if (model.Id > 0) 
            {
                // UPDATE
                var command = Mapper.Map<UpdateVoucherKasCommand>(model);
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else
            {
                // CREATE
                var command = Mapper.Map<CreateVoucherKasCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

         // Action untuk menampilkan form Edit (dengan data yang sudah ada)
        public async Task<IActionResult> Edit(long id)
        {

             var databaseName = Request.Cookies["DatabaseValue"];
            // if (string.IsNullOrEmpty(databaseName))
            //     {
            //         return RedirectToAction("Logout", "Account");
            //     }
                // kode cabang
                var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
                ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
                {
                    Value = c.kd_cb.Trim(),
                    Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}"
                }).ToList();

                // u/debetkredit
                ViewBag.DebetKreditOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Debit", Value = "D" },
                    new SelectListItem { Text = "Kredit", Value = "K" }
                };

                // select mata uang
                var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
                ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
                {
                    Value = x.kd_mtu.Trim(),
                    Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
                }).ToList();

                // 1. Ambil List KAS
                var kasList = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS" });
                ViewBag.KodeKasOptions = kasList.Select(x => new SelectListItem
                {
                    Value = x.Kode,
                    Text = $"{x.Kode} - {x.Keterangan}"
                }).ToList();

                // kodeakun
                var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "KAS" });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{int.Parse(x.NoPerkiraan):N0} - {x.Keterangan}" // Format: NO PERKIRAAN - NAMA BANK
                }).ToList();

                // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };

                var dto = await Mediator.Send(new GetVoucherKasByIdQuery { Id = id }); 
            
                if (dto == null)
                {
                    return NotFound();
                }
                var model = Mapper.Map<VoucherKasViewModel>(dto);
                return PartialView(model);
        }

        // Action untuk menghapus data
        [HttpGet]
       public async Task<IActionResult> Delete(long id) // <-- Ganti string jadi long
        {
            // Kirim ID ke Command
            await Mediator.Send(new DeleteVoucherKasCommand { Id = id });
            return Json(new { success = true });
        }

       [HttpGet]
        public async Task<IActionResult> GetNextVoucherSementara(string kodeCabang, string kodeKas, DateTime? tanggalVoucher, string debetKredit)
        {
            // Validasi input
            if (string.IsNullOrEmpty(kodeCabang) || string.IsNullOrEmpty(kodeKas) || tanggalVoucher == null)
            {
                return Json(new { success = false, message = "Data belum lengkap" });
            }

            var dateToUse = tanggalVoucher.Value;

            // 1. MINTA NOMOR URUT (Global Sequence)
            // Kita cuma kirim Cabang, Bulan, Tahun. 
            // Query akan mengabaikan apakah itu KK01 atau KD01.
            var nextSequence = await Mediator.Send(new GetNextVoucherSementaraNumberQuery 
            { 
                KodeCabang = kodeCabang,
                Bulan = dateToUse.Month, 
                Tahun = dateToUse.Year
            });

            var sequenceStr = nextSequence.ToString("000"); // Contoh: "002"

            // 2. RAKIT STRING TAMPILAN (FORMAT LENGKAP)
            // Format: SMT / Cabang / Tipe+Kas / Bulan / Tahun / Urut
            
            // A. Cabang (2 digit)
            var cabangFormat = kodeCabang.Length >= 2 ? kodeCabang.Substring(kodeCabang.Length - 2) : kodeCabang; 

            // B. Tengah (K + D/K + KodeKas) -> Contoh: KD01 atau KK01
            // Logic ini sama persis dengan Voucher Original
            string prefixTengah = "K"; 
            if (!string.IsNullOrEmpty(debetKredit))
            {
                if (debetKredit.ToUpper() == "D") prefixTengah += "D";
                else if (debetKredit.ToUpper() == "K") prefixTengah += "K";
            }
            prefixTengah += kodeKas;

            // C. Bulan/Tahun
            var bulan = dateToUse.Month.ToString("00");
            var tahun = dateToUse.Year.ToString();

            // 3. GABUNGKAN
            // Hasil: SMT/50/KD01/02/2026/002
            var noVoucherSmt = $"SMT/{cabangFormat}/{prefixTengah}/{bulan}/{tahun}/{sequenceStr}";

            return Json(new { success = true, noVoucherSmt = noVoucherSmt });
        }

      [HttpGet]
        public async Task<IActionResult> Cetak(long id)
        {
            if (id <= 0)
                return BadRequest("ID voucher tidak valid.");

            var voucher = await Mediator.Send(
                new GetVoucherKasByIdQuery { Id = id });

            if (voucher == null)
                return NotFound($"Voucher dengan ID {id} tidak ditemukan.");

            var viewModel = new EntriPembayaranKasViewModel
            {
                VoucherKasHeader = voucher
            };

            return View(viewModel);
        }

    }
}