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
using ABB.Application.Coas.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace ABB.Web.Modules.EntriPembayaranKas
{
    public class EntriPembayaranKasController : AuthorizedBaseController
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
        public async Task<ActionResult> GetEntriPembayaranKas([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
             var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllVoucherKasQuery() { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = false

                });
            return Json(await data.ToDataSourceResultAsync(request));
        }
        [HttpPost]
        public async Task<ActionResult> GetEntriPembayaranKasFinal([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
             var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllVoucherKasQuery() { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = true
                
                });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailPembayaran([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetAllEntriPembayaranKasQuery { NoVoucher = noVoucher });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult> GetDetailPembayaranTemp([DataSourceRequest] DataSourceRequest request, string noVoucher)
        {
            var data = await Mediator.Send(new GetAllEntriPembayaranKasTempQuery { NoVoucher = noVoucher });
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

            var databaseName = Request.Cookies["DatabaseValue"];

            var voucherDto = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = noVoucher });
            if (voucherDto == null) return NotFound();

            // u/ flagpembayaran
            ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }
                

            };

           var kodeCabangCookie = Request.Cookies["UserCabang"]?.Trim();
            

            string glDept = null;
            if (!string.IsNullOrEmpty(kodeCabangCookie) && kodeCabangCookie.Length >= 2)
            {
                glDept = kodeCabangCookie.Substring(kodeCabangCookie.Length - 2);
            }
            ViewBag.DebugUserCabang = glDept;

            var akunlist = await Mediator.Send(new GetAllCoaQuery());

            if (!string.IsNullOrEmpty(glDept))
            {
                akunlist = akunlist
                    .Where(x => x.Dept == glDept)   // sesuaikan nama field
                    .ToList();
            }
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}" 
            }).ToList();

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
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else // kalau No = 0 atau null → create baru
            {
                var command = Mapper.Map<CreatePembayaranKasCommand>(model);
                  command.KodeUserInput = CurrentUser.UserId;
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
        public async Task<IActionResult> PilihNota(string NoVoucher)
        {
            var akunlist = await Mediator.Send(new GetAllCoaQuery());
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.Kode.Trim(), // Pastikan di-trim
                Text = $"{x.Kode.Trim()} - {x.Nama.Trim()}" 
            }).ToList();
            
            ViewBag.NoVoucher = NoVoucher;
            return PartialView("PilihNota");
        }

        // Action untuk mengisi data ke grid _PilihNota
        [HttpPost]
        public async Task<IActionResult> GetNotaProduksi([DataSourceRequest] DataSourceRequest request,
            string searchKeyword,
            string jenisAsset,
            DateTime? startDate, 
            DateTime? endDate)
        {
            // ✅ Cegah load data jika semua filter kosong
            if (string.IsNullOrEmpty(searchKeyword) &&
                string.IsNullOrEmpty(jenisAsset))
            {
                var emptyList = new List<InquiryNotaProduksiDto>();
                return Json(await emptyList.ToDataSourceResultAsync(request));
            }

            var kodeCabangCookie = Request.Cookies["UserCabang"]?.Trim();
            string glDept = null;
            if (!string.IsNullOrEmpty(kodeCabangCookie) && kodeCabangCookie.Length >= 2)
            {
                glDept = kodeCabangCookie.Substring(kodeCabangCookie.Length - 2);
            }

            // 🔹 Ambil data sesuai filter
            var data = await Mediator.Send(new GetNotaUntukPembayaranQuery()
            {
                SearchKeyword = searchKeyword,
                JenisAsset = jenisAsset,
                StartDate = startDate, 
                EndDate = endDate,
                glDept = glDept
            });

            // ✅ Jika hasil kosong, kirim response dengan indikator “tidak ditemukan”
           
            return Json(await data.ToDataSourceResultAsync(request));
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
            public async Task<IActionResult> GetTotalPembayaran(string noVoucher, string voucherDK)
            {
                if (string.IsNullOrEmpty(noVoucher))
                {
                    return BadRequest("No Voucher tidak boleh kosong.");
                }

                // Panggil handler baru yang kita buat
                var total = await Mediator.Send(new GetTotalPembayaranQuery { NoVoucher = noVoucher, VoucherDK = voucherDK });
                
                // Kembalikan totalnya sebagai JSON
                return Json(new { totalPembayaran = total });
            }

            [HttpGet]
            public async Task<IActionResult> GetTotalPembayaranFinal(string noVoucher, string voucherDK)
            {
                if (string.IsNullOrEmpty(noVoucher))
                {
                    return BadRequest("No Voucher tidak boleh kosong.");
                }

                // Panggil handler baru yang kita buat
                var total = await Mediator.Send(new GetTotalPembayaranFinalQuery { NoVoucher = noVoucher, VoucherDK = voucherDK });
                
                // Kembalikan totalnya sebagai JSON
                return Json(new { totalPembayaran = total });
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
            public async Task<ActionResult> SimpanNota([FromBody] CreatePembayaranKasNotaCommand command)
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
                    return Ok(new { Status = "ERROR", Message = e.InnerException?.Message ?? e.Message });
                }
            }


             [HttpGet]
                public async Task<IActionResult> GetJenisAssetList()
                {
                    var list = await Mediator.Send(new GetDistinctJenisAssetQuery());

                    var result = list.Select(x => new
                    {
                        NamaJenisAsset = x,
                        KodeJenisAsset = x
                    }).ToList();

                    return Json(result);
                }

           [HttpPost]
            public async Task<IActionResult> SaveFinal([FromBody] SaveFinalPembayaranKasRequest request)
            {
                if (string.IsNullOrEmpty(request.NoVoucher))
                    return Json(new { success = false, message = "Nomor voucher tidak boleh kosong." });

                try
                {
                    var result = await Mediator.Send(new SaveFinalPembayaranKasCommand
                    {
                        NoVoucher = request.NoVoucher
                    });

                    return Json(new
                    {
                        success = true,
                        message = $"Berhasil memindahkan {result} data dari tabel TEMP ke tabel FINAL."
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

        public async Task<IActionResult> Lihat(string noVoucher)
        {

            var databaseName = Request.Cookies["DatabaseValue"];

            var voucherDto = await Mediator.Send(new GetVoucherKasByIdQuery { NoVoucher = noVoucher });
            if (voucherDto == null) return NotFound();
            // u/ flagpembayaran
            ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }


            };
            var akunlist = await Mediator.Send(new GetAllCoaQuery());
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}"
            }).ToList();

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
        public async Task<IActionResult> UpdateFinal([FromBody] EntriPembayaranKasViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.No > 0) 
            {
                // JANGAN pakai Mapper.Map, pakai ini:
              
                var command = Mapper.Map<UpdateFinalPembayaranKasCommand>(model);
                
                await Mediator.Send(command);
            }
          
            return Json(new { success = true });
        }
            // Action untuk menghapus data
        [HttpGet]
        public async Task<IActionResult> ProsesUlang(string id)
        {
            await Mediator.Send(new ProsesUlangPembayaranKasCommand { NoVoucher = id });
            return Json(new { success = true });
        }
     
    }

    
}