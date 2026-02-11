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
            // LOGIC BARU: Global Sequence per Cabang & Periode
            // Kita cari semua voucher di Cabang ini pada Bulan & Tahun ini.
            // Tidak peduli D/K atau Kode Kas-nya apa.
            
            var existingVouchers = await _context.VoucherKas
                .Where(x => x.KodeCabang == request.KodeCabang 
                            && x.TanggalVoucher.HasValue 
                            && x.TanggalVoucher.Value.Month == request.Bulan 
                            && x.TanggalVoucher.Value.Year == request.Tahun
                            && x.NoVoucher != null)
                .Select(x => x.NoVoucher)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            foreach (var noVoucher in existingVouchers)
            {
                // Format DB: 50/KK01/02/2026/001
                // Kita mau ambil 3 digit terakhir (001)
                // Cara aman: Split by '/' dan ambil elemen terakhir
                var parts = noVoucher.Split('/');
                if (parts.Length > 0)
                {
                    var lastPart = parts.Last(); // Ambil bagian paling belakang
                    if (int.TryParse(lastPart, out int currentSeq))
                    {
                        if (currentSeq > maxNumber)
                        {
                            maxNumber = currentSeq;
                        }
                    }
                }
            }

            // Return nomor selanjutnya (Max + 1)
            return (maxNumber + 1).ToString("000");
        }
    }
}