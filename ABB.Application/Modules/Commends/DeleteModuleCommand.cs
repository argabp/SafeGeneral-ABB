using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Modules.Commends
{
    public class DeleteModuleCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand>
    {
        private readonly IDbContext _context;

        public DeleteModuleCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Module.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            entity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}