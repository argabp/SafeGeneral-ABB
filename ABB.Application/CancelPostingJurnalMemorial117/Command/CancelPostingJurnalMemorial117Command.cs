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
            // 1. Ambil data yang sesuai request
            var recordsToCancel = await _context.JurnalMemorial117
                .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
                .ToListAsync(cancellationToken);

            // 2. Loop dan Update statusnya ke FALSE
            foreach (var item in recordsToCancel)
            {
                item.FlagPosting = false; // BALIKKAN JADI FALSE
                
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