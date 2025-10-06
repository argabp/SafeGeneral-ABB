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

namespace ABB.Web.Modules.PostingVoucherKas
{
    public class PostingVoucherKasController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }
        
         [HttpPost] // Action untuk Kendo Grid biasanya POST
        public async Task<ActionResult> GetPostingVoucherKas([DataSourceRequest] DataSourceRequest request)
        {
            // --- PERBAIKI BAGIAN INI ---
            var ds = await Mediator.Send(new GetAllVoucherKasByFlagQuery
            {
                DatabaseName = Request.Cookies["DatabaseName"]
            });
            // -------------------------

            return Json(ds.ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> Posting([FromBody] List<VoucherKasDto> model)
        {
            try
            {
                // --- PERBAIKI BAGIAN INI ---
                var command = new PostingVoucherKasCommand()
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