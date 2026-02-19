using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.VoucherBanks.Commands;
using ABB.Application.CancelPostingVoucherBank.Commands;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.CancelPostingVoucherBank.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingVoucherBank
{
    public class CancelPostingVoucherBankController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
         [HttpPost] // Action untuk Kendo Grid biasanya POST
        public async Task<ActionResult> GetCancelPostingVoucherBank([DataSourceRequest] DataSourceRequest request)
        {
        var kodeCabang = Request.Cookies["UserCabang"];
           var ds = await Mediator.Send(new GetAllVoucherBankByFlagCancelQuery
            {
                DatabaseName = Request.Cookies["DatabaseName"],
                FlagPosting = true,
                KodeCabang = kodeCabang
            });
            return Json(ds.ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> CancelPosting([FromBody] List<VoucherBankDto> model)
        {
            try
            {
                // --- PERBAIKI BAGIAN INI ---
                var command = new CancelPostingVoucherBankCommand()
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