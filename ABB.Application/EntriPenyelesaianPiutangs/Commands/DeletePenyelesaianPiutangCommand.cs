using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class DeletePenyelesaianPiutangCommand : IRequest
    {
        public int  No { get; set; }          // Id detail pembayaran
        public string NoBukti { get; set; }   // Nomor voucher header
    }

    public class DeletePenyelesaianPiutangCommandHandler : IRequestHandler<DeletePenyelesaianPiutangCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeletePenyelesaianPiutangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePenyelesaianPiutangCommand request, CancellationToken cancellationToken)
        {
            if (request.No <= 0 || string.IsNullOrEmpty(request.NoBukti))
            {
                 return Unit.Value;
            }

            // Cari detail pembayaran berdasarkan Id dan NoVoucher
            var entity = await _context.EntriPenyelesaianPiutang
                .FirstOrDefaultAsync(v => v.No == request.No && v.NoBukti == request.NoBukti, cancellationToken);

            if (entity != null)
            {
                _context.EntriPenyelesaianPiutang.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
