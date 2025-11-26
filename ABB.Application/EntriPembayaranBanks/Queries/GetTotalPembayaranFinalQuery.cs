using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    // 1. DEFINISI QUERY
    public class GetTotalPembayaranFinalQuery : IRequest<decimal>
    {
        public string NoVoucher { get; set; }
        public string VoucherDK { get; set; }
    }

    // 2. HANDLER UNTUK QUERY
    public class GetTotalPembayaranFinalQueryHandler : IRequestHandler<GetTotalPembayaranFinalQuery, decimal>
    {
        private readonly IDbContextPstNota _context;

        public GetTotalPembayaranFinalQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

       public async Task<decimal> Handle(GetTotalPembayaranFinalQuery request, CancellationToken cancellationToken)
    {
        // 1. Bersihkan dan Validasi Voucher D/K
        var voucherDK = request.VoucherDK?.Trim();

        // 2. JIKA voucherDK KOSONG, kirim error. Ini akan menunjukkan
        //    bahwa JavaScript Anda masih mengirim data yang salah.
        if (string.IsNullOrEmpty(voucherDK))
        {
            throw new System.Exception("GetTotalPembayaranQuery gagal: VoucherDK tidak boleh kosong.");
        }

        // 3. Ambil data dari tabel temp
        var payments = await _context.EntriPembayaranBank
            .Where(pb => pb.NoVoucher == request.NoVoucher)
            .Select(pb => new { pb.TotalBayar, pb.DebetKredit })
            .ToListAsync(cancellationToken);

        decimal total = 0;
        foreach (var p in payments)
        {
            var nilai = (decimal)(p.TotalBayar ?? 0); 
            var paymentDK = p.DebetKredit?.Trim();
            
            // 4. Lakukan logika perhitungan (yang sudah benar)
            if (paymentDK != voucherDK)
            {
                // Berbeda (D vs K, atau K vs D) -> MENAMBAH
                total += nilai;
            }
            else
            {
                // Sama (D vs D, atau K vs K) -> MENGURANGI
                total -= nilai;
            }
        }
        
        return total;
    }
    }
}