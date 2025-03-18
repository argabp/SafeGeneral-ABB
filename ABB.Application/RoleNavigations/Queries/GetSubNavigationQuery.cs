using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleNavigations.Queries
{
    public class GetSubNavigationQuery : IRequest<List<NavigationDto>>
    {
        public int ParentId { get; set; }
    }

    public class GetSubNavigationQueryHandler : IRequestHandler<GetSubNavigationQuery, List<NavigationDto>>
    {
        private readonly IDbConnection _db;

        public GetSubNavigationQueryHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<NavigationDto>> Handle(GetSubNavigationQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT NavigationId, Text FROM MS_Navigation WHERE ParentId = @ParentId";

            var menus = (await _db.Query<NavigationDto>(sql, new { request.ParentId })).OrderBy(o => o.Text).ToList();

            return menus;
        }
    }
}