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
    public class PostingJurnalMemorial117Command : IRequest<Unit>
    {
        public List<string> NoVouchers { get; set; } // List No Voucher yang dicentang
        public string KodeCabang { get; set; }
        public string KodeUserUpdate { get; set; }
    }

    public class PostingJurnalMemorial117CommandHandler : IRequestHandler<PostingJurnalMemorial117Command, Unit>
    {
        private readonly IDbContextPstNota _context;

        public PostingJurnalMemorial117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingJurnalMemorial117Command request, CancellationToken cancellationToken)
        {
            // 1. Ambil semua data berdasarkan list NoVoucher
            var recordsToPost = await _context.JurnalMemorial117
                .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
                .ToListAsync(cancellationToken);

            // 2. Loop dan Update statusnya
            foreach (var item in recordsToPost)
            {
                item.FlagPosting = true; // Set jadi Posting
                
                // Audit Trail
                item.KodeUserUpdate = request.KodeUserUpdate;
                item.TanggalUpdate = DateTime.Now;
            }

            // 3. Simpan perubahan (Bulk Update)
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}