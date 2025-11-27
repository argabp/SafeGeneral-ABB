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

namespace ABB.Application.LaporanOutstandings.Queries
{
  public class GetLaporanOutstandingQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string TglProduksiAwal { get; set; }
        public string TglProduksiAkhir { get; set; }
        public string TglPelunasan { get; set; }
        public string JenisTransaksi { get; set; }
        public string UserLogin { get; set; }
    }

    public class GetLaporanOutstandingQueryHandler : IRequestHandler<GetLaporanOutstandingQuery, string>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _environment;

        public GetLaporanOutstandingQueryHandler(IDbContextPstNota context, IMapper mapper, IHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanOutstandingQuery request, CancellationToken cancellationToken)
        {
            // --- Convert Tanggal ---
            DateTime tglProdAwal = DateTime.Parse(request.TglProduksiAwal);
            DateTime tglProdAkhir = DateTime.Parse(request.TglProduksiAkhir);
            DateTime tglPelunasan = DateTime.Parse(request.TglPelunasan);

            // --- Base Query Produksi (Saldo > 0) ---
            var db = _context.Set<Produksi>()
                .Where(x => (x.saldo ?? 0) > 0);

            // --- Filter Lokasi berdasar 2 digit terakhir kode cabang ---
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var cabang2Digit = request.KodeCabang.Length >= 2
                    ? request.KodeCabang[^2..]
                    : request.KodeCabang;

                db = db.Where(x =>
                    !string.IsNullOrEmpty(x.lok) &&
                    x.lok.Trim() == cabang2Digit);
            }

            // --- Filter Tanggal Produksi ---
            db = db.Where(x =>
                x.date.HasValue &&
                x.date.Value.Date >= tglProdAwal &&
                x.date.Value.Date <= tglProdAkhir);

            // --- Filter Tanggal Pelunasan (tanggal bayar <= tglPelunasan) ---
            db = db.Where(x =>
                x.tgl_byr.HasValue &&
                x.tgl_byr.Value.Date <= tglPelunasan);

            // --- Nama Cabang ---
            string namaCabang = "-";
            var cabangEntity = await _context.Set<Cabang>()
                .FirstOrDefaultAsync(c => c.kd_cb == request.KodeCabang, cancellationToken);
            if (cabangEntity != null)
                namaCabang = cabangEntity.nm_cb;

            // --- Ambil Data Produksi ---
            var dataLaporan = await db
                .ProjectTo<InquiryNotaProduksiDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.jn_ass)
                .ThenBy(x => x.date)
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any())
                throw new Exception("Data tidak ditemukan.");

            // --- Proses Template (sama seperti sebelumnya) ---

            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? $"{n.Value:N2}" : "0.00";

            string reportPath = Path.Combine(
                _environment.ContentRootPath,
                "Modules", "Reports", "Templates",
                "LaporanOutstanding.html"
            );

            string templateHtml = await File.ReadAllTextAsync(reportPath);

            StringBuilder detailsBuilder = new StringBuilder();
            int idx = 1;

            foreach (var item in dataLaporan)
            {
                string tglProduksi = fmtDate(item.date);
                string tglJthTempo = fmtDate(item.tgl_jth_tempo);
                string tglBayar = fmtDate(item.tgl_byr);

                // Hitung Umur
                int umur = 0;
                if (item.date.HasValue && item.tgl_jth_tempo.HasValue)
                {
                    umur = (item.tgl_jth_tempo.Value - item.date.Value).Days;
                    if (umur < 0) umur = 0;
                }

                decimal nilaiNota = item.saldo ?? 0;
                decimal nilaiBayar = item.jumlah ?? 0;
                decimal nilaiOs = nilaiNota - nilaiBayar;

                detailsBuilder.Append($@"
                    <tr>
                        <td class='center'>{idx}</td>
                        <td>{item.no_nd}/<br>{item.no_pl}</td>
                        <td>{item.nm_cust2}/<br>{item.nm_pos}</td>
                        <td>{item.nm_brok}</td>
                        <td></td>
                        <td>{item.lok}/<br>{item.kd_tutup}</td>
                        <td>{tglProduksi}/<br>{tglJthTempo}</td>
                        <td>{item.curensi}/<br>{item.kurs}</td>
                        <td>{umur}</td>
                        <td>{fmtNum(nilaiNota)}</td>
                        <td>{fmtNum(nilaiBayar)}</td>
                        <td>{fmtNum(nilaiOs)}</td>
                    </tr>");

                idx++;
            }

            var template = Scriban.Template.Parse(templateHtml);

            string renderedHtml = template.Render(new
            {
                details = detailsBuilder.ToString(),
                KodeCabang = request.KodeCabang,
                NamaCabang = namaCabang,
                Periode = $"{request.TglProduksiAwal} s/d {request.TglProduksiAkhir} - Pelunasan ≤ {request.TglPelunasan}"
            });

            return renderedHtml;
        }
    }

}
