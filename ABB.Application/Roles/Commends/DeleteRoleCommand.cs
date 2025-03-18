using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Roles.Commends
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IDbContext _context;

        public DeleteRoleCommandHandler(IDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            if (_context.RoleNavigation.Any(a => a.RoleId == request.Id))
                return false;

            var role = _context.Role.Find(request.Id);
            role.IsDeleted = true;
            role.NormalizedName = role.NormalizedName + "DELETED";

            RemoveRoleNavigation(request.Id);
            RemoveRoleRoute(request.Id);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        private void RemoveRoleNavigation(string Id)
        {
            var roleNavigations = _context.RoleNavigation.Where(w => w.RoleId == Id).ToList();
            _context.RoleNavigation.RemoveRange(roleNavigations);
        }
        private void RemoveRoleRoute(string Id)
        {
            var roleRoute = _context.RoleRoute.Where(w => w.RoleId == Id).ToList();
            _context.RoleRoute.RemoveRange(roleRoute);
        }
    }
}