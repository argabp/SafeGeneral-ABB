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

     public class BayarDto {
        public string NoNota { get; set; }
        public decimal Total { get; set; }
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
            // --- 1. QUERY MASTER PRODUKSI ---
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

            // Filter Tanggal
            db = db.Where(x => x.date.HasValue && x.date.Value.Date >= tglProdAwal && x.date.Value.Date <= tglProdAkhir);
            db = db.Where(x => x.tgl_byr == null || x.tgl_byr.Value.Date <= tglPelunasan);

            // Filter Checkbox (Piutang/Hutang)
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

            // Execute Query ke Memory
            var dataLaporan = await db
                .ProjectTo<InquiryNotaProduksiDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.jn_ass)
                .ThenBy(x => x.date)
                .ToListAsync(cancellationToken);

            if (!dataLaporan.Any()) throw new Exception("Data tidak ditemukan.");


            // =========================================================================================
            // --- 2. HITUNG NILAI BAYAR (FIXED WITH TRIM) ---
            // =========================================================================================
            
            // 1. Ambil List No Nota dari Produksi & TRIM spasi
            var listNoNota = dataLaporan
                .Where(x => !string.IsNullOrEmpty(x.no_nd))
                .Select(x => x.no_nd.Trim()) 
                .Distinct()
                .ToList();

            // 2. Query BANK (Pakai NoNota4 sesuai DTO kamu)
            // SQL Server biasanya case-insensitive & ignore trailing space di WHERE clause, jadi Contains aman.
            var bayarBank = await _context.Set<EntriPembayaranBank>()
                .Where(x => listNoNota.Contains(x.NoNota4)) 
                .GroupBy(x => x.NoNota4)
                .Select(g => new BayarDto { NoNota = g.Key, Total = g.Sum(x => x.TotalDlmRupiah ?? 0) })
                .ToListAsync(cancellationToken);

            // 3. Query KAS (Pakai NoNota4 sesuai DTO kamu)
            var bayarKas = await _context.Set<EntriPembayaranKas>()
                .Where(x => listNoNota.Contains(x.NoNota4))
                .GroupBy(x => x.NoNota4)
                .Select(g => new BayarDto { NoNota = g.Key, Total = g.Sum(x => x.TotalDlmRupiah ?? 0) })
                .ToListAsync(cancellationToken);

            // 4. Query PIUTANG (Pakai NoNota - Asumsi EntriPenyelesaianPiutang kolomnya NoNota)
            var bayarPiutang = await _context.Set<EntriPenyelesaianPiutang>()
                .Where(x => listNoNota.Contains(x.NoNota)) 
                .GroupBy(x => x.NoNota)
                .Select(g => new BayarDto { NoNota = g.Key, Total = g.Sum(x => x.TotalBayarRp ?? 0) })
                .ToListAsync(cancellationToken);

            // 5. Gabungkan ke Dictionary dengan TRIM KEY
            var totalPembayaranPerNota = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            void MergeToDict(List<BayarDto> sumberData)
            {
                foreach (var item in sumberData)
                {
                    if (string.IsNullOrWhiteSpace(item.NoNota)) continue;
                    
                    // === INI KUNCINYA: TRIM SPASI DARI DATA PEMBAYARAN ===
                    // Data DB: "N001   " -> Trim -> "N001"
                    var key = item.NoNota.Trim(); 
                    
                    if (totalPembayaranPerNota.ContainsKey(key))
                        totalPembayaranPerNota[key] += item.Total;
                    else
                        totalPembayaranPerNota[key] = item.Total;
                }
            }

            MergeToDict(bayarBank);
            MergeToDict(bayarKas);
            MergeToDict(bayarPiutang);

            // 6. UPDATE DATA LAPORAN (Overwrite item.jumlah)
            foreach (var item in dataLaporan)
            {
                if (string.IsNullOrWhiteSpace(item.no_nd)) {
                    item.jumlah = 0; continue;
                }

                // Data Produksi juga di-Trim saat mau dicocokkan
                var key = item.no_nd.Trim();

                if (totalPembayaranPerNota.ContainsKey(key))
                {
                    // Masukkan nilai hitungan (Bank + Kas + Piutang)
                    item.jumlah = totalPembayaranPerNota[key];
                }
                else
                {
                    item.jumlah = 0;
                }
                
                if (item.saldo == null) item.saldo = 0;
            }

            // =========================================================================================

            // --- 3. Labeling Logic ---
            string displayLabel = "DETAIL (KESELURUHAN)"; 
            switch (request.JenisLaporan)
            {
                case "Pos": displayLabel = "PEMBAWA POS"; break;
                case "Broker": displayLabel = "AGEN/BROKER"; break;
                case "COB": displayLabel = "COB"; break;
                case "Tertanggung": displayLabel = "TERTANGGUNG"; break;
                case "Detail": default: displayLabel = "DETAIL (KESELURUHAN)"; break;
            }

            // --- 4. Generate HTML ---
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
                decimal nBayar = item.jumlah ?? 0; // Nilai ini sudah hasil gabungan & trim
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
                        <td colspan='9' style='text-align:right; padding-right:10px;'>TOTAL :</td>
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
                    detailsBuilder.Append($@"<tr><td colspan='12' style='background-color:#e6f7ff; font-weight:bold; padding-left:10px;'>{displayLabel}: {group.Key}</td></tr>");
                    
                    decimal subNota = 0, subBayar = 0;
                    foreach (var item in group)
                    {
                        detailsBuilder.Append(BuildRowHtml(idx++, item));
                        subNota += (item.saldo ?? 0);
                        subBayar += (item.jumlah ?? 0);
                    }
                    detailsBuilder.Append(BuildSubtotalHtml(group.Key, subNota, subBayar, subNota - subBayar));
                }
            }

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
                JudulLaporan = $"Laporan Outstanding - {request.JenisTransaksi} ({displayLabel})",
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
