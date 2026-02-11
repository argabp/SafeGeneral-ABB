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
            // 1. SIAPKAN POLA PENCARIAN (Global per Cabang & Periode)
            // Format DB: SMT/Cabang/...../Bulan/Tahun/Urut
            
            // Ambil 2 digit cabang (misal "50")
            var cabangFmt = request.KodeCabang.Length >= 2 
                            ? request.KodeCabang.Substring(request.KodeCabang.Length - 2) 
                            : request.KodeCabang;

            var bulanFmt = request.Bulan.ToString("00");
            var tahunFmt = request.Tahun.ToString();

            // Kita cari voucher yang:
            // 1. Depannya "SMT/{Cabang}/"
            // 2. Mengandung "/{Bulan}/{Tahun}/"
            
            string prefixAwal = $"SMT/{cabangFmt}/";
            string suffixPeriode = $"/{bulanFmt}/{tahunFmt}/";

            // Ambil semua kandidat dari DB (Filter kasar dulu biar ringan)
            var candidates = await _context.VoucherKas
                .Where(x => x.NoVoucherSementara != null 
                            && x.NoVoucherSementara.StartsWith(prefixAwal)
                            && x.NoVoucherSementara.Contains(suffixPeriode))
                .Select(x => x.NoVoucherSementara)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            foreach (var noSmt in candidates)
            {
                // Format: SMT/50/KK01/02/2026/001
                // Split berdasarkan '/'
                var parts = noSmt.Split('/');
                
                // Urutan biasanya ada di paling belakang
                if (parts.Length > 0)
                {
                    var lastPart = parts.Last(); // Ambil "001"
                    if (int.TryParse(lastPart, out int currentSeq))
                    {
                        if (currentSeq > maxNumber)
                        {
                            maxNumber = currentSeq;
                        }
                    }
                }
            }

            return maxNumber + 1; // Return Max + 1
        }
    }
}