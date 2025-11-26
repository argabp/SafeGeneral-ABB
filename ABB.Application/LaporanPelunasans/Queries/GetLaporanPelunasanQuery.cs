using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scriban;
using System.IO;
using System.Text;
using ABB.Application.Cabangs.Queries;

namespace ABB.Application.LaporanPelunasans.Queries
{
    public class GetLaporanPelunasanQuery : IRequest<string> // <--- UBAH DI SINI
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

    public class GetLaporanPelunasanQueryHandler : IRequestHandler<GetLaporanPelunasanQuery, string> // <--- UBAH DI SINI
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _environment; // <--- Tambahkan ini

        public GetLaporanPelunasanQueryHandler(IDbContextPstNota context, IMapper mapper, IHostEnvironment environment) // <--- Tambahkan IHostEnvironment
        {
            _context = context;
            _mapper = mapper;
            _environment = environment; // <--- Injeksi
        }

        public async Task<string> Handle(GetLaporanPelunasanQuery request, CancellationToken cancellationToken)
        {
         var db = _context.Set<Produksi>().Where(x => x.saldo == 0);

            // 🔹 1. Filter Lokasi (LOK) berdasarkan 2 digit terakhir KodeCabang
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var kodeCabangBersih = request.KodeCabang.Trim();
                var cabang2Digit = kodeCabangBersih.Length > 2
                ? kodeCabangBersih.Substring(kodeCabangBersih.Length - 2)
                : kodeCabangBersih;

                db = db.Where(x =>
                    !string.IsNullOrEmpty(x.lok) &&
                    x.lok.Trim().Equals(cabang2Digit)); 
            }

            // 🔹 2. Filter Jenis Asset (A1 - A3, B1 - B5, dll)
            if (!string.IsNullOrEmpty(request.JenisAwal) && !string.IsNullOrEmpty(request.JenisAkhir))
            {
                db = db.Where(x =>
                    string.Compare(x.jn_ass, request.JenisAwal) >= 0 &&
                    string.Compare(x.jn_ass, request.JenisAkhir) <= 0);
            }

            // 🔹 3. Filter Bulan dan Tahun (gunakan range tanggal)
            if (!string.IsNullOrEmpty(request.BulanAwal) &&
                !string.IsNullOrEmpty(request.BulanAkhir) &&
                !string.IsNullOrEmpty(request.Tahun))
            {
                int tahun = int.Parse(request.Tahun);
                int bulanAwal = int.Parse(request.BulanAwal);
                int bulanAkhir = int.Parse(request.BulanAkhir);

                // Periode: dari awal bulanAwal sampai akhir bulanAkhir
                DateTime tanggalAwal = new DateTime(tahun, bulanAwal, 01);
                DateTime tanggalAkhir = new DateTime(tahun, bulanAkhir, DateTime.DaysInMonth(tahun, bulanAkhir));

                db = db.Where(x =>
                    x.date.HasValue &&
                    x.date.Value.Date >= tanggalAwal &&
                    x.date.Value.Date <= tanggalAkhir);
            }

            string namaCabang = "-";
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var cabangEntity = await _context.Set<Cabang>()
                    .FirstOrDefaultAsync(c => c.kd_cb == request.KodeCabang, cancellationToken);

                if (cabangEntity != null)
                    namaCabang = cabangEntity.nm_cb;
            }

            // 🔹 4. Eksekusi query dan urutkan hasil
            var dataLaporan = await db
                .ProjectTo<InquiryNotaProduksiDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.jn_ass)
                .ThenBy(x => x.date)
                .ToListAsync(cancellationToken);

            // =================================================================
            // START: LOGIKA RENDERING TEMPLATE SCRIBA
            // =================================================================

            if (!dataLaporan.Any())
                throw new NullReferenceException("Data tidak ditemukan");
            
            // Definisikan fungsi helper (sama seperti yang ada di Razor View Anda)
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? string.Format("{0:N2}", n.Value) : "0.00";
            
            // 1. Baca Template HTML
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanPelunasan.html" );
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            // 2. Buat Detail Tabel (Baris data)
            StringBuilder detailsBuilder = new StringBuilder();
            var idx = 1;
            
            foreach (var item in dataLaporan)
            {
                var tglProduksi = fmtDate(item.date);
                var tglLunas = fmtDate(item.tgl_byr);
                var nilaiLunas = fmtNum(item.jumlah);
                
                detailsBuilder.Append($@"
                    <tr>
                        <td class='center'>{idx}</td>
                        <td>{tglProduksi}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_nd) ? "-" : item.no_nd)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_pl) ? "-" : item.no_pl)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_cust) ? "-" : item.nm_cust)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_pos) ? "-" : item.nm_pos)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_brok) ? "-" : item.nm_brok)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.nm_cust2) ? "-" : item.nm_cust2)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.lok) ? "-" : item.lok)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.kd_tutup) ? "-" : item.kd_tutup)}</td>
                        <td>{(string.IsNullOrWhiteSpace(item.no_bukti) ? "-" : item.no_bukti)}</td>
                        <td>{tglLunas}</td>
                        <td class='right'>{nilaiLunas}</td>
                    </tr>");
                idx++;
            }
            
            // 3. Render Scriban Template
            Template templateReport = Template.Parse( templateReportHtml );

            string resultTemplate = templateReport.Render(new
            {
                details = detailsBuilder.ToString(),
                KodeCabang = request.KodeCabang,
                NamaCabang = namaCabang,
                Periode = $"{request.BulanAwal}-{request.BulanAkhir}-{request.Tahun}"
            });

            return resultTemplate;
        }
    }
}