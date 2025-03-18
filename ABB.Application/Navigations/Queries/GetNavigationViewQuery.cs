using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetNavigationViewQuery : IRequest<List<NavigationViewDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetNavigationViewQueryHandler : IRequestHandler<GetNavigationViewQuery, List<NavigationViewDto>>
    {
        private readonly IDbConnection _db;

        public GetNavigationViewQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<NavigationViewDto>> Handle(GetNavigationViewQuery request,
            CancellationToken cancellationToken)
        {
            var sql = @"SELECT[NavigationId]
              ,[Text]
              ,[Icon]
              ,CASE WHEN [IsActive]=0 THEN 'Inactive' ELSE 'Active' END as IsActive
	          , stuff((SELECT ',' + Text FROM MS_Navigation subnav WHERE subnav.ParentId = nav.NavigationId for XML path('')), 1, 1, '') as SubNavigation
                ,ParentId
              INTO #tempNav
              FROM [MS_Navigation] nav

              SELECT NavigationId,Text, Icon, IsActive, SubNavigation FROM #tempNav WHERE [Text] like '%'+@SearchKeyword+'%'
			                        OR @SearchKeyword = '' OR @SearchKeyword is null
                                    OR Subnavigation like '%' + @SearchKeyword + '%'
                                    OR @SearchKeyword = '' OR @SearchKeyword is null
                                   ORDER BY ParentId,NavigationId ASC
             DROP TABLE #tempNav";
            var result = await _db.Query<NavigationViewDto>(sql, new { request.SearchKeyword });
            return result.ToList();
        }
    }
}