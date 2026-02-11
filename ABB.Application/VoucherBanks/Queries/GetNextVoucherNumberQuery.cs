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
            // LOGIC BARU: Cari Voucher di Cabang ini & Periode ini
            
            var existingVouchers = await _context.VoucherBank
                .Where(v => v.KodeCabang == request.KodeCabang 
                            && v.TanggalVoucher.HasValue
                            && v.TanggalVoucher.Value.Month == request.Bulan
                            && v.TanggalVoucher.Value.Year == request.Tahun
                            && v.NoVoucher != null)
                .Select(v => v.NoVoucher)
                .ToListAsync(cancellationToken);

            int maxNumber = 0;

            foreach (var noVoucher in existingVouchers)
            {
                // Format: 50/BD01/02/2026/001
                // Ambil bagian paling belakang (Sequence)
                var parts = noVoucher.Split('/');
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

            // Return Max + 1 (Format 3 digit)
            return (maxNumber + 1).ToString("000");
        }
    }
}