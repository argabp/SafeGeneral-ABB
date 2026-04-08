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

namespace ABB.Application.LaporanPelunasans.Queries
{
    public class LaporanPelunasanResponse
    {
        public string HtmlString { get; set; }
        // KITA PAKAI ENTITY BARU, BUKAN InquiryNotaProduksiDto LAGI
        public List<SpLaporanPelunasanResult> RawData { get; set; } 
    }

    public class GetLaporanPelunasanQuery : IRequest<LaporanPelunasanResponse> 
    {
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string JenisAwal { get; set; }
        public string JenisAkhir { get; set; }
        public string BulanAwal { get; set; }
        public string BulanAkhir { get; set; }
        public string Tahun { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetLaporanPelunasanQueryHandler : IRequestHandler<GetLaporanPelunasanQuery, LaporanPelunasanResponse>
    {
        private readonly IDbContextPstNota _context;
        private readonly IHostEnvironment _environment;

        // IMapper sudah dihapus karena tidak butuh lagi
        public GetLaporanPelunasanQueryHandler(IDbContextPstNota context, IHostEnvironment environment) 
        {
            _context = context;
            _environment = environment; 
        }

        public async Task<LaporanPelunasanResponse> Handle(GetLaporanPelunasanQuery request, CancellationToken cancellationToken)
        {
            // 1. Parsing parameter
            int bulanAwal = string.IsNullOrEmpty(request.BulanAwal) ? 1 : int.Parse(request.BulanAwal);
            int bulanAkhir = string.IsNullOrEmpty(request.BulanAkhir) ? 12 : int.Parse(request.BulanAkhir);
            int tahun = string.IsNullOrEmpty(request.Tahun) ? DateTime.Now.Year : int.Parse(request.Tahun);

            // 2. Eksekusi Stored Procedure langsung ke Entity
            var dataLaporan = await _context.SpLaporanPelunasanResults
                .FromSqlRaw("EXEC sp_LaporanPelunasan {0}, {1}, {2}, {3}, {4}, {5}",
                    request.KodeCabang ?? "",
                    request.JenisAwal ?? "",
                    request.JenisAkhir ?? "",
                    bulanAwal,
                    bulanAkhir,
                    tahun)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Jika kosong, langsung lempar Exception
            if (!dataLaporan.Any())
                throw new Exception("Data tidak ditemukan untuk periode tersebut.");

            // 3. Ambil Nama Cabang untuk Header
            string namaCabang = "SEMUA CABANG";

            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var cabangEntity = await _context.Set<Cabang>()
                    .FirstOrDefaultAsync(c => c.kd_cb == request.KodeCabang, cancellationToken);

                if (cabangEntity != null)
                    namaCabang = cabangEntity.nm_cb;
            }

            // =================================================================
            // START: LOGIKA RENDERING TEMPLATE SCRIBAN
            // =================================================================
            
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";
            
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanPelunasan.html");
            if (!File.Exists(reportPath)) throw new FileNotFoundException("Template LaporanPelunasan.html tidak ditemukan!");

            string templateReportHtml = await File.ReadAllTextAsync(reportPath, cancellationToken);
            
            StringBuilder detailsBuilder = new StringBuilder();
            var idx = 1;
            
            // 4. Looping langsung dari Entity Baru
            foreach (var item in dataLaporan) 
            {
                detailsBuilder.Append($@"
                    <tr>
                        <td class='center'>{idx}</td>
                        <td>{fmtDate(item.date)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_nd) ? "-" : item.no_nd)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_pl) ? "-" : item.no_pl)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_cust) ? "-" : item.nm_cust)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_pos) ? "-" : item.nm_pos)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_brok) ? "-" : item.nm_brok)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_cust2) ? "-" : item.nm_cust2)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.lok) ? "-" : item.lok)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.kd_tutup) ? "-" : item.kd_tutup)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_bukti) ? "-" : item.no_bukti)}</td> 
                        <td>{fmtDate(item.tgl_byr)}</td>                                          
                        <td class='right'>{fmtNum(item.jumlah)}</td>
                    </tr>");
                idx++;
            }
            
            Template templateReport = Template.Parse(templateReportHtml);

            string resultTemplate = templateReport.Render(new
            {
                details = detailsBuilder.ToString(),
                kodecabang = request.KodeCabang,
                namacabang = namaCabang,
                periode = $"{request.BulanAwal} s/d {request.BulanAkhir} - {request.Tahun}"
            });

            return new LaporanPelunasanResponse
            {
                HtmlString = resultTemplate,
                RawData = dataLaporan
            };
        }
    }
}