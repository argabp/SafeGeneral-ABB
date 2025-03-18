using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Commands
{
    public class DeleteModuleNavigationCommand : IRequest
    {
        public int ModuleId { get; set; }
    }

    public class DeleteModuleNavigationCommandHandler : IRequestHandler<DeleteModuleNavigationCommand>
    {
        private readonly IDbContext _context;

        public DeleteModuleNavigationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteModuleNavigationCommand request, CancellationToken cancellationToken)
        {
            var entities = _context.ModuleNavigation.Where(roleModule => roleModule.ModuleId == request.ModuleId).ToList();

            _context.ModuleNavigation.RemoveRange(entities);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}