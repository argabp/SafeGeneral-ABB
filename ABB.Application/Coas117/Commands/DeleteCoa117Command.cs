using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas117.Commands
{
    public class DeleteCoa117Command : IRequest
    {
        public string Kode { get; set; }
    }

    public class DeleteCoa117CommandHandler : IRequestHandler<DeleteCoa117Command>
    {
        private readonly IDbContextPstNota _context;

        public DeleteCoa117CommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCoa117Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.Coa117
                .FirstOrDefaultAsync(v => v.gl_kode == request.Kode, cancellationToken);

            if (entity != null)
            {
                _context.Coa117.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}