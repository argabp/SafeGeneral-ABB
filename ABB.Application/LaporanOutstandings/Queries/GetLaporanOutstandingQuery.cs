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
using Scriban.Runtime;
using System.IO;
using System.Text;
using ABB.Application.Cabangs.Queries;
using ABB.Domain.Entities;

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
        public string JenisLaporan { get; set; }
        public string UserLogin { get; set; }
        public List<string> SelectedCodes { get; set; }
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
            // --- 1. Filter Data Produksi (Master) ---
            DateTime tglProdAwal = DateTime.Parse(request.TglProduksiAwal);
            DateTime tglProdAkhir = DateTime.Parse(request.TglProduksiAkhir);
            DateTime tglPelunasan = DateTime.Parse(request.TglPelunasan);

            var db = _context.Set<Produksi>().Where(x => (x.saldo ?? 0) > 0);

            // Filter Lokasi
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                var cabang2Digit = request.KodeCabang.Length >= 2 ? request.KodeCabang[^2..] : request.KodeCabang;
                db = db.Where(x => !string.IsNullOrEmpty(x.lok) && x.lok.Trim() == cabang2Digit);
            }

            // Filter Tanggal Produksi
            db = db.Where(x => x.date.HasValue && x.date.Value.Date >= tglProdAwal && x.date.Value.Date <= tglProdAkhir);
            
            // Filter Tanggal Pelunasan (Hanya ambil yang tanggal bayarnya <= tglPelunasan)
            // Catatan: Ini filter awal di tabel produksi, nanti validasi real di tabel pembayaran
            db = db.Where(x => x.tgl_byr.HasValue && x.tgl_byr.Value.Date <= tglPelunasan);
            
            // Filter Checkbox (Jenis Transaksi & Kode Ass)
             if (request.SelectedCodes != null && request.SelectedCodes.Any())
            {
                var codesForTypeP = new List<string>();
                var codesForTypeK = new List<string>();

                if (request.JenisTransaksi == "Piutang")
                {
                    foreach (var code in request.SelectedCodes)
                    {
                        if (code == "A3" || code == "C1" || code == "C2") codesForTypeK.Add(code);
                        else codesForTypeP.Add(code);
                    }
                }
                else // HUTANG
                {
                    foreach (var code in request.SelectedCodes)
                    {
                        if (code == "A3" || code == "C1" || code == "C2") codesForTypeP.Add(code);
                        else codesForTypeK.Add(code);
                    }
                }

                db = db.Where(x => 
                    (x.type == "P" && codesForTypeP.Contains(x.jn_ass)) || 
                    (x.type == "K" && codesForTypeK.Contains(x.jn_ass))
                );
            }

            // Ambil Nama Cabang
            string namaCabang = "-";
            var cabangEntity = await _context.Set<Cabang>().FirstOrDefaultAsync(c => c.kd_cb == request.KodeCabang, cancellationToken);
            if (cabangEntity != null) namaCabang = cabangEntity.nm_cb;

            // Execute Query Master Produksi
            var dataLaporan = await db
                .ProjectTo<InquiryNotaProduksiDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.jn_ass) 
                .ThenBy(x => x.date)
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any()) throw new Exception("Data tidak ditemukan.");

            // =========================================================================================
            // --- 2. HITUNG NILAI BAYAR (EntriPembayaranBank, Kas, Piutang) ---
            // =========================================================================================
            
            // A. Ambil Daftar No Nota Unik dari data yang sudah ditarik
            var listNoNota = dataLaporan.Select(x => x.no_nd).Distinct().ToList();

            // B. Query Agregat Pembayaran Bank
            // Asumsi: Nama kolom adalah 'no_nd' dan 'jumlah', sesuaikan jika beda
            var bayarBank = await _context.Set<EntriPembayaranBank>()
                .Where(x => listNoNota.Contains(x.NoNota4))
                .GroupBy(x => x.NoNota4)
                .Select(g => new { NoNota = g.Key, Total = g.Sum(x => x.TotalDlmRupiah ?? 0) })
                .ToListAsync(cancellationToken);

            // C. Query Agregat Pembayaran Kas
            var bayarKas = await _context.Set<EntriPembayaranKas>()
                .Where(x => listNoNota.Contains(x.NoNota4))
                .GroupBy(x => x.NoNota4)
                .Select(g => new { NoNota = g.Key, Total = g.Sum(x => x.TotalDlmRupiah ?? 0) })
                .ToListAsync(cancellationToken);

            // D. Query Agregat Pembayaran Piutang (Jurnal/Offset)
            var bayarPiutang = await _context.Set<EntriPenyelesaianPiutang>()
                .Where(x => listNoNota.Contains(x.NoNota))
                .GroupBy(x => x.NoNota)
                .Select(g => new { NoNota = g.Key, Total = g.Sum(x => x.TotalBayarRp ?? 0) })
                .ToListAsync(cancellationToken);

            // E. Gabungkan semua pembayaran ke Dictionary Memory
            var totalPembayaranPerNota = new Dictionary<string, decimal>();

            // Helper lokal untuk merge
            void MergeToDict(IEnumerable<dynamic> sumberData)
            {
                foreach (var item in sumberData)
                {
                    if (totalPembayaranPerNota.ContainsKey(item.NoNota))
                        totalPembayaranPerNota[item.NoNota] += item.Total;
                    else
                        totalPembayaranPerNota[item.NoNota] = item.Total;
                }
            }

            MergeToDict(bayarBank);
            MergeToDict(bayarKas);
            MergeToDict(bayarPiutang);

            // F. Update Nilai Bayar di Data Laporan Utama
            foreach (var item in dataLaporan)
            {
                // Jika ada pembayaran di dictionary, pakai itu. Jika tidak, 0.
                if (totalPembayaranPerNota.ContainsKey(item.no_nd))
                {
                    item.jumlah = totalPembayaranPerNota[item.no_nd];
                }
                else
                {
                    item.jumlah = 0;
                }
                
                // Pastikan Saldo (Nilai Nota) tidak null
                if (item.saldo == null) item.saldo = 0;
            }

            // =========================================================================================
            // --- END HITUNG NILAI BAYAR ---
            // =========================================================================================

            // --- 3. Generate Rows HTML ---
            Func<DateTime?, string> fmtDate = d => d.HasValue ? d.Value.ToString("dd-MM-yyyy") : "-";
            Func<decimal?, string> fmtNum = n => n.HasValue ? $"{n.Value:N2}" : "0.00";

            string BuildRowHtml(int index, InquiryNotaProduksiDto item)
            {
                int umur = 0;
                if (item.date.HasValue && item.tgl_jth_tempo.HasValue)
                {
                    umur = (item.tgl_jth_tempo.Value - item.date.Value).Days;
                    if (umur < 0) umur = 0;
                }
                
                decimal nNota = item.saldo ?? 0;
                decimal nBayar = item.jumlah ?? 0; // Ini sekarang sudah nilai gabungan dari 3 tabel
                decimal nOs = nNota - nBayar;

                return $@"
                    <tr>
                        <td class='center'>{index}</td>
                        <td>{item.no_nd}/<br>{item.no_pl}</td>
                        <td>{item.nm_cust2}/<br>{item.nm_pos}</td>
                        <td>{item.nm_brok}</td>
                        <td>{item.jn_ass}</td>
                        <td>{item.lok}/<br>{item.kd_tutup}</td>
                        <td>{fmtDate(item.date)}/<br>{fmtDate(item.tgl_jth_tempo)}</td>
                        <td>{item.curensi}/<br>{item.kurs}</td>
                        <td>{umur}</td>
                        <td style='text-align:right'>{fmtNum(nNota)}</td>
                        <td style='text-align:right'>{fmtNum(nBayar)}</td>
                        <td style='text-align:right'>{fmtNum(nOs)}</td>
                    </tr>";
            }

            string BuildSubtotalHtml(string label, decimal totNota, decimal totBayar, decimal totOs)
            {
                return $@"
                    <tr style='font-weight:bold; background-color:#f0f0f0;'>
                        <td colspan='9' style='text-align:right; padding-right:10px;'>TOTAL {label.ToUpper()} :</td>
                        <td style='text-align:right'>{fmtNum(totNota)}</td>
                        <td style='text-align:right'>{fmtNum(totBayar)}</td>
                        <td style='text-align:right'>{fmtNum(totOs)}</td>
                    </tr>";
            }

            StringBuilder detailsBuilder = new StringBuilder();
            int idx = 1;

            if (request.JenisLaporan == "Detail" || string.IsNullOrEmpty(request.JenisLaporan))
            {
                foreach (var item in dataLaporan) detailsBuilder.Append(BuildRowHtml(idx++, item));
                
                // Hitung Grand Total dari data yang sudah diupdate nilai bayarnya
                decimal gNota = dataLaporan.Sum(x => x.saldo ?? 0);
                decimal gBayar = dataLaporan.Sum(x => x.jumlah ?? 0);
                decimal gOs = gNota - gBayar;
                detailsBuilder.Append(BuildSubtotalHtml("KESELURUHAN", gNota, gBayar, gOs));
            }
            else
            {
                IEnumerable<IGrouping<string, InquiryNotaProduksiDto>> groupedData;
                switch (request.JenisLaporan)
                {
                    case "COB": groupedData = dataLaporan.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key); break;
                    case "Tertanggung": groupedData = dataLaporan.GroupBy(x => x.nm_cust2 ?? "TANPA NAMA").OrderBy(g => g.Key); break;
                    case "Pos": groupedData = dataLaporan.GroupBy(x => x.nm_pos ?? "TANPA POS").OrderBy(g => g.Key); break;
                    case "Broker": groupedData = dataLaporan.GroupBy(x => x.nm_brok ?? "DIRECT").OrderBy(g => g.Key); break;
                    default: groupedData = dataLaporan.GroupBy(x => x.jn_ass ?? "LAINNYA").OrderBy(g => g.Key); break;
                }

                foreach (var group in groupedData)
                {
                    detailsBuilder.Append($@"<tr><td colspan='12' style='background-color:#e6f7ff; font-weight:bold; padding-left:10px;'>{request.JenisLaporan}: {group.Key}</td></tr>");
                    
                    decimal subNota = 0, subBayar = 0;
                    foreach (var item in group)
                    {
                        detailsBuilder.Append(BuildRowHtml(idx++, item));
                        subNota += (item.saldo ?? 0);
                        subBayar += (item.jumlah ?? 0); // Akumulasi nilai bayar yang baru
                    }
                    detailsBuilder.Append(BuildSubtotalHtml(group.Key, subNota, subBayar, subNota - subBayar));
                }
            }

            // --- 4. Render Template ---
            string reportPath = Path.Combine(_environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanOutstanding.html");
            string templateHtml = await File.ReadAllTextAsync(reportPath);
            var template = Scriban.Template.Parse(templateHtml);

            var modelData = new
            {
                details = detailsBuilder.ToString(),
                KodeCabang = request.KodeCabang,
                NamaCabang = namaCabang,
                TglProduksiAwal = request.TglProduksiAwal,
                TglProduksiAkhir = request.TglProduksiAkhir,
                TanggalPosisi = DateTime.Now.ToString("dd-MM-yyyy"),
                JudulLaporan = $"Laporan Outstanding - {request.JenisTransaksi} ({request.JenisLaporan})",
                TglPelunasan = request.TglPelunasan
            };

            var context = new TemplateContext();
            var scriptObj = new ScriptObject();
            scriptObj.Import(modelData, renamer: member => member.Name);
            context.PushGlobal(scriptObj);
            
            return template.Render(context);
        }
    }

}
