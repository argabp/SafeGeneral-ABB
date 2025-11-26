using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.Entities;

namespace ABB.Application.PostingPenyelesaianPiutang.Commands
{
    public class PostingPenyelesaianPiutangCommand : IRequest
    {
         public List<string> Data { get; set; } // Berisi daftar NoVoucher
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
           var vouchersToUpdate = await _context.HeaderPenyelesaianUtang
                .Where(v => request.Data.Contains(v.NomorBukti))
                .ToListAsync(cancellationToken);

            // 2. Loop melalui hasil yang ditemukan dan update flag_posting-nya
            foreach (var voucher in vouchersToUpdate)
            {
                voucher.FlagPosting = true; // Atau 'Y', sesuaikan dengan standar Anda
            }

            // 3. Simpan semua perubahan ke database dalam satu kali perintah
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}