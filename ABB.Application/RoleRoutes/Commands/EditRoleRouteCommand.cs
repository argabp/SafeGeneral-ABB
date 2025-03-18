using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.RoleRoutes.Commands
{
    public class EditRoleRouteCommand : IRequest
    {
        public EditRoleRouteCommand()
        {
            Routes = new List<int>();
        }
        public string RoleId { get; set; }
        public List<int> Routes { get; set; }
        public List<RoleRoute> RoleRouteTemp { get; set; }
    }
    public class EditRoleRouteCommandHandler : IRequestHandler<EditRoleRouteCommand>
    {
        private readonly IDbContext _context;
        public EditRoleRouteCommandHandler(IDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(EditRoleRouteCommand request, CancellationToken cancellationToken)
        {
            request.RoleRouteTemp = new List<RoleRoute>();

            DeleteRoleRoutes(request.RoleId);

            foreach (var route in request.Routes)
            {
                var isExist = request.RoleRouteTemp.Any(a => a.RoleId == request.RoleId && a.RouteId == route);
                if (!isExist)
                    request.RoleRouteTemp.Add(new RoleRoute()
                    {
                        RoleId = request.RoleId,
                        RouteId = route
                    });
            }

            _context.RoleRoute.AddRange(request.RoleRouteTemp);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        private void DeleteRoleRoutes(string roleId)
        {
            var roleRoute = _context.RoleRoute.Where(w => w.RoleId == roleId);

            _context.RoleRoute.RemoveRange(roleRoute);
        }
    }
}