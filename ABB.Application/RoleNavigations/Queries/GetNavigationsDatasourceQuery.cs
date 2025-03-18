using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleNavigations.Queries
{
    public class GetNavigationsDatasourceQuery : IRequest<List<NavigationsDatasourceDto>>
    {
        public string RoleId { get; set; }
    }

    public class GetNavigationsDatasourceQueryHandler : IRequestHandler<GetNavigationsDatasourceQuery, List<NavigationsDatasourceDto>>
    {
        private readonly IDbConnection _db;

        public GetNavigationsDatasourceQueryHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<NavigationsDatasourceDto>> Handle(GetNavigationsDatasourceQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT rn.RoleId, rn.NavigationId, n.ParentId, n.Text FROM TR_RoleNavigation rn
	                    INNER JOIN MS_Navigation n ON rn.NavigationId = n.NavigationId 
                        WHERE rn.RoleId = @RoleId";

            var navigations = (await _db.Query<NavigationsDatasourceDto>(sql, new { request.RoleId })).OrderBy(o => o.Text).ToList();

            return navigations;
        }
    }
}