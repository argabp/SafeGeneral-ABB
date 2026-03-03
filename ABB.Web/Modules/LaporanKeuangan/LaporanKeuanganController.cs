using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using DinkToPdf;
using ABB.Application.LaporanKeuangan.Queries;
using ClosedXML.Excel;
using System.IO;

namespace ABB.Web.Modules.LaporanKeuangan
{
    public class LaporanKeuanganController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanKeuanganController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanKeuanganFilterDto model)
        {
            try
            {
                var user = CurrentUser.UserId;
                LaporanKeuanganResponse response = null;
                string outputFileName = "";

                // PERCABANGAN LOGIKA BERDASARKAN TIPE LAPORAN
                switch (model.TipeLaporan)
                {
                    case "NERACA":
                        response = await Mediator.Send(new GetLaporanNeracaQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        outputFileName = "LaporanNeraca.pdf";
                        break;
                    case "LABARUGI":
                        response = await Mediator.Send(new GetLaporanLabaRugiQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        outputFileName = "LaporanLabaRugi.pdf";
                        break;
                    case "ARUSKAS":
                        response = await Mediator.Send(new GetLaporanArusKasQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        outputFileName = "LaporanArusKas.pdf";
                        break;
                    default:
                        throw new Exception("Tipe Laporan tidak valid.");
                }

                if (response == null || string.IsNullOrEmpty(response.HtmlString))
                    throw new Exception("Data tidak ditemukan atau Template kosong.");

                // GENERATE PDF
                _reportGeneratorService.GenerateReport(
                    outputFileName, 
                    response.HtmlString, // Ambil HtmlString dari Response
                    user,
                    Orientation.Portrait,
                    5, 5, 5, 5,
                    PaperKind.A4
                );

                return Ok(new { Status = "OK", Data = user, Filename = outputFileName });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanKeuanganFilterDto model)
        {
            try
            {
                LaporanKeuanganResponse response = null;

                // Tarik data yang sama persis dengan PDF
                switch (model.TipeLaporan)
                {
                    case "NERACA":
                        response = await Mediator.Send(new GetLaporanNeracaQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        break;
                    case "LABARUGI":
                        response = await Mediator.Send(new GetLaporanLabaRugiQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        break;
                    case "ARUSKAS":
                        response = await Mediator.Send(new GetLaporanArusKasQuery { JenisPeriode = model.JenisPeriode, Bulan = model.Bulan, Tahun = model.Tahun });
                        break;
                    default: throw new Exception("Tipe Laporan tidak valid.");
                }

                if (response == null || response.ExcelData == null) throw new Exception("Data tidak ditemukan.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(model.TipeLaporan);

                    // --- 1. KOP LAPORAN ---
                    worksheet.Cell(1, 1).Value = $"LAPORAN {model.TipeLaporan}";
                    worksheet.Range("A1:C1").Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    
                    string labelPeriode = model.JenisPeriode == "BULANAN" ? $"Bulan {model.Bulan} " : "";
                    worksheet.Cell(2, 1).Value = $"PERIODE: {labelPeriode}Tahun {model.Tahun}";
                    worksheet.Range("A2:C2").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // --- 2. SETUP KOLOM ---
                    worksheet.Column(1).Width = 50; // Deskripsi
                    worksheet.Column(2).Width = 20; // Tahun Ini
                    worksheet.Column(3).Width = 20; // Tahun Lalu

                    int currentRow = 4;

                    // --- 3. LOOP DATA DARI TEMPLATE DATABASE ---
                    foreach (var item in response.ExcelData)
                    {
                        if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                        {
                            currentRow++;
                            continue;
                        }

                        var cellDesc = worksheet.Cell(currentRow, 1);
                        var cellIni = worksheet.Cell(currentRow, 2);
                        var cellLalu = worksheet.Cell(currentRow, 3);

                        // Header Utama (Aset, Kewajiban beserta label Tahun)
                        if (item.IsHeaderKolom)
                        {
                            cellDesc.Value = item.Deskripsi;
                            cellIni.Value = item.HeaderTahunIni;
                            cellLalu.Value = item.HeaderTahunLalu;
                            
                            var headerRange = worksheet.Range(currentRow, 1, currentRow, 3);
                            headerRange.Style.Font.SetBold();
                            headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            cellIni.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            cellLalu.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            currentRow++;
                            continue;
                        }

                        // Isi Deskripsi Biasa
                        cellDesc.Value = item.Deskripsi;

                        // Isi Angka
                        if (item.NilaiIni.HasValue) {
                            cellIni.Value = item.NilaiIni.Value;
                            cellIni.Style.NumberFormat.Format = "#,##0.00";
                        }
                        if (item.NilaiLalu.HasValue) {
                            cellLalu.Value = item.NilaiLalu.Value;
                            cellLalu.Style.NumberFormat.Format = "#,##0.00";
                        }

                        // --- 4. LOGIKA INDENTASI & FONT BOLD (MIRROR PDF) ---
                        if (item.TipeBaris == "HEADING")
                        {
                            cellDesc.Style.Font.SetBold();
                            // Beri spasi indentasi berdasarkan level
                            if (item.Level > 1) cellDesc.Style.Alignment.SetIndent(item.Level - 1);
                        }
                        else if (item.TipeBaris == "DETAIL")
                        {
                            cellDesc.Style.Alignment.SetIndent(item.Level);
                        }
                        else if (item.TipeBaris == "TOTAL")
                        {
                            cellDesc.Style.Font.SetBold();
                            cellIni.Style.Font.SetBold();
                            cellLalu.Style.Font.SetBold();
                            
                            cellDesc.Style.Alignment.SetIndent(item.Level > 1 ? item.Level : 0);

                            // Garis Atas
                            cellIni.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            cellLalu.Style.Border.TopBorder = XLBorderStyleValues.Thin;

                            // Garis Bawah Ganda untuk Grand Total (Level 1)
                            if (item.Level == 1) 
                            {
                                cellIni.Style.Border.BottomBorder = XLBorderStyleValues.Double;
                                cellLalu.Style.Border.BottomBorder = XLBorderStyleValues.Double;
                            }
                        }

                        currentRow++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return Ok(new { Status = "OK", FileName = $"Laporan_{model.TipeLaporan}_{DateTime.Now:yyyyMMdd}.xlsx", FileData = Convert.ToBase64String(stream.ToArray()) });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.Message });
            }
        }

    
    }

    // UPDATE DTO AGAR SESUAI DENGAN KIRIMAN JAVASCRIPT
    public class LaporanKeuanganFilterDto
    {
        public string TipeLaporan { get; set; }
        public string JenisPeriode { get; set; } // Baru
        public int Bulan { get; set; }           // Baru (int)
        public int Tahun { get; set; }           // Baru (int)
    }
}