using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherBanks.Queries
{
    // 1. REQUEST (Parameter yang dibutuhkan untuk menghitung urutan)
    public class GetNextVoucherSementaraNumberQuery : IRequest<int>
    {
        public string KodeCabang { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
    }

    // 2. HANDLER (Logika Pencarian Database)
    public class GetNextVoucherSementaraNumberQueryHandler : IRequestHandler<GetNextVoucherSementaraNumberQuery, int>
    {
        private readonly IDbContextPstNota _context;

        public GetNextVoucherSementaraNumberQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetNextVoucherSementaraNumberQuery request, CancellationToken cancellationToken)
        {
            // A. RAKIT POLA PENCARIAN
            // Format DB: SMT/Cabang/...../Bulan/Tahun/Urut
            
            // Ambil 2 digit cabang (misal "50")
            var cabangFmt = request.KodeCabang.Length >= 2 
                            ? request.KodeCabang.Substring(request.KodeCabang.Length - 2) 
                            : request.KodeCabang;

            var bulanFmt = request.Bulan.ToString("00");
            var tahunFmt = request.Tahun.ToString();

            // Pola dasar: Harus diawali "SMT/50/" dan mengandung "/02/2026/"
            string prefixAwal = $"SMT/{cabangFmt}/";
            string suffixPeriode = $"/{bulanFmt}/{tahunFmt}/";

            // B. AMBIL DATA DARI DB
            var candidates = await _context.VoucherBank
                .Where(x => x.NoVoucherSementara != null 
                            && x.NoVoucherSementara.StartsWith(prefixAwal)
                            && x.NoVoucherSementara.Contains(suffixPeriode))
                .Select(x => x.NoVoucherSementara)
                .ToListAsync(cancellationToken);

            // C. CARI NOMOR TERTINGGI
            int maxNumber = 0;

            foreach (var noSmt in candidates)
            {
                // Contoh: SMT/50/BD01/02/2026/001
                // Split berdasarkan '/'
                var parts = noSmt.Split('/');
                
                // Urutan ada di paling belakang
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