using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;
// using ABB.Application.Coas.Queries; // Tidak dipakai karena akses langsung db
using ABB.Application.LaporanBukuBesars.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Domain.Entities;

namespace ABB.Web.Modules.LaporanBukuBesar
{
    public class LaporanBukuBesarController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;
        
        // [PERBAIKAN 1]: Deklarasikan variabel _context
        private readonly IDbContextPstNota _context; 

        // [PERBAIKAN 2]: Tambahkan IDbContextPstNota ke dalam parameter Constructor
        public LaporanBukuBesarController(IReportGeneratorService reportGeneratorService, IDbContextPstNota context)
        {
            _reportGeneratorService = reportGeneratorService;
            _context = context; // Sekarang 'context' dikenali dari parameter di atas
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            // Load Cabang Info
            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var userCabang = cabangList.FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;

            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;

            return View();
        }

        // Dropdown Kode Akun
       [HttpGet]
        public async Task<IActionResult> GetCoaList(string text)
        {
            // Ambil kode cabang dari cookie login
            // var kodeCabang = Request.Cookies["UserCabang"]?.Trim();

            var kodeCabangCookie = Request.Cookies["UserCabang"]?.Trim();
            

            string glDept = null;
            if (!string.IsNullOrEmpty(kodeCabangCookie) && kodeCabangCookie.Length >= 2)
            {
                glDept = kodeCabangCookie.Substring(kodeCabangCookie.Length - 2);
            }

            var query = _context.Set<Coa>()
                .AsNoTracking()
                .AsQueryable();

            // Filter berdasarkan kode cabang login
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                query = query.Where(x => x.gl_dept == glDept);
                // kalau nama kolom beda, ganti:
                // x.KodeCabang == kodeCabang
            }

            // Filter dari input user (autocomplete / search)
            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(x =>
                    x.gl_kode.Contains(text) ||
                    x.gl_nama.Contains(text));
            }

            // Ambil maksimal 50 data
            var list = await query
                .OrderBy(x => x.gl_kode)
                .Take(50)
                .Select(x => new
                {
                    Kode = x.gl_kode.Trim(),
                    Nama = x.gl_nama.Trim(),
                    Display = x.gl_kode.Trim() + " - " + x.gl_nama.Trim()
                })
                .ToListAsync();

            return Json(list);
        }


        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanBukuBesarFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan.");

                var query = new GetLaporanBukuBesarQuery
                {
                    DatabaseName = databaseName,
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    AkunAwal = model.AkunAwal,
                    AkunAkhir = model.AkunAkhir,
                    UserLogin = user
                };

                var reportTemplate = await Mediator.Send(query);

                if (string.IsNullOrEmpty(reportTemplate))
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanBukuBesar.pdf",
                    reportTemplate,
                    user,
                    Orientation.Landscape, 
                    5, 5, 5, 5,
                    PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }

    public class LaporanBukuBesarFilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string AkunAwal { get; set; }
        public string AkunAkhir { get; set; }
    }
}