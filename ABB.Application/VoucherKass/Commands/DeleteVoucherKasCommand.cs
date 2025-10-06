using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Commands
{
    public class DeleteVoucherKasCommand : IRequest
    {
        public string NoVoucher { get; set; }
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
            var entity = await _context.VoucherKas
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity != null)
            {
                _context.VoucherKas.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}