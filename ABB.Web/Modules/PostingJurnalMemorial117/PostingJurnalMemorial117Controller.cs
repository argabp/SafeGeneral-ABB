using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ABB.Application.JurnalMemorial117.Queries;
using ABB.Application.JurnalMemorial117.Commands;
using ABB.Web.Modules.JurnalMemorial117.Models; // Pastikan namespace DTO benar

namespace ABB.Web.Modules.PostingJurnalMemorial117
{
    public class PostingJurnalMemorial117Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetPostingJurnalMemorial117([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            var userCabang = Request.Cookies["UserCabang"];

            // Panggil Query: Minta data yang FlagPosting = FALSE
            var data = await Mediator.Send(new GetJurnalMemorial117ByFlagQuery
            {
                FlagPosting = false, // Ambil yang belum posting
                SearchKeyword = searchKeyword,
                KodeCabang = userCabang
            });

            return Json(await data.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        public async Task<ActionResult> Posting([FromBody] List<JurnalMemorial117Dto> model)
        {
            try
            {
                if (model == null || !model.Any())
                {
                    return Ok(new { Status = "ERROR", Message = "Tidak ada data yang dipilih." });
                }

                // Ambil User ID (potong jika perlu)
                string userId = CurrentUser.UserId ?? "SYSTEM";
                if (userId.Length > 25) userId = userId.Substring(0, 25);

                // Buat Command
                var command = new PostingJurnalMemorial117Command()
                {
                    KodeCabang = Request.Cookies["UserCabang"], // Ambil cabang dari cookie
                    KodeUserUpdate = userId,
                    
                    // Ambil list NoVoucher dari data yang dikirim grid
                    NoVouchers = model.Select(m => m.NoVoucher).ToList() 
                };

                // Kirim Command
                await Mediator.Send(command);

                return Ok(new { Status = "OK" });
            }
            catch (Exception e)
            {
                return Ok(new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}