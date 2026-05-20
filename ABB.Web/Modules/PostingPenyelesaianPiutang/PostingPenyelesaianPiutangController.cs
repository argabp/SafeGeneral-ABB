using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;
// 🎯 DIPERBAIKI: Using statement yang benar
using ABB.Application.PostingPenyelesaianPiutang.Commands;
using ABB.Application.PostingPenyelesaianPiutang.Queries;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;
using ABB.Application.Common.Interfaces;


namespace ABB.Web.Modules.PostingPenyelesaianPiutang
{
    public class PostingPenyelesaianPiutangController : AuthorizedBaseController
    {

        private readonly IDbContextPstNota _context;

        public PostingPenyelesaianPiutangController(IDbContextPstNota context)
        {
            _context = context;
        }
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
           [HttpPost]
        // 🎯 DIPERBAIKI: Tambahkan parameter untuk menerima filter dari Kendo Grid
        public async Task<ActionResult> GetPostingPenyelesaianPiutang([DataSourceRequest] DataSourceRequest request)
        {
            var kodeCabang = Request.Cookies["UserCabang"];
            // 🎯 DIPERBAIKI: Panggil Query yang benar
            var data = await Mediator.Send(new GetAllPenyelesaianPiutangByFlagQuery
            {
                DatabaseName = Request.Cookies["DatabaseName"],
                KodeCabang = kodeCabang
            });

            return Json(await data.ToDataSourceResultAsync(request));
        }
        
        
       [HttpPost] // Action untuk tombol "Posting"
        // 🎯 DIPERBAIKI: Terima DTO yang benar
        public async Task<ActionResult> Posting([FromBody] List<HeaderPenyelesaianUtangDto> model)
        {
            try
            {

                // 1. Validasi Periode (Dilakukan sebelum proses Posting)
                    foreach (var item in model)
                    {
                        if (item.Tanggal.HasValue)
                        {
                            short blnPrdInput = (short)item.Tanggal.Value.Month;
                            decimal thnPrdInput = (decimal)item.Tanggal.Value.Year;

                            bool isPeriodeClosed = _context.EntriPeriode
                                .Any(p => p.BlnPrd == blnPrdInput && 
                                        p.ThnPrd == thnPrdInput && 
                                        p.FlagClosing == "N"); // "N" = Tutup

                            if (isPeriodeClosed)
                            {
                                string namaBulan = blnPrdInput.ToString("00");
                                // Langsung return error jika ditemukan ada yang di-close
                                return Ok(new { 
                                    Status = "ERROR", 
                                    Message = $"Nomor {item.NomorBukti} (Bulan {namaBulan}/{thnPrdInput}) tidak bisa di-posting karena periode tersebut sudah di-close." 
                                });
                            }
                        }
                    }


                // --- PERBAIKI BAGIAN INI ---
                var command = new PostingPenyelesaianPiutangCommand()
                {
                    // 1. Hapus 'DatabaseName' karena sudah tidak ada di Command
                    // DatabaseName = Request.Cookies["DatabaseValue"],
                    
                    // 2. Ubah List<VoucherBankDto> menjadi List<string> berisi NoVoucher
                    Data = model.Select(m => m.NomorBukti).ToList(),
                    UserId = CurrentUser.UserId 
                };
                // -------------------------

                await Mediator.Send(command);

                return Ok(new { Status = "OK"});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}