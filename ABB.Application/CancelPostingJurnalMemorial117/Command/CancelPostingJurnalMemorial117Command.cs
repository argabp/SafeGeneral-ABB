using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class CancelPostingJurnalMemorial117Command : IRequest<Unit>
    {
        public List<string> NoVouchers { get; set; } // List No Voucher yang mau di-cancel
        public string KodeCabang { get; set; }
        public string KodeUserUpdate { get; set; }
    }

    public class CancelPostingJurnalMemorial117CommandHandler : IRequestHandler<CancelPostingJurnalMemorial117Command, Unit>
    {
        private readonly IDbContextPstNota _context;

        public CancelPostingJurnalMemorial117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingJurnalMemorial117Command request, CancellationToken cancellationToken)
        {

            var userId = request.KodeUserUpdate ?? "SYSTEM";

            foreach (var voucher in request.NoVouchers)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_jurnal_memorial_117 {0}, {1}",
                    voucher,      // Masuk ke {0} -> @NoVoucher
                    userId     // Masuk ke {1} -> @KodeUserUpdate
                );
            }

            return Unit.Value;
            // var recordsToCancel = await _context.JurnalMemorial117
            //     .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
            //     .ToListAsync(cancellationToken);
            // foreach (var item in recordsToCancel)
            // {
            //     item.FlagPosting = false; 
            //     item.KodeUserUpdate = request.KodeUserUpdate;
            //     item.TanggalUpdate = DateTime.Now;
            // }
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}