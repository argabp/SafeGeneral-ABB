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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.Cabangs.Queries;
using ABB.Application.LaporanJurnalHarian117s117.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Application.JenisTransaksis.Queries;
using ClosedXML.Excel;
using System.IO;


namespace ABB.Web.Modules.LaporanJurnalHarian117
{
    public class LaporanJurnalHarian117Controller : AuthorizedBaseController
    {

         private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanJurnalHarian117Controller(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        private string GetCleanCabangCookie() 
        {
            return Request.Cookies["UserCabang"]?.Replace("%20", " ").Trim() ?? "";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            
            // --- 2. PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 3. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }
            ViewBag.IsPusat = isPusat;
            // ------------------------

            var cabangList = await Mediator.Send(new GetCabangsQuery { DatabaseName = databaseName });
           
            var userCabang = cabangList
                .FirstOrDefault(c => string.Equals(c.kd_cb.Trim(), kodeCabangCookie?.Trim(), StringComparison.OrdinalIgnoreCase));
            
            string displayCabang = userCabang != null 
                ? $"{userCabang.kd_cb.Trim()} - {userCabang.nm_cb.Trim()}" 
                : kodeCabangCookie;
           
            ViewBag.UserCabangValue = kodeCabangCookie; 
            ViewBag.UserCabangText = displayCabang;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetKodeCabang(string tipe)
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            
            // --- 4. PAKAI FUNGSI PEMBERSIH ---
            var kodeCabangCookie = GetCleanCabangCookie();

            // --- 5. LOGIKA PS10 ---
            bool isPusat = false;
            if (!string.IsNullOrEmpty(kodeCabangCookie))
            {
                isPusat = (kodeCabangCookie.Trim().ToUpper() == "PS10");
            }

            if (string.IsNullOrWhiteSpace(kodeCabangCookie))
                return Json(new List<object>()); // cookie tidak ada → kirim kosong

            var result = await Mediator.Send(new GetCabangsQuery
            {
                DatabaseName = databaseName
            });

            // --- 6. FILTER PAKAI isPusat || ---
            var filtered = result
                .Where(c => isPusat || c.kd_cb?.Trim().ToUpper() == kodeCabangCookie.ToUpper())
                .Select(c => new
                {
                    kd_cb = c.kd_cb.Trim(),
                    nm_cb = c.nm_cb.Trim()
                })
                .ToList(); 

            ViewBag.UserCabang = kodeCabangCookie;

            return Json(filtered);
        }

         [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanJurnalHarian117FilterDto model)
        {
            try
            {
                var query = new GetLaporanJurnalHarian117Query
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    JenisTransaksi = model.JenisTransaksi,
                    UserLogin = CurrentUser.UserId
                };  
                var response = await Mediator.Send(query);

                if (response.RawData == null || !response.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                _reportGeneratorService.GenerateReport(
                    "LaporanJurnalHarian117.pdf",
                    response.HtmlString, // Pakai properti HtmlString
                    CurrentUser.UserId,
                    Orientation.Landscape, 5, 5, 5, 5, PaperKind.Legal
                );
                return Ok(new { Status = "OK", Data = CurrentUser.UserId });
            }
            catch (Exception ex) { return Ok(new { Status = "ERROR", Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanJurnalHarian117FilterDto model)
        {
            try
            {
                var query = new GetLaporanJurnalHarian117Query
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    KodeCabang = model.KodeCabang,
                    PeriodeAwal = model.PeriodeAwal,
                    PeriodeAkhir = model.PeriodeAkhir,
                    JenisTransaksi = model.JenisTransaksi,
                    UserLogin = CurrentUser.UserId
                };
                var response = await Mediator.Send(query);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Jurnal Harian 117");

                    // --- KOP SURAT MERGE TENGAH ---
                    worksheet.Cell(1, 1).Value = "LAPORAN JURNAL HARIAN 117";
                    worksheet.Range("A1:H1").Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(2, 1).Value = $"PERIODE : {model.PeriodeAwal} s/d {model.PeriodeAkhir}";
                    worksheet.Range("A2:H2").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // --- SETUP KOLOM LEBAR ---
                    worksheet.Column(1).Width = 5;  worksheet.Column(2).Width = 12;
                    worksheet.Column(3).Width = 15; worksheet.Column(4).Width = 8;
                    worksheet.Column(5).Width = 18; worksheet.Column(6).Width = 18;
                    worksheet.Column(7).Width = 18; worksheet.Column(8).Width = 40;
                    worksheet.Column(8).Style.Alignment.WrapText = true;

                    // --- HEADER TABEL ---
                    int currentRow = 4;
                    var headers = new[] { "No", "Tanggal", "Akun", "Mtu", "Nilai Org", "Debet (IDR)", "Kredit (IDR)", "Keterangan" };
                    for (int i = 0; i < headers.Length; i++) worksheet.Cell(currentRow, i + 1).Value = headers[i];
                    worksheet.Range(currentRow, 1, currentRow, 8).Style.Font.Bold = true;
                    worksheet.Range(currentRow, 1, currentRow, 8).Style.Fill.BackgroundColor = XLColor.LightGray;

                    currentRow++;

                    // --- LOOPING DATA DENGAN GROUPING ---
                    var groupedData = response.RawData.GroupBy(x => x.GlBukti);
                    foreach (var group in groupedData)
                    {
                        // Group Header (No Jurnal)
                        var rowHeader = worksheet.Range(currentRow, 1, currentRow, 8);
                        rowHeader.Merge().Value = $"No. Jurnal : {group.Key}";
                        rowHeader.Style.Font.Bold = true;
                        rowHeader.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                        rowHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        currentRow++;

                        decimal subD = 0, subK = 0;
                        int idx = 1;
                        foreach (var item in group)
                        {
                            worksheet.Cell(currentRow, 1).Value = idx++;
                            worksheet.Cell(currentRow, 2).Value = item.GlTanggal?.ToString("dd/MM");
                            worksheet.Cell(currentRow, 3).Value = item.GlAkun ?? "-";
                            worksheet.Cell(currentRow, 4).Value = item.GlMtu ?? "-";
                            worksheet.Cell(currentRow, 5).Value = item.GlNilaiOrg ?? 0;
                            
                            decimal d = item.GlDk == "D" ? (item.GlNilaiIdr ?? 0) : 0;
                            decimal k = item.GlDk == "K" ? (item.GlNilaiIdr ?? 0) : 0;
                            
                            worksheet.Cell(currentRow, 6).Value = d;
                            worksheet.Cell(currentRow, 7).Value = k;
                            worksheet.Cell(currentRow, 8).Value = item.GlKet ?? "-";

                            // Borders per row
                            worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range(currentRow, 5, currentRow, 7).Style.NumberFormat.Format = "#,##0.00";

                            subD += d; subK += k;
                            currentRow++;
                        }

                        // Subtotal
                        var rowSub = worksheet.Range(currentRow, 1, currentRow, 8);
                        worksheet.Cell(currentRow, 5).Value = "Sub Total :";
                        worksheet.Cell(currentRow, 6).Value = subD;
                        worksheet.Cell(currentRow, 7).Value = subK;
                        rowSub.Style.Font.Bold = true;
                        rowSub.Style.Fill.BackgroundColor = XLColor.FromHtml("#E3F2FD");
                        rowSub.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        rowSub.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range(currentRow, 6, currentRow, 7).Style.NumberFormat.Format = "#,##0.00";

                        currentRow += 2;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return Ok(new { Status = "OK", FileName = "JurnalHarian117.xlsx", FileData = Convert.ToBase64String(stream.ToArray()) });
                    }
                }
            }
            catch (Exception ex) { return Ok(new { Status = "ERROR", Message = ex.Message }); }
        }

        [HttpGet]
            public async Task<IActionResult> GetJenisTransaksi()
        {
            var list = await Mediator.Send(new GetAllJenisTransaksiQuery());

            var result = list.Select(x => new
            {
                NamaJenisTransaksi = $"{x.nama.Trim()} ({x.kode.Trim()})",
                KodeJenisTransaksi = x.kode.Trim()
            }).ToList();

            return Json(result);
        }
    }

      public class LaporanJurnalHarian117FilterDto
    {
        public string KodeCabang { get; set; }
        public string PeriodeAwal { get; set; }
        public string PeriodeAkhir { get; set; }
        public string JenisTransaksi { get; set; }
    }
}