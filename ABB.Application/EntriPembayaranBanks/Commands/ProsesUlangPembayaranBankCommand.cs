using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Commands
{
    public class ProsesUlangPembayaranBankCommand : IRequest<int>
    {
        public string NoVoucher { get; set; }
    }

    public class ProsesUlangPembayaranBankCommandHandler : IRequestHandler<ProsesUlangPembayaranBankCommand, int>
    {
        private readonly IDbContextPstNota _context;

        public ProsesUlangPembayaranBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<int> Handle(ProsesUlangPembayaranBankCommand request, CancellationToken cancellationToken)
        {
            var voucherHeader = await _context.VoucherBank
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (voucherHeader == null)
                throw new Exception("Voucher Induk tidak ditemukan.");

            // 1. Panggil SP Reverse Saldo SEBELUM data dihapus
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_prosesulangpembayaranbank @p0", 
                new[] { request.NoVoucher }, 
                cancellationToken
            );

            voucherHeader.FlagFinal = false;
            _context.VoucherBank.Update(voucherHeader);

            var existingFinal = await _context.EntriPembayaranBank
                .Where(x => x.NoVoucher == request.NoVoucher)
                .ToListAsync(cancellationToken);

            if (existingFinal.Any())
            {
                _context.EntriPembayaranBank.RemoveRange(existingFinal);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return existingFinal.Count;
        }
    }
}
