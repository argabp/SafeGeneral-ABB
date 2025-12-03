using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class ProsesUlangPembayaranPiutangCommand : IRequest<int>
    {
        public string NoBukti { get; set; }
    }

    public class ProsesUlangPembayaranPiutangCommandHandler : IRequestHandler<ProsesUlangPembayaranPiutangCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public ProsesUlangPembayaranPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(ProsesUlangPembayaranPiutangCommand request, CancellationToken cancellationToken)
        {

            var voucherHeader = await _context.HeaderPenyelesaianUtang
            .FirstOrDefaultAsync(v => v.NomorBukti == request.NoBukti, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");
                
            voucherHeader.FlagFinal = false;
            _context.HeaderPenyelesaianUtang.Update(voucherHeader);

          
          var existingFinal = await _context.EntriPenyelesaianPiutang
                .Where(x => x.NoBukti == request.NoBukti)
                .ToListAsync(cancellationToken);

            _context.EntriPenyelesaianPiutang.RemoveRange(existingFinal);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);

            // Kembalikan jumlah data detail yang berhasil dipindah
            return existingFinal.Count;
        }
    }
}
