using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.Base;
using Kendo.Mvc.UI;          // <--- TAMBAHKAN INI 
using Kendo.Mvc.Extensions;
using System.Linq;
using ABB.Application.EditJurnals104.Queries;
using ABB.Application.EditJurnals104.Commands;
using ABB.Application.Coas.Queries;
using ABB.Application.MataUangs.Queries;

namespace ABB.Web.Modules.EditJurnal104
{
    public class EditJurnal104Controller : AuthorizedBaseController
    {
        // 1. TAMPILAN HALAMAN UTAMA (INDEX)
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseValue"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        // --- TAMBAHAN BARU DI SINI ---
        // Digunakan oleh Kendo Window untuk me-load form Edit.cshtml
        [HttpGet]
        public IActionResult Edit(string noBukti)
        {
            ViewBag.NoBukti = noBukti;
            return PartialView(); 
        }
        // -----------------------------

        // 2. INQUIRY / PENCARIAN HEADER JURNAL
        // Dipanggil saat user menekan tombol "Cari" di layar parameter
        // Tambahkan [DataSourceRequest] DataSourceRequest request
        [HttpPost]
        public async Task<IActionResult> GetInquiryData([DataSourceRequest] DataSourceRequest request, GetInquiryEditJurnal104Query query)
        {
            try
            {
                // Set database name dari cookie jika diperlukan di handler
                query.DatabaseName = Request.Cookies["DatabaseValue"];

                var result = await Mediator.Send(query);
                
                // --- KEAJAIBAN KENDO ADA DI BARIS INI ---
                // Ini akan otomatis menghitung Total, memotong Paging (20 baris), 
                // dan membungkus JSON-nya agar Kendo Grid langsung paham!
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                // Jika error, kembalikan format Kendo yang kosong
                return Json(new List<InquiryJurnal104Dto>().ToDataSourceResult(request));
            }
        }

        // 3. GET DETAIL JURNAL
        // Dipanggil saat user menekan tombol "Edit" pada Grid Inquiry
        [HttpGet]
        public async Task<IActionResult> GetDetailJurnal(string noBukti)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noBukti))
                    throw new Exception("Nomor Bukti tidak valid.");

                var databaseName = Request.Cookies["DatabaseValue"];

                var query = new GetDetailEditJurnal104Query 
                { 
                    DatabaseName = databaseName,
                    NoBukti = noBukti 
                };

                var result = await Mediator.Send(query);

                return Json(new { Status = "OK", Data = result });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // 4. SIMPAN PERUBAEMIT EDIT JURNAL
        // Dipanggil saat user menekan tombol "Simpan" setelah mengedit detail Kendo Grid
        [HttpPost]
        public async Task<IActionResult> SaveEditJurnal([FromBody] UpdateJurnal104Command command)
        {
            try
            {
                // Sisipkan data user yang login untuk audit trail di kolom kd_user_update
                command.UserId = CurrentUser.UserId;
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                // Validasi awal di Controller (Opsional, tapi bagus untuk keamanan)
                if (command.Details == null || command.Details.Count == 0)
                    throw new Exception("Detail jurnal tidak boleh kosong.");

                // Eksekusi proses hapus & insert baru via MediatR
                var result = await Mediator.Send(command);

                return Json(new { Status = "OK", Message = "Jurnal berhasil diperbarui!" });
            }
            catch (Exception ex)
            {
                // Tangkap error balance jurnal 
                return Json(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCoa([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            try
            {
                // Tarik semua data COA
                var result = await Mediator.Send(new GetAllCoaQuery()); 

                // Filter manual jika user ngetik di ComboBox
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    searchKeyword = searchKeyword.ToLower();
                    result = result.Where(x => 
                        (x.Kode != null && x.Kode.ToLower().Contains(searchKeyword)) || 
                        (x.Nama != null && x.Nama.ToLower().Contains(searchKeyword))
                    ).ToList();
                }

                // ToDataSourceResult akan otomatis memotong data menjadi 25 baris (Paging)
                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new List<object>().ToDataSourceResult(request)); // Hindari crash Kendo
            }
        }

        // ==========================================
        // TAMBAHAN BARU: 6. GET KURS MATA UANG
        // ==========================================
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