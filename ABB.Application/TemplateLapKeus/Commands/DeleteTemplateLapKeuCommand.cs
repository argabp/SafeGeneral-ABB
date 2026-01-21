using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities; // Pastikan using ini ada
using MediatR;

namespace ABB.Application.TemplateLapKeus.Commands
{
    public class DeleteTemplateLapKeuCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteTemplateLapKeuCommandHandler : IRequestHandler<DeleteTemplateLapKeuCommand>
    {
        private readonly IDbContextPstNota _context;

        public DeleteTemplateLapKeuCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTemplateLapKeuCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TemplateLapKeu.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TemplateLapKeu), request.Id);
            }

            _context.TemplateLapKeu.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}