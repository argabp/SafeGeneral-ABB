using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleNavigations.Queries
{
    public class GetNavigationsQuery : IRequest<List<NavigationDto>>
    {
    }

    public class GetNavigationsQueryHandler : IRequestHandler<GetNavigationsQuery, List<NavigationDto>>
    {
        private readonly IDbConnection _db;

        public GetNavigationsQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<NavigationDto>> Handle(GetNavigationsQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT NavigationId, Text + ' - ' + ISNULL((SELECT Controller FROM MS_Route WHERE RouteId = n.RouteId), '') AS Text 
                            FROM MS_Navigation n
                          WHERE ParentId = 0";

            var menus = (await _db.Query<NavigationDto>(sql)).OrderBy(o => o.Text).ToList();

            return menus;
        }
    }
}