using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    // 1. DEFINISI QUERY
    public class GetTotalPembayaranQuery : IRequest<decimal>
    {
        public string no_bukti { get; set; }
        public string PiutangDK { get; set; }
    }

    // 2. HANDLER UNTUK QUERY
    public class GetTotalPembayaranQueryHandler : IRequestHandler<GetTotalPembayaranQuery, decimal>
    {
        private readonly IDbContextPstNota _context;

        public GetTotalPembayaranQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

       public async Task<decimal> Handle(GetTotalPembayaranQuery request, CancellationToken cancellationToken)
    {
        // 1. Bersihkan dan Validasi Voucher D/K
        var PiutangDK = request.PiutangDK?.Trim();

        // 2. JIKA voucherDK KOSONG, kirim error. Ini akan menunjukkan
        //    bahwa JavaScript Anda masih mengirim data yang salah.
        if (string.IsNullOrEmpty(PiutangDK))
        {
            throw new System.Exception("GetTotalPembayaranQuery gagal: PiutangDK tidak boleh kosong.");
        }

        // 3. Ambil data dari tabel temp
        var payments = await _context.EntriPenyelesaianPiutangTemp
            .Where(pb => pb.NoBukti == request.no_bukti)
            .Select(pb => new { pb.TotalBayarOrg, pb.DebetKredit })
            .ToListAsync(cancellationToken);

        decimal total = 0;
        foreach (var p in payments)
        {
            var nilai = (decimal)(p.TotalBayarOrg ?? 0); 
            var paymentDK = p.DebetKredit?.Trim();
            
            // 4. Lakukan logika perhitungan (yang sudah benar)
            if (paymentDK != PiutangDK)
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