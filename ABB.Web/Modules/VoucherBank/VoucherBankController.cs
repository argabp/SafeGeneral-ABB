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
using ABB.Web.Modules.Base;
using ABB.Web.Modules.VoucherBank.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ABB.Web.Modules.VoucherBank
{
    public class VoucherBankController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetVoucherBank([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var data = await Mediator.Send(new GetAllVoucherBankQuery() { SearchKeyword = searchKeyword });
            return Json(await data.ToDataSourceResultAsync(request));
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

         public async Task<IActionResult> Add()
        {
            // get db untuk relasi
            var databaseName = Request.Cookies["DatabaseName"];
           
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
            // -------------------------
            // u/ jenispembayaran
                ViewBag.jenispembayaranOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "KAS", Value = "KAS" },
                    new SelectListItem { Text = "CEK", Value = "CEK" },
                    new SelectListItem { Text = "GIRO", Value = "GIRO" },
                    new SelectListItem { Text = "TRANSFER", Value = "TRANS" }
                    
                };
            
            var model = new VoucherBankViewModel();
            model.JenisVoucher = "BANK"; 
            
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetNextVoucherNumber()
        {
            var now = DateTime.Now;
            var nextNumber = await Mediator.Send(new GetNextVoucherNumberQuery 
            { 
                Bulan = now.Month, 
                Tahun = now.Year % 100 // Ambil 2 digit terakhir tahun
            });
            
            return Json(new { success = true, nextNumber = nextNumber });
        }

         // Action untuk menampilkan form Edit (dengan data yang sudah ada)
        public async Task<IActionResult> Edit(string id)
        {
             // get db untuk relasi
            var databaseName = Request.Cookies["DatabaseName"];
            
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

            var dto = await Mediator.Send(new GetVoucherBankByIdQuery { NoVoucher = id });
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

            var existingData = await Mediator.Send(new GetVoucherBankByIdQuery { NoVoucher = model.NoVoucher });

            if (existingData != null) // Jika data sudah ada, jalankan Update
            {
                await Mediator.Send(Mapper.Map<UpdateVoucherBankCommand>(model));
            }
            else // Jika data belum ada, jalankan Create
            {
                await Mediator.Send(Mapper.Map<CreateVoucherBankCommand>(model));
            }

            return Json(new { success = true });
        }

        // Action untuk menghapus data
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteVoucherBankCommand { NoVoucher = id });
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAkunByBank(string kodeBank)
        {
            // HAPUS BARIS INI KARENA TIDAK DIPERLUKAN
            // var databaseName = Request.Cookies["DatabaseName"];

            // Panggil query HANYA dengan parameter yang ada (yaitu 'Kode')
            var bankData = await Mediator.Send(new GetKasBankByIdQuery 
            { 
                Kode = kodeBank 
            });

            if (bankData != null)
            {
                return Json(new { success = true, kodeAkun = bankData.NoPerkiraan });
            }

            return Json(new { success = false });
        }

    }
}