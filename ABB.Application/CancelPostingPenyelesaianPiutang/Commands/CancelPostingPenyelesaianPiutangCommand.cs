using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.CancelPostingPenyelesaianPiutang.Commands
{
    public class CancelPostingPenyelesaianPiutangCommand : IRequest
    {
        // Properti DatabaseName tidak lagi diperlukan
        // public string DatabaseName { get; set; }

        public List<string> Data { get; set; } // Berisi daftar NoVoucher
        public string UserId { get; set; }
    }

    public class CancelPostingPenyelesaianPiutangCommandHandler : IRequestHandler<CancelPostingPenyelesaianPiutangCommand>
    {
        // DIGANTI: Gunakan IDbContextPstNota
        private readonly IDbContextPstNota _context;

        public CancelPostingPenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelPostingPenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {

            // Ambil User ID yang sedang login
            var userId = request.UserId ?? "SYSTEM";

            // Loop setiap NoVoucher yang dikirim dari client
            foreach (var noVoucher in request.Data)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_penyelesaian_piutang {0}, {1}",
                    noVoucher,      // Masuk ke {0} -> @NoVoucher
                    userId        // Masuk ke {1} -> @KodeUserUpdate
                );
            }

            return Unit.Value;
           
            // var vouchersToUpdate = await _context.HeaderPenyelesaianUtang
            //     .Where(v => request.Data.Contains(v.NomorBukti))
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