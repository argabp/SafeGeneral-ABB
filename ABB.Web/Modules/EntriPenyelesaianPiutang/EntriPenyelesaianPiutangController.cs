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
using ABB.Application.Coas.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ABB.Web.Modules.EntriPenyelesaianPiutang
{
    public class EntriPenyelesaianPiutangController : AuthorizedBaseController
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
        public async Task<ActionResult> GetEntriPenyelesaianPiutang([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {

            var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllHeaderPenyelesaianUtangQuery() { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = false
                
                 });
            return Json(await data.ToDataSourceResultAsync(request));

        }

         [HttpPost]
        public async Task<ActionResult> GetEntriPenyelesaianPiutangFinal([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
             var kodeCabang = Request.Cookies["UserCabang"];
            var data = await Mediator.Send(new GetAllHeaderPenyelesaianUtangQuery() { 
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = true
                });
            return Json(await data.ToDataSourceResultAsync(request));
        }
       

        // --- PERBAIKI ACTION INI (SEKARANG BISA UNTUK ADD & EDIT) ---
        public async Task<IActionResult> Add(string kodeCabang, string nomorBukti)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var viewModel = new EntriPenyelesaianPiutangViewModel();

              var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            // Cek apakah ini mode Edit (jika nomorBukti diisi)
            if (string.IsNullOrEmpty(nomorBukti))
            {
               viewModel.PenyelesaianHeader = new HeaderPenyelesaianUtangDto(); 

                var now = DateTime.Now;
                var jenisPenyelesaian = "BM"; // Asumsi default "BM"
                var userCabang = Request.Cookies["UserCabang"];
                var namaCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), userCabang?.Trim(), StringComparison.OrdinalIgnoreCase))
                ?.nm_cb?.Trim();
                ViewBag.DisplayCabang = $"{userCabang} - {namaCabang}";

                // Panggil Query untuk mendapatkan nomor bukti baru
                var nextNomorBukti = await Mediator.Send(new GetNextNomorBuktiQuery
                {
                    KodeCabang = userCabang,
                    JenisPenyelesaian = jenisPenyelesaian,
                    Bulan = now.Month,
                    Tahun = now.Year
                });

                // Isi ViewModel dengan nilai default
                viewModel.PenyelesaianHeader.KodeCabang = userCabang;
                viewModel.PenyelesaianHeader.Tanggal = now;
                viewModel.PenyelesaianHeader.JenisPenyelesaian = jenisPenyelesaian;
                viewModel.PenyelesaianHeader.NomorBukti = nextNomorBukti;

            }else // Mode Edit
                {
                    // Ambil data header
                    var headerDto = await Mediator.Send(new GetHeaderPenyelesaianUtangByIdQuery { KodeCabang = kodeCabang, NomorBukti = nomorBukti });
                    if (headerDto == null) return NotFound();
                    viewModel.PenyelesaianHeader = headerDto;

                    // --- TAMBAHKAN BAGIAN INI ---
                    // Ambil data detail yang sudah ada
                    var detailListDto = await Mediator.Send(new GetAllEntriPenyelesaianPiutangQuery { NoBukti = headerDto.NomorBukti });

                    // Mapping dari DTO ke ViewModel Item
                    viewModel.PembayaranItems = Mapper.Map<List<PenyelesaianPiutangItem>>(detailListDto);
                    // -----------------------------
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
                Text = $"{c.kd_cb.Trim()} - {c.nm_cb.Trim()}",
               
            }).ToList();

            ViewBag.JenisPenyelesaianOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bukti", Value = "BM" }
            };

             ViewBag.DebetKreditOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Debit", Value = "D" },
                new SelectListItem { Text = "Kredit", Value = "K" }
            };


            var kodeCabangCookie = Request.Cookies["UserCabang"]?.Trim();
            

            string glDept = null;
            if (!string.IsNullOrEmpty(kodeCabangCookie) && kodeCabangCookie.Length >= 2)
            {
                glDept = kodeCabangCookie.Substring(kodeCabangCookie.Length - 2);
            }
            ViewBag.DebugUserCabang = glDept;

            var COAList = await Mediator.Send(new GetAllCoaQuery 
            { 
                KodeCabang = glDept 
            });

            // if (!string.IsNullOrEmpty(glDept))
            // {
            //     COAList = COAList
            //         .Where(x => x.Dept == glDept)   // sesuaikan nama field
            //         .ToList();
            // }

             ViewBag.COAoptions = COAList.Select(c => new SelectListItem
            {
                Value = c.Kode.Trim(),
                Text = $"{c.Kode.Trim()} - {c.Nama.Trim()}",
               
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
        public async Task<ActionResult> GetTempDetailPembayaran([DataSourceRequest] DataSourceRequest request, string NoBukti)
        {
            var data = await Mediator.Send(new GetAllEntriPenyelesaianPiutangTempQuery { NoBukti = NoBukti });
            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveHeader([FromBody] HeaderPenyelesaianUtangDto model)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            // 1. Cek dulu ke database apakah data dengan Primary Key ini sudah ada
            var existingData = await Mediator.Send(new GetHeaderPenyelesaianUtangByIdQuery 
            { 
                KodeCabang = model.KodeCabang, 
                NomorBukti = model.NomorBukti 
            });

            string nomorBukti;

            if (existingData != null) // Jika data ditemukan -> UPDATE
            {
                var command = Mapper.Map<UpdateHeaderPenyelesaianUtangCommand>(model);
               
                await Mediator.Send(command);
                nomorBukti = model.NomorBukti; // Gunakan nomor bukti yang sudah ada
            }
            else // Jika tidak ditemukan -> CREATE
            {
                var command = Mapper.Map<CreateHeaderPenyelesaianUtangCommand>(model);
              
                nomorBukti = await Mediator.Send(command); // Dapatkan nomor bukti baru dari Handler
            }

            return Json(new { success = true, nomorBukti = nomorBukti });
        }

        // Action 'Save' sekarang menerima ViewModel utama
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriPenyelesaianPiutangViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Cek nilai 'No' untuk menentukan apakah ini Create atau Update
            if (model.No > 0) 
            {
                // Jika ada 'No', kita anggap ini UPDATE.
                // Kita perlu mapping dari CreateCommand ke UpdateCommand.
                var command = Mapper.Map<UpdatePenyelesaianPiutangCommand>(model);
               // Atau UserId
                command.KodeUserUpdate = CurrentUser.UserId;
                await Mediator.Send(command);
            }
            else // 'No' bernilai 0, berarti ini data BARU.
            {
                // Atau UserId
                var command = Mapper.Map<CreatePenyelesaianPiutangCommand>(model);
                 command.KodeUserInput = CurrentUser.UserId;
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

        // Nota Produksi 
        public async Task<IActionResult> PilihNota()
        {
            var userCabang = Request.Cookies["UserCabang"];
            string glDept = (!string.IsNullOrEmpty(userCabang) && userCabang.Length >= 2) 
                ? userCabang.Substring(userCabang.Length - 2) 
                : userCabang;

           var akunlist = await Mediator.Send(new GetAllCoaQuery { KodeCabang = glDept });
           
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.Kode.Trim(), // Pastikan di-trim
                Text = $"{x.Kode.Trim()} - {x.Nama.Trim()}" 
            }).ToList();
            return PartialView("PilihNota");
        }

        // Action untuk mengisi data ke grid _PilihNota
        [HttpPost]
        public async Task<IActionResult> GetNotaProduksi([DataSourceRequest] DataSourceRequest request,
            string searchKeyword,
            string jenisAsset,
            DateTime? startDate, // <--- TAMBAH
            DateTime? endDate)   // <--- TAMBAH
        {
            if (string.IsNullOrEmpty(searchKeyword) && string.IsNullOrEmpty(jenisAsset))
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

            var data = await Mediator.Send(new GetNotaUntukPembayaranQuery()
            {
                SearchKeyword = searchKeyword,
                JenisAsset = jenisAsset,
                StartDate = startDate, // <--- MAPPING
                EndDate = endDate,     // <--- MAPPING
                glDept = glDept
            });

            return Json(await data.ToDataSourceResultAsync(request));
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
        public async Task<ActionResult> SimpanNota([FromBody] CreatePenyelesaianPiutangNotaCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.NoBukti))
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
            public async Task<IActionResult> SaveFinal([FromBody] SaveFinalPembayaranPiutangRequest request)
            {
                if (string.IsNullOrEmpty(request.noBukti))
                    return Json(new { success = false, message = "Nomor bukti tidak boleh kosong." });

                try
                {
                    var result = await Mediator.Send(new SaveFinalPembayaranPiutangCommand
                    {
                        NoBukti = request.noBukti
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

            [HttpGet]
            public async Task<IActionResult> GetTotalPembayaran(string no_bukti, string PiutangDK)
            {
                if (string.IsNullOrEmpty(no_bukti))
                {
                    return BadRequest("No Bukti tidak boleh kosong.");
                }

                // Panggil handler baru yang kita buat
                var total = await Mediator.Send(new GetTotalPembayaranQuery { no_bukti = no_bukti, PiutangDK = PiutangDK });
                
                // Kembalikan totalnya sebagai JSON
                return Json(new { totalPembayaran = total });
            }

             public async Task<IActionResult> Lihat(string kodeCabang, string nomorBukti)
        {
            var userCabang = Request.Cookies["UserCabang"];
            string glDept = (!string.IsNullOrEmpty(userCabang) && userCabang.Length >= 2) 
                ? userCabang.Substring(userCabang.Length - 2) 
                : userCabang;

            var databaseName = Request.Cookies["DatabaseValue"];
            var viewModel = new EntriPenyelesaianPiutangViewModel();
            var jenisPenyelesaian = "BM";
            var headerDto = await Mediator.Send(new GetHeaderPenyelesaianUtangByIdQuery { KodeCabang = kodeCabang, NomorBukti = nomorBukti });
            if (headerDto == null) return NotFound();
             ViewBag.FlagPembayaranOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nota", Value = "NOTA" },
                new SelectListItem { Text = "Akun", Value = "AKUN" }


            };
            
             ViewBag.JenisPenyelesaianOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bukti", Value = "BM" }
            };

            var akunlist = await Mediator.Send(new GetAllCoaQuery { KodeCabang = glDept });
            viewModel.PenyelesaianHeader.JenisPenyelesaian = jenisPenyelesaian;
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
            var COAList = await Mediator.Send(new GetAllCoaQuery { KodeCabang = glDept });
             ViewBag.COAoptions = COAList.Select(c => new SelectListItem
            {
                Value = c.Kode.Trim(),
                Text = $"{c.Kode.Trim()} - {c.Nama.Trim()}",
               
            }).ToList();

            viewModel.PenyelesaianHeader = headerDto;

            // --- TAMBAHKAN BAGIAN INI ---
            // Ambil data detail yang sudah ada
            var detailListDto = await Mediator.Send(new GetAllEntriPenyelesaianPiutangQuery { NoBukti = headerDto.NomorBukti });

            // Mapping dari DTO ke ViewModel Item
            viewModel.PembayaranItems = Mapper.Map<List<PenyelesaianPiutangItem>>(detailListDto);

            return PartialView(viewModel);
        }

         [HttpPost]
        public async Task<IActionResult> UpdateFinal([FromBody] EntriPenyelesaianPiutangViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.No > 0) 
            {
                // JANGAN pakai Mapper.Map, pakai ini:
              
                var command = Mapper.Map<UpdateFinalPenyelesaianPiutangCommand>(model);
                
                await Mediator.Send(command);
            }
          
            return Json(new { success = true });
        }
          // Action untuk menghapus data
        [HttpGet]
        public async Task<IActionResult> ProsesUlang(string id)
        {
            await Mediator.Send(new ProsesUlangPembayaranPiutangCommand { NoBukti = id });
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHeader([FromBody] DeleteHeaderPenyelesaianUtangCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result)
                {
                    return Json(new { success = true, message = "Data berhasil dihapus." });
                }
                else
                {
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNextNomorBukti(string kodeCabang, string jenisPenyelesaian, DateTime? tanggalBukti)
        {
            // Jika tanggal kosong, jangan generate (kembalikan kosong atau error)
            if (tanggalBukti == null)
            {
                return Json(new { success = false, message = "Tanggal belum dipilih" });
            }

            var nextNomor = await Mediator.Send(new GetNextNomorBuktiQuery
            {
                KodeCabang = kodeCabang,
                JenisPenyelesaian = jenisPenyelesaian,
                // Ambil Bulan & Tahun dari tanggal inputan user
                Bulan = tanggalBukti.Value.Month,
                Tahun = tanggalBukti.Value.Year
            });

            return Json(new { success = true, nomorBukti = nextNomor });
        }

        [HttpGet]
        public async Task<IActionResult> Cetak(string kodeCabang, string nomorBukti)
        {
            if (string.IsNullOrEmpty(nomorBukti))
                return BadRequest("Nomor bukti tidak boleh kosong.");

            // 1. Ambil Header
            var header = await Mediator.Send(new GetHeaderPenyelesaianUtangByIdQuery 
            { 
                KodeCabang = kodeCabang, 
                NomorBukti = nomorBukti 
            });

            if (header == null)
                return NotFound($"Bukti dengan nomor {nomorBukti} tidak ditemukan.");

            // 2. Ambil Detail (Gunakan Query yang sudah ada untuk list detail)
            var details = await Mediator.Send(new GetAllEntriPenyelesaianPiutangQuery 
            { 
                NoBukti = nomorBukti 
            });

            // 3. Masukkan ke ViewModel
            var viewModel = new EntriPenyelesaianPiutangViewModel
            {
                    PenyelesaianHeader = header,
                    PembayaranItems = Mapper.Map<List<PenyelesaianPiutangItem>>(details)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertLabaRugiKurs([FromBody] EntriPenyelesaianPiutangViewModel model)
        {
            try 
            {
                // 1. Ambil & Cut Kode Cabang dari Cookie (Misal: JK50 -> 50)
                var userCabang = Request.Cookies["UserCabang"] ?? "";
                string glDeptStr = (userCabang.Length >= 2) ? userCabang.Substring(userCabang.Length - 2) : userCabang;

                // 2. Cari Kode Akun di abb_kodeakun_valas via Mediator
                var akunValas = await Mediator.Send(new GetLabaRugiKursQuery { gl_dept = glDeptStr });

                if (string.IsNullOrEmpty(akunValas))
                    return Json(new { success = false, message = $"Setting Akun Valas cabang {glDeptStr} belum ditemukan." });

                // 3. Siapkan Command untuk Insert Detail (Flag AKUN)
                var command = new CreatePenyelesaianPiutangCommand
                {
                    NoBukti = model.NoBukti,
                    KodeAkun = akunValas.Trim(),
                    TotalBayarOrg = 0, // Original selalu 0
                    TotalBayarRp = model.TotalBayarRp, // Selisih Rupiah dari JS
                    DebetKredit = model.DebetKredit,   // D/K Penyeimbang dari JS
                    FlagPembayaran = "AKUN",
                    KodeMataUang = "001", // Penyesuaian selalu IDR
                    // Kurs = 1,
                    KodeUserInput = CurrentUser.UserId
                };

                await Mediator.Send(command);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}