using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    public class DeleteJurnalMemorial117DetailCommand : IRequest<Unit>
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
    }

    public class DeleteJurnalMemorial117DetailCommandHandler : IRequestHandler<DeleteJurnalMemorial117DetailCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public DeleteJurnalMemorial117DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteJurnalMemorial117DetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial117Detail
                .FirstOrDefaultAsync(x => x.NoVoucher == request.NoVoucher && x.No == request.No, cancellationToken);

            if (entity != null)
            {
                _context.JurnalMemorial117Detail.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}