using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherBanks.Queries
{
    public class GetNextVoucherNumberQuery : IRequest<string>
    {
        // Parameter yang kita butuhkan untuk mencari
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string KodeCabang { get; set; }
        public string kodeBank { get; set; }
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
            // 1. QUERY DASAR (Filter bawaan untuk Januari & Februari)
            var query = _context.VoucherBank
                .Where(v => v.KodeCabang == request.KodeCabang 
                            && v.TanggalVoucher.HasValue
                            && v.TanggalVoucher.Value.Month == request.Bulan
                            && v.TanggalVoucher.Value.Year == request.Tahun
                            && v.NoVoucher != null);

            // 2. PERKONDISIAN UNTUK BULAN MARET (3) KE ATAS
            // 2. PERKONDISIAN UNTUK MARET 2026 KE ATAS (TERMASUK JANUARI & FEBRUARI 2027 DST)
            if (request.Tahun > 2026 || (request.Tahun == 2026 && request.Bulan >= 3))
            {
                // Jaga-jaga filter berdasarkan Kode Bank
                if (!string.IsNullOrEmpty(request.kodeBank))
                {
                    query = query.Where(v => v.KodeBank == request.kodeBank);
                }

                // --- LOGIKA BARU PEMISAH DEBET / KREDIT ---
                if (!string.IsNullOrEmpty(request.DebetKredit) && !string.IsNullOrEmpty(request.kodeBank))
                {
                    // Tentukan Prefix (Contoh: "B" + "D" + "01" = "BD01")
                    string prefixTipe = "B";
                    if (request.DebetKredit.ToUpper() == "D") prefixTipe += "D";
                    else if (request.DebetKredit.ToUpper() == "K") prefixTipe += "K";

                    string prefixTengah = prefixTipe + request.kodeBank; // Hasil: "BD01" atau "BK01"

                    // Suruh sistem HANYA mencari NoVoucher yang mengandung "/BD01/" atau "/BK01/"
                    query = query.Where(v => v.NoVoucher.Contains("/" + prefixTengah + "/"));
                }
            }

            // 3. EKSEKUSI QUERY KE DATABASE
            var existingVouchers = await query
                .Select(v => v.NoVoucher)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            // 4. MENCARI NOMOR URUT TERBESAR
            foreach (var noVoucher in existingVouchers)
            {
                // Format: 50/BD01/02/2026/001
                var parts = noVoucher.Split('/');
                if (parts.Length > 0)
                {
                    var lastPart = parts.Last(); // Ambil bagian paling ujung (001)
                    if (int.TryParse(lastPart, out int currentSeq))
                    {
                        if (currentSeq > maxNumber)
                        {
                            maxNumber = currentSeq;
                        }
                    }
                }
            }

            // Return Max + 1 (Format 3 digit)
            return (maxNumber + 1).ToString("000");
        }
    }
}