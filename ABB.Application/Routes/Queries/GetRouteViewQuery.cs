using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Routes.Queries
{
    public class GetRouteViewQuery : IRequest<List<RouteViewDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetRouteViewHandler : IRequestHandler<GetRouteViewQuery, List<RouteViewDto>>
    {
        private readonly IDbConnection _db;

        public GetRouteViewHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<RouteViewDto>> Handle(GetRouteViewQuery request, CancellationToken cancellationToken)
        {
            var sql =
                @"SELECT RouteId, Controller + ' - ' + [Action] as RouteName ,Controller + '/' + [Action] as Route INTO #temp FROM [MS_Route]
                        
                        SELECT * FROM #temp WHERE (RouteName like '%'+@SearchKeyword+'%'
			            OR @SearchKeyword = '' OR @SearchKeyword is null) 
                        OR (Route like '%'+@SearchKeyword+'%'
			            OR @SearchKeyword = '' OR @SearchKeyword is null) ORDER BY RouteId DESC
                        
                        DROP TABLE #temp
                        ";
            var result = await _db.Query<RouteViewDto>(sql, new { request.SearchKeyword });
            return result.ToList();
        }
    }
}