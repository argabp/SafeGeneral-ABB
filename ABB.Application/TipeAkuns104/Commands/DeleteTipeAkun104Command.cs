using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TipeAkuns104.Commands
{
    public class DeleteTipeAkun104Command : IRequest
    {
        public string Kode { get; set; }
    }

    public class DeleteTipeAkun104CommandHandler : IRequestHandler<DeleteTipeAkun104Command>
    {
        private readonly IDbContextPstNota _context;

        public DeleteTipeAkun104CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTipeAkun104Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.TipeAkun104
                .FirstOrDefaultAsync(v => v.Kode == request.Kode, cancellationToken);

            if (entity != null)
            {
                _context.TipeAkun104.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}