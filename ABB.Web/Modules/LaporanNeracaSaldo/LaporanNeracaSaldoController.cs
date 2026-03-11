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

namespace ABB.Web.Modules.LaporanNeracaSaldo
{
    public class LaporanNeracaSaldoController : AuthorizedBaseController
    {

        private readonly IReportGeneratorService _reportGeneratorService;
        // private readonly IViewRenderService _viewRenderService;

        public LaporanNeracaSaldoController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
            // _viewRenderService = viewRenderService; // Injeksi
        }

        public async Task<IActionResult> Index() // Ubah jadi async Task
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            var databaseName = Request.Cookies["DatabaseValue"]; // Pastikan pake DatabaseValue buat query
            return View();
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
                    Tahun = model.Tahun
                };

                var response = await Mediator.Send(query);
                
                if (response == null || response.RawData == null || !response.RawData.Any())
                    throw new Exception("Data tidak ditemukan.");

                var userId = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new Exception("User ID tidak ditemukan. Tidak dapat menyimpan laporan.");                
                
                _reportGeneratorService.GenerateReport(
                    "LaporanNeracaSaldo.pdf",
                    response.HtmlString, 
                    userId,
                    Orientation.Landscape,
                    5, 5, 5, 5,
                   PaperKind.Legal
                );

                return Ok(new { Status = "OK", Data = userId });
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
                    Tahun = model.Tahun
                };

                var response = await Mediator.Send(query);
                
                var data = response.RawData;
                
                if (data == null || !data.Any())
                    throw new Exception("Data tidak ditemukan.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Laporan Neraca Saldo");

                    worksheet.Cell(1, 1).Value = "LAPORAN NERACA SALDO";
                    worksheet.Range("A1:L1").Merge(); 
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(2, 1).Value = $"PERIODE : {model.Bulan} - {model.Tahun}";
                    worksheet.Range("A2:L2").Merge(); // <--- BIKIN MERGE KE TENGAH
                    worksheet.Cell(2, 1).Style.Font.Bold = true;
                    worksheet.Cell(2, 1).Style.Font.FontSize = 14;
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // <--- CENTER TEXT

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
                    worksheet.Range(startRow, 12, startRow, 13).Merge();

                    worksheet.Cell(5, 8).Value = "Debet";
                    worksheet.Cell(5, 9).Value = "Kredit";

                    worksheet.Cell(5, 10).Value = "Debet";
                    worksheet.Cell(5, 11).Value = "Kredit";

                    worksheet.Cell(5, 12).Value = "Debet";
                    worksheet.Cell(5, 13).Value = "Kredit";

                    var headerRange = worksheet.Range($"A{startRow}:L{startRow}");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                    decimal totalSaldoAwalDebet = 0;
                    decimal totalSaldoAwalKredit = 0;
                    decimal totalMutasiDebet = 0;
                    decimal totalMutasiKredit = 0;
                    decimal totalSaldoAkhirDebet = 0;
                    decimal totalSaldoAkhirKredit = 0;

                    int row = 6;
                    foreach (var item in data)
                    {
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

                            totalSaldoAwalDebet += item.saldoawal_debet ?? 0;
                            totalSaldoAwalKredit += item.saldoawal_kredit ?? 0;
                            totalMutasiDebet += item.mutasi_debet ?? 0;
                            totalMutasiKredit += item.mutasi_kredit ?? 0;
                            totalSaldoAkhirDebet += item.saldoakhir_debet ?? 0;
                            totalSaldoAkhirKredit += item.saldoakhir_kredit ?? 0;

                            row++;

                    }

                        worksheet.Cell(row, 1).Value = "TOTAL";
                        worksheet.Range(row, 1, row, 7).Merge();

                        worksheet.Cell(row, 8).Value = totalSaldoAwalDebet;
                        worksheet.Cell(row, 9).Value = totalSaldoAwalKredit;

                        worksheet.Cell(row, 10).Value = totalMutasiDebet;
                        worksheet.Cell(row, 11).Value = totalMutasiKredit;

                        worksheet.Cell(row, 12).Value = totalSaldoAkhirDebet;
                        worksheet.Cell(row, 13).Value = totalSaldoAkhirKredit;

                        worksheet.Range(6,8,row,13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        var totalRange = worksheet.Range(row, 1, row, 13);

                        totalRange.Style.Font.Bold = true;
                        totalRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                        totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        worksheet.Columns(8, 13).Style.NumberFormat.Format = "#,##0.00";

                         worksheet.Columns().AdjustToContents();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            var base64Str = Convert.ToBase64String(content);

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
    }

}