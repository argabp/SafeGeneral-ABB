using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.LaporanNeracaSaldos.Queries;
using System.Linq;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;
using ABB.Application.TypeCoas.Queries;

namespace ABB.Web.Modules.LaporanNeracaSaldo
{
    public class LaporanNeracaSaldoController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanNeracaSaldoController(IReportGeneratorService reportGeneratorService)
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

        [HttpGet]
        public async Task<IActionResult> GetTipeAkunList(string text)
        {
            var filterText = text ?? "";
            var data = await Mediator.Send(new GetAllTypeCoaQuery() { SearchKeyword = filterText });
            var list = data.Select(x => new
            {
                Value = x.Type.Trim(),
                Text = $"{x.Type.Trim()} - {x.Nama.Trim()}" 
            }).ToList();

            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] LaporanNeracaSaldoFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanNeracaSaldoQuery
                {
                    DatabaseName = databaseName,
                    Bulan = model.Bulan,
                    Tahun = model.Tahun,
                    TipeAkunAwal = model.TipeAkunAwal,
                    TipeAkunAkhir = model.TipeAkunAkhir
                };

                var response = await Mediator.Send(query);
                
                if (response == null || response.RawData == null || !response.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                if (string.IsNullOrWhiteSpace(user))
                    throw new Exception("User ID tidak ditemukan. Tidak dapat menyimpan laporan.");                
                
                _reportGeneratorService.GenerateReport(
                    "LaporanNeracaSaldo.pdf",
                    response.HtmlString, 
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

        [HttpPost]
        public async Task<IActionResult> GenerateExcel([FromBody] LaporanNeracaSaldoFilterDto model)
        {
            try
            {
                var databaseName = Request.Cookies["DatabaseValue"];
                var user = CurrentUser.UserId;

                var query = new GetLaporanNeracaSaldoQuery
                {
                    DatabaseName = databaseName,
                    Bulan = model.Bulan,
                    Tahun = model.Tahun,
                    TipeAkunAwal = model.TipeAkunAwal,
                    TipeAkunAkhir = model.TipeAkunAkhir
                };

                var response = await Mediator.Send(query);
                var data = response.RawData;
                
                if (data == null || !data.Any())
                    throw new Exception("Data tidak ditemukan.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Laporan Neraca Saldo");

                    worksheet.Cell(1, 1).Value = "LAPORAN NERACA SALDO";
                    worksheet.Range("A1:N1").Merge(); // Berubah ke N (Kolom 14)
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(2, 1).Value = $"PERIODE : {model.Bulan} - {model.Tahun}";
                    worksheet.Range("A2:N2").Merge();
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Cell(2, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    int startRow = 4; 
                    worksheet.Cell(startRow, 1).Value = "Tanggal";
                    worksheet.Cell(startRow, 2).Value = "Group";
                    worksheet.Cell(startRow, 3).Value = "Tipe";
                    worksheet.Cell(startRow, 4).Value = "Nama Tipe";
                    worksheet.Cell(startRow, 5).Value = "Lokasi";
                    worksheet.Cell(startRow, 6).Value = "Kode Akun";
                    worksheet.Cell(startRow, 7).Value = "Nama Akun";

                    worksheet.Cell(startRow, 8).Value = "Saldo Awal";
                    worksheet.Range(startRow, 8, startRow, 9).Merge();

                    worksheet.Cell(startRow, 10).Value = "Mutasi";
                    worksheet.Range(startRow, 10, startRow, 11).Merge();

                    worksheet.Cell(startRow, 12).Value = "Saldo Akhir";
                    worksheet.Range(startRow, 12, startRow, 14).Merge(); // Berubah merge ke kolom 14

                    worksheet.Cell(5, 8).Value = "Debet";
                    worksheet.Cell(5, 9).Value = "Kredit";
                    worksheet.Cell(5, 10).Value = "Debet";
                    worksheet.Cell(5, 11).Value = "Kredit";
                    worksheet.Cell(5, 12).Value = "Debet";
                    worksheet.Cell(5, 13).Value = "Kredit";
                    worksheet.Cell(5, 14).Value = "Netto"; // Kolom Baru

                    var headerRange = worksheet.Range($"A{startRow}:N{startRow + 1}");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    decimal grandSaldoAwalDebet = 0, grandSaldoAwalKredit = 0;
                    decimal grandMutasiDebet = 0, grandMutasiKredit = 0;
                    decimal grandSaldoAkhirDebet = 0, grandSaldoAkhirKredit = 0;
                    decimal grandNetto = 0;

                    int row = 6;
                    
                    // GROUPING DATA
                    var groupedData = data.GroupBy(x => new { x.tipe, x.nm_tipe }).OrderBy(g => g.Key.tipe);

                    foreach (var group in groupedData)
                    {
                        decimal subSaldoAwalDebet = 0, subSaldoAwalKredit = 0;
                        decimal subMutasiDebet = 0, subMutasiKredit = 0;
                        decimal subSaldoAkhirDebet = 0, subSaldoAkhirKredit = 0;
                        decimal subNetto = 0;

                        foreach (var item in group)
                        {
                            decimal itemNetto = (item.saldoakhir_debet ?? 0) - (item.saldoakhir_kredit ?? 0);

                            worksheet.Cell(row, 1).Value = item.posisi?.ToString("dd-MM-yyyy");
                            worksheet.Cell(row, 2).Value = item.pos;
                            worksheet.Cell(row, 3).Value = item.tipe;
                            worksheet.Cell(row, 4).Value = item.nm_tipe;
                            worksheet.Cell(row, 5).Value = item.lokasi;
                            worksheet.Cell(row, 6).Value = item.kd_akun;
                            worksheet.Cell(row, 7).Value = item.nm_akun;
                            worksheet.Cell(row, 8).Value = item.saldoawal_debet;
                            worksheet.Cell(row, 9).Value = item.saldoawal_kredit;
                            worksheet.Cell(row, 10).Value = item.mutasi_debet;
                            worksheet.Cell(row, 11).Value = item.mutasi_kredit;
                            worksheet.Cell(row, 12).Value = item.saldoakhir_debet;
                            worksheet.Cell(row, 13).Value = item.saldoakhir_kredit;
                            worksheet.Cell(row, 14).Value = itemNetto;

                            subSaldoAwalDebet += item.saldoawal_debet ?? 0;
                            subSaldoAwalKredit += item.saldoawal_kredit ?? 0;
                            subMutasiDebet += item.mutasi_debet ?? 0;
                            subMutasiKredit += item.mutasi_kredit ?? 0;
                            subSaldoAkhirDebet += item.saldoakhir_debet ?? 0;
                            subSaldoAkhirKredit += item.saldoakhir_kredit ?? 0;
                            subNetto += itemNetto;

                            row++;
                        }

                        // PRINT SUB TOTAL
                        worksheet.Cell(row, 1).Value = $"SUB TOTAL {group.Key.nm_tipe}";
                        worksheet.Range(row, 1, row, 7).Merge();
                        worksheet.Cell(row, 8).Value = subSaldoAwalDebet;
                        worksheet.Cell(row, 9).Value = subSaldoAwalKredit;
                        worksheet.Cell(row, 10).Value = subMutasiDebet;
                        worksheet.Cell(row, 11).Value = subMutasiKredit;
                        worksheet.Cell(row, 12).Value = subSaldoAkhirDebet;
                        worksheet.Cell(row, 13).Value = subSaldoAkhirKredit;
                        worksheet.Cell(row, 14).Value = subNetto;
                        
                        worksheet.Range(row, 1, row, 14).Style.Font.Bold = true;
                        worksheet.Range(row, 1, row, 14).Style.Fill.BackgroundColor = XLColor.WhiteSmoke;
                        
                        grandSaldoAwalDebet += subSaldoAwalDebet;
                        grandSaldoAwalKredit += subSaldoAwalKredit;
                        grandMutasiDebet += subMutasiDebet;
                        grandMutasiKredit += subMutasiKredit;
                        grandSaldoAkhirDebet += subSaldoAkhirDebet;
                        grandSaldoAkhirKredit += subSaldoAkhirKredit;
                        grandNetto += subNetto;
                        
                        row++;
                    }

                    // PRINT GRAND TOTAL
                    worksheet.Cell(row, 1).Value = "GRAND TOTAL";
                    worksheet.Range(row, 1, row, 7).Merge();
                    worksheet.Cell(row, 8).Value = grandSaldoAwalDebet;
                    worksheet.Cell(row, 9).Value = grandSaldoAwalKredit;
                    worksheet.Cell(row, 10).Value = grandMutasiDebet;
                    worksheet.Cell(row, 11).Value = grandMutasiKredit;
                    worksheet.Cell(row, 12).Value = grandSaldoAkhirDebet;
                    worksheet.Cell(row, 13).Value = grandSaldoAkhirKredit;
                    worksheet.Cell(row, 14).Value = grandNetto;

                    var totalRange = worksheet.Range(row, 1, row, 14);
                    totalRange.Style.Font.Bold = true;
                    totalRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    
                    worksheet.Range(6, 8, row, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Columns(8, 14).Style.NumberFormat.Format = "#,##0.00";
                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var base64Str = Convert.ToBase64String(stream.ToArray());
                        return Ok(new { Status = "OK", FileName = "Laporan Neraca Saldo.xlsx", FileData = base64Str });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "ERROR", Message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }

    public class LaporanNeracaSaldoFilterDto
    {
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        public string TipeAkunAwal { get; set; }
        public string TipeAkunAkhir { get; set; }
    }
}