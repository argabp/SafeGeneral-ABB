using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Queries
{
    public class GetNextVoucherNumberQuery : IRequest<string>
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeCabang { get; set; } // Cukup ini kuncinya
        public string kodeKas { get; set; }
        public string DebetKredit { get; set; }
    }

    public class GetNextVoucherNumberQueryHandler : IRequestHandler<GetNextVoucherNumberQuery, string>
    {
        private readonly IDbContextPstNota _context;

        public GetNextVoucherNumberQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetNextVoucherNumberQuery request, CancellationToken cancellationToken)
        {
            // 1. QUERY DASAR (Hanya filter Cabang, Bulan, Tahun - Untuk Jan & Feb 2026)
            var query = _context.VoucherKas
                .Where(x => x.KodeCabang == request.KodeCabang 
                            && x.TanggalVoucher.HasValue 
                            && x.TanggalVoucher.Value.Month == request.Bulan 
                            && x.TanggalVoucher.Value.Year == request.Tahun
                            && x.NoVoucher != null);

            // 2. PERKONDISIAN UNTUK MARET 2026 KE ATAS (TERMASUK TAHUN DEPAN)
            if (request.Tahun > 2026 || (request.Tahun == 2026 && request.Bulan >= 3))
            {
                // Filter spesifik Kode Kas
                if (!string.IsNullOrEmpty(request.kodeKas))
                {
                    query = query.Where(v => v.KodeKas == request.kodeKas);
                }

                // --- LOGIKA BARU PEMISAH DEBET / KREDIT UNTUK KAS ---
                if (!string.IsNullOrEmpty(request.DebetKredit) && !string.IsNullOrEmpty(request.kodeKas))
                {
                    // Tentukan Prefix (Contoh: "K" + "D" + "01" = "KD01")
                    string prefixTipe = "K";
                    if (request.DebetKredit.ToUpper() == "D") prefixTipe += "D";
                    else if (request.DebetKredit.ToUpper() == "K") prefixTipe += "K";

                    string prefixTengah = prefixTipe + request.kodeKas; // Hasil: "KD01" atau "KK01"

                    // Suruh sistem HANYA mencari NoVoucher yang mengandung "/KD01/" atau "/KK01/"
                    query = query.Where(v => v.NoVoucher.Contains("/" + prefixTengah + "/"));
                }
            }

            // 3. EKSEKUSI PENCARIAN
            var existingVouchers = await query
                .Select(x => x.NoVoucher)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            // 4. MENCARI NOMOR URUT TERBESAR
            foreach (var noVoucher in existingVouchers)
            {
                // Format DB: 50/KK01/02/2026/001
                var parts = noVoucher.Split('/');
                if (parts.Length > 0)
                {
                    var lastPart = parts.Last(); 
                    if (int.TryParse(lastPart, out int currentSeq))
                    {
                        if (currentSeq > maxNumber)
                        {
                            maxNumber = currentSeq;
                        }
                    }
                }
            }

            return (maxNumber + 1).ToString("000");
        }
    }
}