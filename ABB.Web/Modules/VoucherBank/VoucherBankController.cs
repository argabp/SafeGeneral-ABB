using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.VoucherBanks.Commands;
using ABB.Application.VoucherBanks.Queries;
// tambahan cabang
using ABB.Application.Cabangs.Queries;
// tambahan mata uang
using ABB.Application.MataUangs.Queries;

using ABB.Application.KasBanks.Queries;
using ABB.Application.EntriPembayaranBanks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.VoucherBank.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Web.Modules.EntriPembayaranBank.Models;


namespace ABB.Web.Modules.VoucherBank
{
    public class VoucherBankController : AuthorizedBaseController
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
        public async Task<ActionResult> GetVoucherBank([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
             var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllVoucherBankQuery() { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang
                 });
            return Json(await data.ToDataSourceResultAsync(request));
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

         public async Task<IActionResult> Add()
        {
            // get db untuk relasi
            var databaseName = Request.Cookies["DatabaseValue"];
            var kodeCabangCookie = Request.Cookies["UserCabang"];
      
            // kode cabang 

            var model = new VoucherBankViewModel();
            model.JenisVoucher = "BANK";
            model.KodeUserInput = CurrentUser.UserId;
            model.NamaUserInput = CurrentUser.UserName; 
           

            
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
             var namaCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase))
                ?.nm_cb?.Trim();

