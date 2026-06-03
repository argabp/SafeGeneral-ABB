using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.VoucherBanks.Commands;
using ABB.Application.PostingVoucherBank.Commands;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.PostingVoucherBank.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Common.Interfaces;

namespace ABB.Web.Modules.PostingVoucherBank
{
    public class PostingVoucherBankController : AuthorizedBaseController
    {

        private readonly IDbContextPstNota _context;

        public PostingVoucherBankController(IDbContextPstNota context)
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
        public async Task<ActionResult> GetPostingVoucherBank([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // --- PERBAIKI BAGIAN INI ---
             var kodeCabang = Request.Cookies["UserCabang"];
            var ds = await Mediator.Send(new GetAllVoucherBankByFlagQuery
            {
                SearchKeyword = searchKeyword,
                KodeCabang = kodeCabang,
                FlagFinal = true,
                DatabaseName = Request.Cookies["DatabaseName"]
            });
            // -------------------------

            return Json(ds.ToDataSourceResult(request));
        }
        
       public async Task<ActionResult> Posting([FromBody] List<VoucherBankDto> model)
        {
            try
            {
                // 1. Validasi Periode (Dilakukan sebelum proses Posting)
                foreach (var item in model)
                {
                    if (item.TanggalVoucher.HasValue)
                    {
                        // PERBAIKAN: Konversi ke waktu lokal agar 30 April 17:00 UTC kembali menjadi 01 Mei 00:00 Lokal
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
                
                // --- PROSES POSTING COMMAND ---
                var command = new PostingVoucherBankCommand()
                {
                    Data = model.Select(m => m.NoVoucher).ToList(),
                    UserId = CurrentUser.UserId 
                };

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