using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ABB.Application.PostingJurnalMemorial104.Commands
{
    public class PostingJurnalMemorial104Command : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

    public class PostingJurnalMemorial104CommandHandler : IRequestHandler<PostingJurnalMemorial104Command>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public PostingJurnalMemorial104CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingJurnalMemorial104Command request, CancellationToken cancellationToken)
        {

            var userId = request.UserId ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            foreach (var voucher in request.Data)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_posting_jurnal_memorial_104 {0}, {1}, {2}",
                    voucher,      // Masuk ke {0} -> @NoVoucher
                    userId,         // Masuk ke {1} -> @KodeUserUpdate
                    tanggalPosting  // Masuk ke {2} -> @TanggalPosting
                );
            }

            return Unit.Value;
           
            // var vouchersToUpdate = await _context.JurnalMemorial104
            //     .Where(v => request.Data.Contains(v.NoVoucher))
            //     .ToListAsync(cancellationToken);
            // foreach (var voucher in vouchersToUpdate)
            // {
            //     voucher.FlagGL = true; 
            //     voucher.TanggalPosting = DateTime.Now;
            // }
            // await _context.SaveChangesAsync(cancellationToken);

            // return Unit.Value;
        }
    }
}