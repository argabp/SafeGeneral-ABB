using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleRoutes.Queries
{
    public class GetRoleRoutesQuery : IRequest<List<RoleRouteDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetRoleRoutesQueryHandle : IRequestHandler<GetRoleRoutesQuery, List<RoleRouteDto>>
    {
        private readonly IDbConnection _db;

        public GetRoleRoutesQueryHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<RoleRouteDto>> Handle(GetRoleRoutesQuery request, CancellationToken cancellationToken)
        {
            var roleRoutes = await _db.QueryProc<RoleRouteDto>("sp_ROLE_GetRoleRoutes", new { request.SearchKeyword });

            return roleRoutes.OrderBy(o => o.RoleName).ToList();
        }
    }
}