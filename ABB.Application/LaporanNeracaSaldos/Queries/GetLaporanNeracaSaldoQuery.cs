using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scriban;
using System.IO;
using System.Text;

namespace ABB.Application.LaporanNeracaSaldos.Queries
{
    public class LaporanNeracaSaldoResponse
    {
        public string HtmlString { get; set; }
        public List<SpLaporanNeracaSaldoResult> RawData { get; set; } 
    }

    public class GetLaporanNeracaSaldoQuery : IRequest<LaporanNeracaSaldoResponse> 
    {
        public string DatabaseName { get; set; }
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        public string TipeAkunAwal { get; set; }  
        public string TipeAkunAkhir { get; set; } 
    }

    public class GetLaporanNeracaSaldoQueryHandler : IRequestHandler<GetLaporanNeracaSaldoQuery, LaporanNeracaSaldoResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        public GetLaporanNeracaSaldoQueryHandler(IDbContextPstNota context, IHostEnvironment environment) 
        {
            _context = context;
            _environment = environment; 
        }

        public async Task<LaporanNeracaSaldoResponse> Handle(GetLaporanNeracaSaldoQuery request, CancellationToken cancellationToken)
        {
            int bulan = string.IsNullOrEmpty(request.Bulan) ? 1 : int.Parse(request.Bulan);
            int tahun = string.IsNullOrEmpty(request.Tahun) ? DateTime.Now.Year : int.Parse(request.Tahun);
            DateTime tgl = new DateTime(tahun, bulan, 1);

            var dataLaporan = await _context.SpLaporanNeracaSaldoResults
                .FromSqlRaw("EXEC spr_akt_03 {0}", tgl)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any())
                throw new Exception("Data tidak ditemukan untuk periode tersebut.");

            if (!string.IsNullOrWhiteSpace(request.TipeAkunAwal) || !string.IsNullOrWhiteSpace(request.TipeAkunAkhir))
            {
                string start = string.IsNullOrWhiteSpace(request.TipeAkunAwal) ? "00" : request.TipeAkunAwal.Trim();
                string end = string.IsNullOrWhiteSpace(request.TipeAkunAkhir) ? "ZZ" : request.TipeAkunAkhir.Trim();

                dataLaporan = dataLaporan.Where(x => 
                    !string.IsNullOrWhiteSpace(x.tipe) && 
                    string.Compare(x.tipe, start) >= 0 && 
                    string.Compare(x.tipe, end) <= 0
                ).ToList();
                
                if (!dataLaporan.Any())
                    throw new Exception($"Data tidak ditemukan untuk rentang Tipe Akun {start} s/d {end}.");
            }
            
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";
            
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanNeracaSaldo.html");
            if (!File.Exists(reportPath)) throw new FileNotFoundException("Template LaporanNeracaSaldo.html tidak ditemukan!");

            string templateReportHtml = await File.ReadAllTextAsync(reportPath, cancellationToken);
            StringBuilder detailsBuilder = new StringBuilder();
            
            decimal grandSaldoAwalDebet = 0, grandSaldoAwalKredit = 0;
            decimal grandMutasiDebet = 0, grandMutasiKredit = 0;
            decimal grandSaldoAkhirDebet = 0, grandSaldoAkhirKredit = 0;
            decimal grandNetto = 0;

            // GROUPING BERDASARKAN TIPE AKUN
            var groupedData = dataLaporan.GroupBy(x => new { x.tipe, x.nm_tipe }).OrderBy(g => g.Key.tipe);

