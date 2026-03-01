using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Queries
{
    public class GetNextVoucherSementaraNumberQuery : IRequest<int>
    {
        public string KodeCabang { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public string kodeKas { get; set; }
        public string debetKredit { get; set; }
        // Parameter KodeKas dan DebetKredit DIHAPUS dari sini karena tidak dipakai buat filter urutan
    }

    public class GetNextVoucherSementaraNumberQueryHandler : IRequestHandler<GetNextVoucherSementaraNumberQuery, int>
    {
        private readonly IDbContextPstNota _context;

        public GetNextVoucherSementaraNumberQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

       public async Task<int> Handle(GetNextVoucherSementaraNumberQuery request, CancellationToken cancellationToken)
        {
            var cabangFmt = request.KodeCabang.Length >= 2
                ? request.KodeCabang.Substring(request.KodeCabang.Length - 2)
                : request.KodeCabang;

            var bulanFmt = request.Bulan.ToString("00");
            var tahunFmt = request.Tahun.ToString();

            // Setup pola dasar pencarian: "SMT/50/" dan "/03/2026/"
            string prefixAwal = $"SMT/{cabangFmt}/";
            string suffixPeriode = $"/{bulanFmt}/{tahunFmt}/";

            // 1. QUERY DASAR (Filter Cabang, Bulan, Tahun - Aturan Lama)
            var query = _context.VoucherKas
                .Where(x => x.NoVoucherSementara != null
                            && x.NoVoucherSementara.StartsWith(prefixAwal)
                            && x.NoVoucherSementara.Contains(suffixPeriode));

            // 2. PERKONDISIAN UNTUK MARET 2026 KE ATAS (TERMASUK TAHUN DEPAN)
            if (request.Tahun > 2026 || (request.Tahun == 2026 && request.Bulan >= 3))
            {
                // --- LOGIKA BARU PEMISAH DEBET / KREDIT UNTUK KAS ---
                if (!string.IsNullOrEmpty(request.debetKredit) && !string.IsNullOrEmpty(request.kodeKas))
                {
                    // Tentukan Prefix (Contoh: "K" + "D" + "01" = "KD01")
                    string prefixTipe = "K";
                    if (request.debetKredit.ToUpper() == "D") prefixTipe += "D";
                    else if (request.debetKredit.ToUpper() == "K") prefixTipe += "K";

                    string prefixTengah = prefixTipe + request.kodeKas; // Hasil: "KD01" atau "KK01"

                    // Suruh sistem HANYA mencari Voucher SMT yang mengandung "/KD01/" atau "/KK01/"
                    query = query.Where(x => x.NoVoucherSementara.Contains("/" + prefixTengah + "/"));
                }
            }

            // 3. EKSEKUSI PENCARIAN KE DATABASE
            var candidates = await query
                .Select(x => x.NoVoucherSementara)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            // 4. MENCARI NOMOR URUT TERBESAR
            foreach (var noSmt in candidates)
            {
                var parts = noSmt.Split('/');

                if (parts.Length > 0)
                {
                    var lastPart = parts.Last(); // Ambil bagian paling ujung (misal "001")

                    if (int.TryParse(lastPart, out int currentSeq))
                    {
                        if (currentSeq > maxNumber)
                            maxNumber = currentSeq;
                    }
                }
            }

            return maxNumber + 1; // Langsung kembalikan integer (Controller yg ubah jadi "000")
        }

    }
}