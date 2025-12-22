using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Commands
{
    public class DeleteJurnalMemorial104DetailCommand : IRequest<Unit>
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
    }

    public class DeleteJurnalMemorial104DetailCommandHandler : IRequestHandler<DeleteJurnalMemorial104DetailCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public DeleteJurnalMemorial104DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteJurnalMemorial104DetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.DetailJurnalMemorial104
                .FirstOrDefaultAsync(x => x.NoVoucher == request.NoVoucher && x.No == request.No, cancellationToken);

            if (entity != null)
            {
                _context.DetailJurnalMemorial104.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}