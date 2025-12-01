using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TypeCoas.Commands
{
    public class DeleteTypeCoaCommand : IRequest
    {
        public string Type { get; set; }
    }

    public class DeleteTypeCoaCommandHandler : IRequestHandler<DeleteTypeCoaCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteTypeCoaCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTypeCoaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TypeCoa
                .FirstOrDefaultAsync(v => v.Type == request.Type, cancellationToken);

            if (entity != null)
            {
                _context.TypeCoa.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}