using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherBanks.Commands
{
    public class DeleteVoucherBankCommand : IRequest
    {
        public string NoVoucher { get; set; }
    }

    public class DeleteVoucherBankCommandHandler : IRequestHandler<DeleteVoucherBankCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteVoucherBankCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteVoucherBankCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.VoucherBank
                .FirstOrDefaultAsync(v => v.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity != null)
            {
                _context.VoucherBank.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}