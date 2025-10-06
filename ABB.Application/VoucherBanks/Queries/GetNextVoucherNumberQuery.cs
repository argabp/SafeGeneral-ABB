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
            // Format yang dicari di database, contoh: "....../09/25"
            var searchTerm = $"/{request.Bulan:D2}/{request.Tahun:D2}";

            var lastVoucher = await _context.VoucherBank
                .Where(v => v.NoVoucher.EndsWith(searchTerm))
                .OrderByDescending(v => v.NoVoucher)
                .FirstOrDefaultAsync(cancellationToken);

            int nextNumber = 1;
            if (lastVoucher != null)
            {
                // Ambil bagian nomor urut dari NoVoucher terakhir
                // Contoh: "10/B01/BD/BD20/09/25" -> ambil "B01"
                var parts = lastVoucher.NoVoucher.Split('/');
                if (parts.Length > 1)
                {
                    // Ambil nomor dari kode bank, contoh B01 -> 1
                    var lastNumberStr = new string(parts[1].Where(char.IsDigit).ToArray());
                    if (int.TryParse(lastNumberStr, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }
            }

            // Format menjadi 3 digit dengan angka nol di depan, contoh: 1 -> "001"
            return nextNumber.ToString("D3");
        }
    }
}