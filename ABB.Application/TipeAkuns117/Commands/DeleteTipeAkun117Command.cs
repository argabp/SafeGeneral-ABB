using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.TipeAkuns117.Commands
{
    public class DeleteTipeAkun117Command : IRequest<Unit>
    {
        public string Kode { get; set; }
    }

    public class DeleteTipeAkun117CommandHandler : IRequestHandler<DeleteTipeAkun117Command, Unit>
    {
        private readonly IDbContextPstNota _context;

        public DeleteTipeAkun117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTipeAkun117Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.TipeAkun117.FindAsync(new object[] { request.Kode }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TipeAkun117), request.Kode);
            }

            _context.TipeAkun117.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}