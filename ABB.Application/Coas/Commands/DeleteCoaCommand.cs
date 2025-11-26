using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas.Commands
{
    public class DeleteCoaCommand : IRequest
    {
        public string Kode { get; set; }
    }

    public class DeleteCoaCommandHandler : IRequestHandler<DeleteCoaCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCoaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Coa
                .FirstOrDefaultAsync(v => v.gl_kode == request.Kode, cancellationToken);

            if (entity != null)
            {
                _context.Coa.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}