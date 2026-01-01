using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.CancelPostingJurnalMemorial104.Commands
{
    public class CancelPostingJurnalMemorial104Command : IRequest<Unit>
    {
            public List<string> NoVouchers { get; set; }
            public string KodeCabang { get; set; }
            public string KodeUserUpdate { get; set; }
    }

    public class CancelPostingJurnalMemorial104CommandHandler : IRequestHandler<CancelPostingJurnalMemorial104Command, Unit>
    {
        private readonly IDbContextPstNota _context;

        public CancelPostingJurnalMemorial104CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingJurnalMemorial104Command request, CancellationToken cancellationToken)
        {

            var userId = request.KodeUserUpdate ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            foreach (var voucher in request.NoVouchers)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_jurnal_memorial_104 {0}, {1}",
                    voucher,      // Masuk ke {0} -> @NoVoucher
                    userId       // Masuk ke {1} -> @KodeUserUpdate
                   
                );
            }

            return Unit.Value;

            // var recordsToCancel = await _context.JurnalMemorial104
            //     .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
            //     .ToListAsync(cancellationToken);
            // foreach (var item in recordsToCancel)
            // {
            //     item.FlagGL = false; 
            //     item.KodeUserUpdate = request.KodeUserUpdate;
            //     item.TanggalUpdate = DateTime.Now;
            // }
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}