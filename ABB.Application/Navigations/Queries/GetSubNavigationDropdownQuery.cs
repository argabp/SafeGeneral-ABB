using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetSubNavigationDropdownQuery : IRequest<List<DropdownDto>>
    {
        public int NavigationId { get; set; }
    }
    public class GetSubNavigationDropdownHandler : IRequestHandler<GetSubNavigationDropdownQuery, List<DropdownDto>>
    {
        private readonly IDbConnection _db;
        public GetSubNavigationDropdownHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<DropdownDto>> Handle(GetSubNavigationDropdownQuery request, CancellationToken cancellationToken)
        {
            var sql = @$"SELECT NavigationId as Id, Text + ' - ' + Controller  as Description FROM [MS_Navigation] n 
                            INNER JOIN MS_Route r
		                        ON n.RouteId = r.RouteId
                            WHERE n.IsActive=1 AND NavigationId <>{request.NavigationId}
                        ORDER BY Text ASC";
            var result = await _db.Query<DropdownDto>(sql);
            return result.ToList();
        }
    }
}