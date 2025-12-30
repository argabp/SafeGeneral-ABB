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


namespace ABB.Web.Modules.PostingPenyelesaianPiutang
{
    public class PostingPenyelesaianPiutangController : AuthorizedBaseController
    {
        
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
            // 🎯 DIPERBAIKI: Panggil Query yang benar
            var data = await Mediator.Send(new GetAllPenyelesaianPiutangByFlagQuery
            {
                DatabaseName = Request.Cookies["DatabaseName"]
            });

            return Json(await data.ToDataSourceResultAsync(request));
        }
        
        
       [HttpPost] // Action untuk tombol "Posting"
        // 🎯 DIPERBAIKI: Terima DTO yang benar
        public async Task<ActionResult> Posting([FromBody] List<HeaderPenyelesaianUtangDto> model)
        {
            try
            {
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