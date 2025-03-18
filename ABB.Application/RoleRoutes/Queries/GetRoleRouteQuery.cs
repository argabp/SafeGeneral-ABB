using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleRoutes.Queries
{
    public class GetRoleRouteQuery : IRequest<List<RoutesDto>>
    {
        public string RoleId { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetRoleRouteQueryHandle : IRequestHandler<GetRoleRouteQuery, List<RoutesDto>>
    {
        private readonly IDbConnection _db;

        public GetRoleRouteQueryHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<RoutesDto>> Handle(GetRoleRouteQuery request, CancellationToken cancellationToken)
        {
            var routes = await _db.QueryProc<RoutesDto>("sp_ROLE_GetRoleRoute", new { request.RoleId, request.SearchKeyword });

            return routes.ToList();
        }
    }
}