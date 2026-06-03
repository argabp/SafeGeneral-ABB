using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.PostingVoucherKas.Commands;
using ABB.Application.VoucherKass.Queries;
using ABB.Application.PostingVoucherKas.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Common.Interfaces;

namespace ABB.Web.Modules.PostingVoucherKas
{
    public class PostingVoucherKasController : AuthorizedBaseController
    {

        private readonly IDbContextPstNota _context;

        public PostingVoucherKasController(IDbContextPstNota context)
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
        
         [HttpPost] // Action untuk Kendo Grid biasanya POST
        public async Task<ActionResult> GetPostingVoucherKas([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // --- PERBAIKI BAGIAN INI ---
             var kodeCabang = Request.Cookies["UserCabang"];
            var ds = await Mediator.Send(new GetAllVoucherKasByFlagQuery
            {
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = true,
                DatabaseName = Request.Cookies["DatabaseName"]
            });
            // -------------------------

            return Json(ds.ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> Posting([FromBody] List<VoucherKasDto> model)
        {
            try
            {
                // 1. Validasi Periode (Dilakukan sebelum proses Posting)
                foreach (var item in model)
                {
                    if (item.TanggalVoucher.HasValue)
                    {
                        // PERBAIKAN: Konversi ke waktu lokal agar zona waktu UTC tidak membuat tanggal mundur 1 hari
                        DateTime localDate = item.TanggalVoucher.Value.ToLocalTime();

                        short blnPrdInput = (short)localDate.Month;
                        decimal thnPrdInput = (decimal)localDate.Year;

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
                                Message = $"Voucher {item.NoVoucher} (Bulan {namaBulan}/{thnPrdInput}) tidak bisa di-posting karena periode tersebut sudah di-close." 
                            });
                        }
                    }
                }

                // --- PERBAIKI BAGIAN INI ---
                var command = new PostingVoucherKasCommand()
                {
                    // 1. Hapus 'DatabaseName' karena sudah tidak ada di Command
                    // DatabaseName = Request.Cookies["DatabaseValue"],
                    
                    // 2. Ubah List<VoucherKasDto> menjadi List<string> berisi NoVoucher
                    Data = model.Select(m => m.NoVoucher).ToList(),
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