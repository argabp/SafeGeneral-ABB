using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriMappings.Commands
{
    public class DeleteEntriMappingCommand : IRequest
    {
        public string gl_akun104 { get; set; }
    }

    public class DeleteEntriMappingCommandHandler : IRequestHandler<DeleteEntriMappingCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteEntriMappingCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEntriMappingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.EntriMapping
                .FirstOrDefaultAsync(v => v.gl_akun104 == request.gl_akun104, cancellationToken);

            if (entity != null)
            {
                _context.EntriMapping.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}