            foreach (var group in groupedData)
            {
                decimal subSaldoAwalDebet = 0, subSaldoAwalKredit = 0;
                decimal subMutasiDebet = 0, subMutasiKredit = 0;
                decimal subSaldoAkhirDebet = 0, subSaldoAkhirKredit = 0;
                decimal subNetto = 0;

                foreach (var item in group) 
                {
                    decimal itemNetto = (item.saldoakhir_debet ?? 0) - (item.saldoakhir_kredit ?? 0);

                    subSaldoAwalDebet += item.saldoawal_debet ?? 0;
                    subSaldoAwalKredit += item.saldoawal_kredit ?? 0;
                    subMutasiDebet += item.mutasi_debet ?? 0;
                    subMutasiKredit += item.mutasi_kredit ?? 0;
                    subSaldoAkhirDebet += item.saldoakhir_debet ?? 0;
                    subSaldoAkhirKredit += item.saldoakhir_kredit ?? 0;
                    subNetto += itemNetto;

                    detailsBuilder.Append($@"
                        <tr>
                            <td>{fmtDate(item.posisi)}</td>
                            <td class='center'>{(string.IsNullOrWhiteSpace(item.pos) ? "-" : item.pos)}</td>
                            <td class='center'>{(string.IsNullOrWhiteSpace(item.tipe) ? "-" : item.tipe)}</td>
                            <td>{(string.IsNullOrWhiteSpace(item.nm_tipe) ? "-" : item.nm_tipe)}</td>
                            <td class='center'>{(string.IsNullOrWhiteSpace(item.lokasi) ? "-" : item.lokasi)}</td>
                            <td class='center'>{(string.IsNullOrWhiteSpace(item.kd_akun) ? "-" : item.kd_akun)}</td>
                            <td>{(string.IsNullOrWhiteSpace(item.nm_akun) ? "-" : item.nm_akun)}</td>
                            <td class='right'>{fmtNum(item.saldoawal_debet)}</td>
                            <td class='right'>{fmtNum(item.saldoawal_kredit)}</td>
                            <td class='right'>{fmtNum(item.mutasi_debet)}</td>
                            <td class='right'>{fmtNum(item.mutasi_kredit)}</td>
                            <td class='right'>{fmtNum(item.saldoakhir_debet)}</td>
                            <td class='right'>{fmtNum(item.saldoakhir_kredit)}</td>
                            <td class='right'>{fmtNum(itemNetto)}</td>
                        </tr>");
                }

                // PRINT BARIS SUB TOTAL PER TIPE
                detailsBuilder.Append($@"
                    <tr style='font-weight:bold; background:#e9ecef;'>
                        <td colspan='7' style='text-align:right; padding-right:15px;'>SUB TOTAL {group.Key.nm_tipe}</td>
                        <td class='right'>{fmtNum(subSaldoAwalDebet)}</td>
                        <td class='right'>{fmtNum(subSaldoAwalKredit)}</td>
                        <td class='right'>{fmtNum(subMutasiDebet)}</td>
                        <td class='right'>{fmtNum(subMutasiKredit)}</td>
                        <td class='right'>{fmtNum(subSaldoAkhirDebet)}</td>
                        <td class='right'>{fmtNum(subSaldoAkhirKredit)}</td>
                        <td class='right'>{fmtNum(subNetto)}</td>
                    </tr>");

                grandSaldoAwalDebet += subSaldoAwalDebet;
                grandSaldoAwalKredit += subSaldoAwalKredit;
                grandMutasiDebet += subMutasiDebet;
                grandMutasiKredit += subMutasiKredit;
                grandSaldoAkhirDebet += subSaldoAkhirDebet;
                grandSaldoAkhirKredit += subSaldoAkhirKredit;
                grandNetto += subNetto;
            }

            // PRINT BARIS GRAND TOTAL
            detailsBuilder.Append($@"
            <tr style='font-weight:bold; background:#d6d8db;'>
                <td colspan='7' style='text-align:center'>GRAND TOTAL</td>
                <td class='right'>{fmtNum(grandSaldoAwalDebet)}</td>
                <td class='right'>{fmtNum(grandSaldoAwalKredit)}</td>
                <td class='right'>{fmtNum(grandMutasiDebet)}</td>
                <td class='right'>{fmtNum(grandMutasiKredit)}</td>
                <td class='right'>{fmtNum(grandSaldoAkhirDebet)}</td>
                <td class='right'>{fmtNum(grandSaldoAkhirKredit)}</td>
                <td class='right'>{fmtNum(grandNetto)}</td>
            </tr>");

            Template templateReport = Template.Parse(templateReportHtml);
            string resultTemplate = templateReport.Render(new
            {
                details = detailsBuilder.ToString(),
                periode = $"{request.Bulan} - {request.Tahun}"
            });

            return new LaporanNeracaSaldoResponse
            {
                HtmlString = resultTemplate,
                RawData = dataLaporan
            };
        }
    }
}