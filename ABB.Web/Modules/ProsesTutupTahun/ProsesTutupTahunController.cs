using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI; 

// Tambahkan Namespace ini
using ABB.Application.ProsesTutupTahun.Queries;
using ABB.Application.ProsesTutupTahun.Commands;
using ABB.Application.ProsesTutupTahun.Dtos;

namespace ABB.Web.Modules.ProsesTutupTahun
{
    public class ProsesTutupTahunController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        // Endpoint Load Data Grid
        [HttpPost]
        public async Task<IActionResult> GetDaftarPeriode([DataSourceRequest] DataSourceRequest request)
        {
            var data = await Mediator.Send(new GetDaftarPeriodeTahunQuery());
            return Json(await data.ToDataSourceResultAsync(request));
        }

        // Endpoint Eksekusi Checkbox Grid
        [HttpPost]
        public async Task<IActionResult> ProcessClosing([FromBody] List<ProsesTutupTahunCommand> commands) 
        {
            if (commands == null || !commands.Any()) 
                return BadRequest("Tidak ada data dipilih");

            try
            {
                foreach(var cmd in commands)
                {
                    cmd.UserLogin = CurrentUser.UserId;
                    // Memanggil SP Sakti yang kita buat di pesan sebelumnya
                    await Mediator.Send(cmd);
                }
                return Json(new { success = true, message = "Proses Tutup Tahun Berhasil!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}