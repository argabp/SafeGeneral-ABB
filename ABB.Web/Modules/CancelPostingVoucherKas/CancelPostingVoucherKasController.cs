using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.CancelPostingVoucherKas.Commands;
using ABB.Application.VoucherKass.Queries;
using ABB.Application.CancelPostingVoucherKas.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelPostingVoucherKas
{
    public class CancelPostingVoucherKasController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
         [HttpPost] // Action untuk Kendo Grid biasanya POST
        public async Task<ActionResult> GetCancelPostingVoucherKas([DataSourceRequest] DataSourceRequest request)
        {
           var ds = await Mediator.Send(new GetAllVoucherKasByFlagCancelQuery
            {
                DatabaseName = Request.Cookies["DatabaseName"],
                FlagPosting = true
            });
            return Json(ds.ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> CancelPosting([FromBody] List<VoucherKasDto> model)
        {
            try
            {
                // --- PERBAIKI BAGIAN INI ---
                var command = new CancelPostingVoucherKasCommand()
                {
                    // 1. Hapus 'DatabaseName' karena sudah tidak ada di Command
                    // DatabaseName = Request.Cookies["DatabaseValue"],
                    
                    // 2. Ubah List<VoucherKasDto> menjadi List<string> berisi NoVoucher
                    Data = model.Select(m => m.NoVoucher).ToList() 
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