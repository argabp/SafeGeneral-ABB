using System;
using System.Collections.Generic; // <-- UNTUK MEMPERBAIKI ERROR List<>
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using MediatR;

// --- TAMBAHKAN INI UNTUK MEMPERBAIKI ERROR DataSourceRequest ---
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI; 
// ---------------------------------------------------------------

using ABB.Application.CancelTutupBulan.Queries;
using ABB.Application.CancelTutupBulan.Commands;
using ABB.Application.ProsesTutupBulan.Dtos; // Pastikan namespace DTO benar


namespace ABB.Web.Modules.CancelTutupBulan
{
    public class CancelTutupBulanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

       [HttpPost]
        public async Task<IActionResult> GetDaftarPeriodeCancel([DataSourceRequest] DataSourceRequest request)
        {
            // Panggil Query yang baru (GetDaftarPeriodeQuery)
            var data = await Mediator.Send(new GetPeriodeCancelQuery());
            
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // Action ProcessClosing tetap sama, menerima BODY berupa object Command
        [HttpPost]
        public async Task<IActionResult> ProcessCancelClosing([FromBody] List<CancelTutupBulanCommand> commands) 
        {
            // Kita ubah parameter jadi List, jaga-jaga kalau user centang banyak sekaligus
            if (commands == null || !commands.Any()) return BadRequest("Tidak ada data dipilih");

            try
            {
                foreach(var cmd in commands)
                {
                    cmd.UserUpdate = CurrentUser.UserId;
                    await Mediator.Send(cmd);
                }
                return Json(new { success = true, message = "Cancel Proses Tutup Bulan Berhasil!" });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}