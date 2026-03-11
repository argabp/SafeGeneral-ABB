using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pastikan namespace entitas terpanggil
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
        // KITA PAKAI ENTITY BARU, BUKAN InquiryNotaProduksiDto LAGI
        public List<SpLaporanNeracaSaldoResult> RawData { get; set; } 
    }

    public class GetLaporanNeracaSaldoQuery : IRequest<LaporanNeracaSaldoResponse> 
    {
        public string DatabaseName { get; set; }
        public string Bulan { get; set; }
        public string Tahun { get; set; }
    }

    public class GetLaporanNeracaSaldoQueryHandler : IRequestHandler<GetLaporanNeracaSaldoQuery, LaporanNeracaSaldoResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        // IMapper sudah dihapus karena tidak butuh lagi
        public GetLaporanNeracaSaldoQueryHandler(IDbContextPstNota context, IHostEnvironment environment) 
        {
            _context = context;
            _environment = environment; 
        }

        public async Task<LaporanNeracaSaldoResponse> Handle(GetLaporanNeracaSaldoQuery request, CancellationToken cancellationToken)
        {
            // 1. Parsing parameter
            int bulan = string.IsNullOrEmpty(request.Bulan) ? 1 : int.Parse(request.Bulan);
            int tahun = string.IsNullOrEmpty(request.Tahun) ? DateTime.Now.Year : int.Parse(request.Tahun);

            // 2. Eksekusi Stored Procedure langsung ke Entity
            DateTime tgl = new DateTime(tahun, bulan, 1);

            var dataLaporan = await _context.SpLaporanNeracaSaldoResults
                .FromSqlRaw("EXEC spr_akt_03 {0}", tgl)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Jika kosong, langsung lempar Exception
            if (!dataLaporan.Any())
                throw new Exception("Data tidak ditemukan untuk periode tersebut.");


            // =================================================================
            // START: LOGIKA RENDERING TEMPLATE SCRIBAN
            // =================================================================
            
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";
            
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanNeracaSaldo.html");
            if (!File.Exists(reportPath)) throw new FileNotFoundException("Template LaporanNeracaSaldo.html tidak ditemukan!");

            string templateReportHtml = await File.ReadAllTextAsync(reportPath, cancellationToken);
            
            StringBuilder detailsBuilder = new StringBuilder();
            var idx = 1;
            
            decimal totalSaldoAwalDebet = 0;
            decimal totalSaldoAwalKredit = 0;
            decimal totalMutasiDebet = 0;
            decimal totalMutasiKredit = 0;
            decimal totalSaldoAkhirDebet = 0;
            decimal totalSaldoAkhirKredit = 0;

            // 4. Looping langsung dari Entity Baru
            foreach (var item in dataLaporan) 
            {

                totalSaldoAwalDebet += item.saldoawal_debet ?? 0;
                totalSaldoAwalKredit += item.saldoawal_kredit ?? 0;
                totalMutasiDebet += item.mutasi_debet ?? 0;
                totalMutasiKredit += item.mutasi_kredit ?? 0;
                totalSaldoAkhirDebet += item.saldoakhir_debet ?? 0;
                totalSaldoAkhirKredit += item.saldoakhir_kredit ?? 0;


                detailsBuilder.Append($@"
                    <tr>
                        <td>{fmtDate(item.posisi)}</td>
                        <td class='center'>{(string.IsNullOrWhiteSpace(item.pos) ? "-" : item.pos)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.tipe) ? "-" : item.tipe)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_tipe) ? "-" : item.nm_tipe)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.lokasi) ? "-" : item.lokasi)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.kd_akun) ? "-" : item.kd_akun)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_akun) ? "-" : item.nm_akun)}</td>
                        <td>{fmtNum(item.saldoawal_debet)}</td>
                        <td>{fmtNum(item.saldoawal_kredit)}</td>
                        <td>{fmtNum(item.mutasi_debet)}</td>
                        <td>{fmtNum(item.mutasi_kredit)}</td>
                        <td>{fmtNum(item.saldoakhir_debet)}</td>
                        <td>{fmtNum(item.saldoakhir_kredit)}</td>
                    </tr>");
                idx++;
            }
            

            detailsBuilder.Append($@"
            <tr style='font-weight:bold;background:#f2f2f2'>
                <td colspan='7' style='text-align:center'>TOTAL</td>
                <td>{fmtNum(totalSaldoAwalDebet)}</td>
                <td>{fmtNum(totalSaldoAwalKredit)}</td>
                <td>{fmtNum(totalMutasiDebet)}</td>
                <td>{fmtNum(totalMutasiKredit)}</td>
                <td>{fmtNum(totalSaldoAkhirDebet)}</td>
                <td>{fmtNum(totalSaldoAkhirKredit)}</td>
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