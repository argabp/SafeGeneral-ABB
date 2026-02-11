using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Commands
{
    public class DeleteVoucherKasCommand : IRequest
    {
        // GANTI INI: Jangan hapus by NoVoucher, tapi by ID
        public long Id { get; set; } 
    }

    public class DeleteVoucherKasCommandHandler : IRequestHandler<DeleteVoucherKasCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteVoucherKasCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteVoucherKasCommand request, CancellationToken cancellationToken)
        {
            // CARI BERDASARKAN ID
            var entity = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (entity != null)
            {
                _context.VoucherKas.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}