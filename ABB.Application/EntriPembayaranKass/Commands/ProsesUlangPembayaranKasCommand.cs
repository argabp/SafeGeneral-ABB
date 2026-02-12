using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Commands
{
    public class ProsesUlangPembayaranKasCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
    }

    public class ProsesUlangPembayaranKasCommandHandler : IRequestHandler<ProsesUlangPembayaranKasCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public ProsesUlangPembayaranKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(ProsesUlangPembayaranKasCommand request, CancellationToken cancellationToken)
        {
            var voucherHeader = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");

            // 1. PANGGIL SP UNTUK BALIKIN SALDO (REVERSE)
            // Kita panggil SP SEBELUM datanya dihapus dari tabel EntriPembayaranKas
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_prosesulangpembayarankas @p0", 
                new[] { request.NoVoucher }, 
                cancellationToken
            );

            // 2. Update status header jadi Un-Final
            voucherHeader.FlagFinal = false;
            _context.VoucherKas.Update(voucherHeader);

            // 3. Ambil dan Hapus data Final
            var existingFinal = await _context.EntriPembayaranKas
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (existingFinal.Any())
            {
                _context.EntriPembayaranKas.RemoveRange(existingFinal);
            }

            // 4. Simpan perubahan ke DB
            await _context.SaveChangesAsync(cancellationToken);

            return existingFinal.Count;
        }
    }
}
