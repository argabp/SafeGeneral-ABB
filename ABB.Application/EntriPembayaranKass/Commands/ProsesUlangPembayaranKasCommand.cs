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
                
            voucherHeader.FlagFinal = false;
            _context.VoucherKas.Update(voucherHeader);

          
          var existingFinal = await _context.EntriPembayaranKas
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            _context.EntriPembayaranKas.RemoveRange(existingFinal);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);

            // Kembalikan jumlah data detail yang berhasil dipindah
            return existingFinal.Count;
        }
    }
}