            ViewBag.DisplayCabang = $"{kodeCabangCookie} - {namaCabang}";
            model.KodeCabang = kodeCabangCookie;

            ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
            {
                Value = c.kd_cb.Trim(),
                Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}",
                Selected = c.kd_cb.Trim() == model.KodeCabang
            }).ToList();

            // select mata uang
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
            {
                Value = x.kd_mtu.Trim(),
                Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
            }).ToList();

            // select debetkredit
            ViewBag.DebetKreditOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pilih..", Value = "" },
                new SelectListItem { Text = "Debit", Value = "D" },
                new SelectListItem { Text = "Kredit", Value = "K" }
            };

            // kode akun
            var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{x.NoPerkiraan} - {x.Keterangan}"
                }).ToList();

           // kodebank
            var bankList = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" , KodeCabang = kodeCabangCookie});
            ViewBag.KodeBankOptions = bankList.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Keterangan}" // Format: KODE - NAMA BANK
            }).ToList();
            // -------------------------
            // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };
            
          
          
            
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetNextVoucherNumber(DateTime tanggalVoucher, string kodeCabang)
        {
            // Validasi sederhana
            if (string.IsNullOrEmpty(kodeCabang))
            {
                return Json(new { success = false, message = "Kode Cabang kosong" });
            }

            var nextNumber = await Mediator.Send(new GetNextVoucherNumberQuery 
            { 
                Bulan = tanggalVoucher.Month, 
                Tahun = tanggalVoucher.Year, // (Gunakan full year, nanti handler yg atur logicnya)
                KodeCabang = kodeCabang      // <--- KIRIM INI
            });
            
            return Json(new { success = true, nextNumber = nextNumber });
        }

         // Action untuk menampilkan form Edit (dengan data yang sudah ada)
        public async Task<IActionResult> Edit(long id)
        {
             // get db untuk relasi
            var databaseName = Request.Cookies["DatabaseValue"];

            // if (string.IsNullOrEmpty(databaseName))
            // {
            //     return RedirectToAction("Logout", "Account");
            // }
            // kode cabang
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            ViewBag.KodeCabangOptions = cabangList.Select(c => new SelectListItem
            {
                Value = c.kd_cb.Trim(),
                Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}"
            }).ToList();

            // select mata uang
            var mataUangList = await Mediator.Send(new GetMataUangQuery { DatabaseName = databaseName });
            ViewBag.MataUangOptions = mataUangList.Select(x => new SelectListItem
            {
                Value = x.kd_mtu.Trim(),
                Text = $"{x.kd_mtu.Trim()} - {x.nm_mtu.Trim()}"
            }).ToList();

            // select debetkredit
             ViewBag.DebetKreditOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Debit", Value = "D" },
                new SelectListItem { Text = "Kredit", Value = "K" }
            };
            // kode akun
            var akunlist = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" });
                ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
                {
                    Value = x.NoPerkiraan,
                    Text = $"{int.Parse(x.NoPerkiraan):N0} - {x.Keterangan}"
                }).ToList();
                
             // kodebank
            var bankList = await Mediator.Send(new GetAllKasBankQuery { TipeKasBank = "BANK" });
            ViewBag.KodeBankOptions = bankList.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Keterangan}" // Format: KODE - NAMA BANK
            }).ToList();

            // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };

            var dto = await Mediator.Send(new GetVoucherBankByIdQuery { Id = id });
            if (dto == null)
            {
                return NotFound();
            }
            var model = Mapper.Map<VoucherBankViewModel>(dto);
            return PartialView(model);
        }

        // Action untuk menyimpan data (baik data baru maupun editan)
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] VoucherBankViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id > 0) 
            {
                // UPDATE
                var command = Mapper.Map<UpdateVoucherBankCommand>(model);
                command.Id = model.Id; // Pastikan Id ter-set
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else
            {
                // CREATE
                var command = Mapper.Map<CreateVoucherBankCommand>(model);
                command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

        // Action untuk menghapus data
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteVoucherBankCommand { Id = id });
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAkunByBank(string kodeBank, string KodeCabang, string Tipe)
        {
            var bankData = await Mediator.Send(new GetKasBankByIdQuery 
            { 
                Kode = kodeBank,
                KodeCabang = KodeCabang,
                Tipe = Tipe
            });

            if (bankData != null)
            {
                return Json(new { success = true, kodeAkun = bankData.NoPerkiraan });
            }

            return Json(new { success = false });
        }

        [HttpGet]
                public async Task<IActionResult> Cetak(long id)
                {
                    if (id <= 0)
                        return BadRequest("ID voucher tidak valid.");

                    var voucher = await Mediator.Send(
                        new GetVoucherBankByIdQuery { Id = id });

                    if (voucher == null)
                        return NotFound($"Voucher dengan ID {id} tidak ditemukan.");

                    var viewModel = new EntriPembayaranBankViewModel
                    {
                        VoucherHeader = voucher
                    };

                    return View(viewModel);
                }

            [HttpGet]
            public async Task<IActionResult> GetNextVoucherSementara(string kodeCabang, string kodeBank, DateTime? tanggalVoucher, string debetKredit)
            {
                // Validasi input
                if (string.IsNullOrEmpty(kodeCabang) || string.IsNullOrEmpty(kodeBank) || tanggalVoucher == null)
                {
                    return Json(new { success = false, message = "Data belum lengkap" });
                }

                var dateToUse = tanggalVoucher.Value;

                // 1. MINTA NOMOR URUT (Global Sequence per Cabang & Periode)
                var nextSequence = await Mediator.Send(new GetNextVoucherSementaraNumberQuery 
                { 
                    KodeCabang = kodeCabang,
                    Bulan = dateToUse.Month, 
                    Tahun = dateToUse.Year
                });

                var sequenceStr = nextSequence.ToString("000"); // Contoh: "002"

                // 2. RAKIT STRING TAMPILAN (FORMAT LENGKAP)
                // Format: SMT / Cabang / Tipe+Bank / Bulan / Tahun / Urut
                
                // A. Cabang (2 digit)
                var cabangFormat = kodeCabang.Length >= 2 ? kodeCabang.Substring(kodeCabang.Length - 2) : kodeCabang; 

                // B. Tengah (B + D/K + KodeBank)
                // "B" untuk Bank. "D" Debit, "K" Kredit.
                string prefixTengah = "B"; 
                if (!string.IsNullOrEmpty(debetKredit))
                {
                    if (debetKredit.ToUpper() == "D") prefixTengah += "D";
                    else if (debetKredit.ToUpper() == "K") prefixTengah += "K";
                }
                prefixTengah += kodeBank; // Contoh jadi: BD01 atau BK01

                // C. Bulan/Tahun
                var bulan = dateToUse.Month.ToString("00");
                var tahun = dateToUse.Year.ToString();

                // 3. GABUNGKAN
                // Hasil: SMT/50/BD01/02/2026/002
                var noVoucherSmt = $"SMT/{cabangFormat}/{prefixTengah}/{bulan}/{tahun}/{sequenceStr}";

                return Json(new { success = true, noVoucherSmt = noVoucherSmt });
            }

    }
}