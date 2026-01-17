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
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanBukuBesars117.Queries; // Namespace Baru
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Domain.Entities; 


namespace ABB.Web.Modules.LaporanBukuBesar117
{
    public class LaporanBukuBesar117Controller : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly IDbContextPstNota _context; // Inject Context buat ambil coa117 buat dropdown

        public LaporanBukuBesar117Controller(IReportGeneratorService reportGeneratorService, IDbContextPstNota context)
        {
            _reportGeneratorService = reportGeneratorService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            var kodeCabangCookie = Request.Cookies["UserCabang"];

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
            var userCabang = cabangList.FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;

            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;

            return View();
        }

        // Dropdown Kode Akun (KHUSUS COA 104)
        [HttpGet]
        public async Task<IActionResult> GetCoaList(string text)
        {
            // 1. Siapkan Query
            var query = _context.Set<Coa117>()
                .AsNoTracking()
                .AsQueryable();

            // 2. Jika ada text pencarian (user mengetik)
            if (!string.IsNullOrEmpty(text))
            {
                // Cari berdasarkan Kode ATAU Nama
                query = query.Where(x => 
                    x.gl_kode.Contains(text) || 
                    x.gl_nama.Contains(text));
            }

            // 3. Ambil data secukupnya saja (misal 50 teratas)
            // Ini kuncinya biar TIDAK BERAT
            var list = await query
                .OrderBy(x => x.gl_kode)
                .Take(50) 
                .Select(x => new 
                {
                    Kode = x.gl_kode.Trim(),
                    Nama = x.gl_nama.Trim(),
                    Display = $"{x.gl_kode.Trim()} - {x.gl_nama.Trim()}"
                })
                .ToListAsync();

            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanBukuBesar117FilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanBukuBesar117Query
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

                // Kita pakai template PDF yang sama (LaporanBukuBesar.pdf) karena formatnya sama
                // Cuma beda isinya saja
                _reportGeneratorService.GenerateReport(
                    "LaporanBukuBesar117.pdf",
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

    public class LaporanBukuBesar117FilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string AkunAwal { get; set; }
        public string AkunAkhir { get; set; }
    }
}