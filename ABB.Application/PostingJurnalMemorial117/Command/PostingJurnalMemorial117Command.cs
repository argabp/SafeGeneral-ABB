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
            var userId = request.KodeUserUpdate ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            foreach (var voucher in request.NoVouchers)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_posting_jurnal_memorial_117 {0}, {1}, {2}",
                    voucher,      // Masuk ke {0} -> @NoVoucher
                    userId,         // Masuk ke {1} -> @KodeUserUpdate
                    tanggalPosting  // Masuk ke {2} -> @TanggalPosting
                );
            }

            return Unit.Value;

            // var recordsToPost = await _context.JurnalMemorial117
            //     .Where(x => request.NoVouchers.Contains(x.NoVoucher) && x.KodeCabang == request.KodeCabang)
            //     .ToListAsync(cancellationToken);

           
            // foreach (var item in recordsToPost)
            // {
            //     item.FlagPosting = true; 
            //     item.KodeUserUpdate = request.KodeUserUpdate;
            //     item.TanggalUpdate = DateTime.Now;
            // }
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}