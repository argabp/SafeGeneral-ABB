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

                // PERCABANGAN LOGIKA BERDASARKAN TIPE LAPORAN (PDF)
                switch (model.TipeLaporan)
                {
                    case "NERACA":
                    case "NERACA (BULAN)":
                        response = await Mediator.Send(new GetLaporanNeracaQuery 
                        { 
                            TipeLaporan = model.TipeLaporan,
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
                        outputFileName = model.TipeLaporan == "NERACA" ? "LaporanNeraca.pdf" : "LaporanNeracaBulan.pdf";
                        break;

                    case "LABARUGI":
                    case "LABA RUGI (BULAN)": 
                        response = await Mediator.Send(new GetLaporanLabaRugiQuery 
                        { 
                            TipeLaporan = model.TipeLaporan,
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
                        outputFileName = model.TipeLaporan == "LABARUGI" ? "LaporanLabaRugi.pdf" : "LaporanLabaRugiBulan.pdf";
                        break;

                    case "ARUSKAS":
                        response = await Mediator.Send(new GetLaporanArusKasQuery 
                        { 
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
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
                    response.HtmlString, 
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

                switch (model.TipeLaporan)
                {
                    case "NERACA":
                    case "NERACA (BULAN)":
                        response = await Mediator.Send(new GetLaporanNeracaQuery 
                        { 
                            TipeLaporan = model.TipeLaporan,
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
                        break;

                    case "LABARUGI":
                    case "LABA RUGI (BULAN)":
                        response = await Mediator.Send(new GetLaporanLabaRugiQuery 
                        { 
                            TipeLaporan = model.TipeLaporan,
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
                        break;

                    case "ARUSKAS":
                        response = await Mediator.Send(new GetLaporanArusKasQuery 
                        { 
                            JenisPeriode = model.JenisPeriode, 
                            Bulan = model.Bulan, 
                            Tahun = model.Tahun 
                        });
                        break;

                    default: 
                        throw new Exception("Tipe Laporan tidak valid.");
                }

                if (response == null || response.ExcelData == null) throw new Exception("Data tidak ditemukan.");

                using (var workbook = new XLWorkbook())
                {
                    string sheetName = model.TipeLaporan.Replace(" ", "").Replace("(", "").Replace(")", "");
                    if (sheetName.Length > 31) sheetName = sheetName.Substring(0, 31);
                    
                    var worksheet = workbook.Worksheets.Add(sheetName);

                    bool isBulanan = model.TipeLaporan.Contains("(BULAN)");
                    int maxCol = isBulanan ? 4 : 3;

                    // --- 1. KOP LAPORAN ---
                    worksheet.Cell(1, 1).Value = $"LAPORAN {model.TipeLaporan}";
                    worksheet.Range(1, 1, 1, maxCol).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    
                    // PERBAIKAN: Judul periode di atas tabel (baris 2) pakai nama bulan
                    string[] arrayBulan = { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
                    string namaBulan = arrayBulan[model.Bulan - 1]; 
                    
                    worksheet.Cell(2, 1).Value = $"PERIODE: s/d {namaBulan} {model.Tahun}";
                    worksheet.Range(2, 1, 2, maxCol).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // --- 2. SETUP LEBAR KOLOM ---
                    worksheet.Column(1).Width = 50; 
                    worksheet.Column(2).Width = 20; 
                    worksheet.Column(3).Width = 20; 
                    if (isBulanan) worksheet.Column(4).Width = 20; 

                    int currentRow = 4;

                    foreach (var item in response.ExcelData)
                    {
                        if (item.TipeBaris == "SPASI" || item.TipeBaris == "BLANK")
                        {
                            currentRow++;
                            continue;
                        }

                        var cellDesc = worksheet.Cell(currentRow, 1);
                        cellDesc.Value = item.Deskripsi;

                        if (item.IsHeaderKolom)
                        {
                            var headerRange = worksheet.Range(currentRow, 1, currentRow, maxCol);
                            headerRange.Style.Font.SetBold();
                            headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            if (isBulanan)
                            {
                                worksheet.Cell(currentRow, 2).Value = "s/d Bulan Lalu";
                                worksheet.Cell(currentRow, 3).Value = "Mutasi";
                                worksheet.Cell(currentRow, 4).Value = "s/d Bulan Ini";
                                worksheet.Range(currentRow, 2, currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            }
                            else
                            {
                                // PERBAIKAN: Tulis "s/d Bulan Tahun" di kolom Excel
                                worksheet.Cell(currentRow, 2).Value = item.HeaderTahunIni;
                                worksheet.Cell(currentRow, 3).Value = item.HeaderTahunLalu;
                                worksheet.Range(currentRow, 2, currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            }
                            currentRow++;
                            continue;
                        }

                        if (isBulanan)
                        {
                            if (item.NilaiLalu.HasValue) worksheet.Cell(currentRow, 2).Value = item.NilaiLalu.Value;
                            if (item.NilaiMutasi.HasValue) worksheet.Cell(currentRow, 3).Value = item.NilaiMutasi.Value;
                            if (item.NilaiIni.HasValue) worksheet.Cell(currentRow, 4).Value = item.NilaiIni.Value;
                            
                            worksheet.Range(currentRow, 2, currentRow, 4).Style.NumberFormat.Format = "#,##0.00;(#,##0.00);0.00";
                        }
                        else
                        {
                            if (item.NilaiIni.HasValue) worksheet.Cell(currentRow, 2).Value = item.NilaiIni.Value;
                            if (item.NilaiLalu.HasValue) worksheet.Cell(currentRow, 3).Value = item.NilaiLalu.Value;
                            
                            // [PERBAIKAN]: Tambahkan format kurung untuk Excel Laporan Tahunan
                            worksheet.Range(currentRow, 2, currentRow, 3).Style.NumberFormat.Format = "#,##0.00;(#,##0.00);0.00";
                        }

                        if (item.TipeBaris == "HEADING")
                        {
                            cellDesc.Style.Font.SetBold();
                            if (item.Level > 1) cellDesc.Style.Alignment.SetIndent(item.Level - 1);
                        }
                        else if (item.TipeBaris == "DETAIL")
                        {
                            cellDesc.Style.Alignment.SetIndent(item.Level);
                        }
                        else if (item.TipeBaris == "TOTAL")
                        {
                            var totalRange = worksheet.Range(currentRow, 1, currentRow, maxCol);
                            totalRange.Style.Font.SetBold();
                            cellDesc.Style.Alignment.SetIndent(item.Level > 1 ? item.Level : 0);

                            worksheet.Range(currentRow, 2, currentRow, maxCol).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                            if (item.Level == 1) 
                            {
                                worksheet.Range(currentRow, 2, currentRow, maxCol).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                            }
                        }

                        currentRow++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return Ok(new { Status = "OK", FileName = $"Laporan_{sheetName}_{DateTime.Now:yyyyMMdd}.xlsx", FileData = Convert.ToBase64String(stream.ToArray()) });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.Message });
            }
        }
    }

    public class LaporanKeuanganFilterDto
    {
        public string TipeLaporan { get; set; }
        public string JenisPeriode { get; set; } 
        public int Bulan { get; set; }           
        public int Tahun { get; set; }           
    }
}