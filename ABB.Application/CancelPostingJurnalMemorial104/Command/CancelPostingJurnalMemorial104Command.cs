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
            // 1. Ambil data yang sesuai request
            var recordsToCancel = await _context.JurnalMemorial104
                .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
                .ToListAsync(cancellationToken);

            // 2. Loop dan Update statusnya ke FALSE
            foreach (var item in recordsToCancel)
            {
                item.FlagGL = false; // BALIKKAN JADI FALSE
                
                // Audit Trail
                item.KodeUserUpdate = request.KodeUserUpdate;
                item.TanggalUpdate = DateTime.Now;
            }

            // 3. Simpan perubahan
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}