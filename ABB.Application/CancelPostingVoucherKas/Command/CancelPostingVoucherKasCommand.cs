using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.CancelPostingVoucherKas.Commands
{
    public class CancelPostingVoucherKasCommand : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

    public class CancelPostingVoucherKasCommandHandler : IRequestHandler<CancelPostingVoucherKasCommand>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public CancelPostingVoucherKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingVoucherKasCommand request, CancellationToken cancellationToken)
        {
           
            var userId = request.UserId ?? "SYSTEM";

            // Loop setiap NoVoucher yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_voucher_kas {0}, {1}",
                    noVoucher,      // Masuk ke {0} -> @NoVoucher
                    userId       // Masuk ke {1} -> @KodeUserUpdate
                );
            }

            return Unit.Value;
            // var vouchersToUpdate = await _context.VoucherKas
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