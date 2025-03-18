using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleModules.Commands
{
    public class DeleteRoleModuleCommand : IRequest
    {
        public string RoleId { get; set; }
    }

    public class DeleteRoleModuleCommandHandler : IRequestHandler<DeleteRoleModuleCommand>
    {
        private readonly IDbContext _context;

        public DeleteRoleModuleCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRoleModuleCommand request, CancellationToken cancellationToken)
        {
            var entities = _context.RoleModule.Where(roleModule => roleModule.RoleId == request.RoleId).ToList();

            _context.RoleModule.RemoveRange(entities);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}