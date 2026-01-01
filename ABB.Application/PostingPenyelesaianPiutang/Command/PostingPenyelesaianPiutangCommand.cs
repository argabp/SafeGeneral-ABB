using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.Entities;
using System;

namespace ABB.Application.PostingPenyelesaianPiutang.Commands
{
    public class PostingPenyelesaianPiutangCommand : IRequest
    {
         public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

   

    public class PostingPenyelesaianPiutangCommandHandler : IRequestHandler<PostingPenyelesaianPiutangCommand>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public PostingPenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostingPenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {

            // Ambil User ID yang sedang login
            var userId = request.UserId ?? "SYSTEM";
            var tanggalPosting = DateTime.Now;

            // Loop setiap NoVoucher yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_posting_penyelesaian_piutang {0}, {1}, {2}",
                    noVoucher,      // Masuk ke {0} -> @NoVoucher
                    userId,         // Masuk ke {1} -> @KodeUserUpdate
                    tanggalPosting  // Masuk ke {2} -> @TanggalPosting
                );
            }

            return Unit.Value;
        //    var vouchersToUpdate = await _context.HeaderPenyelesaianUtang
        //         .Where(v => request.Data.Contains(v.NomorBukti))
        //         .ToListAsync(cancellationToken);
        //     foreach (var voucher in vouchersToUpdate)
        //     {
        //         voucher.FlagPosting = true;
        //     }
        //     await _context.SaveChangesAsync(cancellationToken);

        //     return Unit.Value;
        }
    }
}