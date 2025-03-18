using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.RoleRoutes.Commands
{
    public class AddRoleRouteCommand : IRequest
    {
        public string RoleId { get; set; }
        public int RouteId { get; set; }
    }

    public class AddRoleRouteCommandHandler : IRequestHandler<AddRoleRouteCommand>
    {
        private readonly IDbContext _context;

        public AddRoleRouteCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddRoleRouteCommand request, CancellationToken cancellationToken)
        {
            _context.RoleRoute.Add(new RoleRoute { RoleId = request.RoleId, RouteId = request.RouteId });
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}