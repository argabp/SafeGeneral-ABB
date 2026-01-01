using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.CancelPostingVoucherBank.Commands
{
    public class CancelPostingVoucherBankCommand : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

    public class CancelPostingVoucherBankCommandHandler : IRequestHandler<CancelPostingVoucherBankCommand>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public CancelPostingVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingVoucherBankCommand request, CancellationToken cancellationToken)
        {

             // Ambil User ID yang sedang login
            var userId = request.UserId ?? "SYSTEM";
            // var tanggalPosting = DateTime.Now;

            // Loop setiap NoVoucher yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_voucher_bank {0}, {1}",
                    noVoucher,      // Masuk ke {0} -> @NoVoucher
                    userId      // Masuk ke {1} -> @KodeUserUpdate
                );
            }

            return Unit.Value;
          
            // var vouchersToUpdate = await _context.VoucherBank
            //     .Where(v => request.Data.Contains(v.NoVoucher))
            //     .ToListAsync(cancellationToken);

            // foreach (var voucher in vouchersToUpdate)
            // {
            //     voucher.FlagPosting = false;
            // }
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}