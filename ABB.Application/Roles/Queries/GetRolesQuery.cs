using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Roles.Queries
{
    public class GetRolesQuery : IRequest<List<RolesDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RolesDto>>
    {
        private readonly IDbConnection _db;

        public GetRolesQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<RolesDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Query<RolesDto>($@"
                    SELECT r.RoleId,
			                r.RoleCode,
			                r.RoleName,
			                r.Description,
			                r.WorkSpaceId
	                FROM MS_Role r 
	                WHERE  (r.RoleCode like '%'+@SearchKeyword+'%'
			                    OR r.RoleName like '%'+@SearchKeyword+'%'
			                    OR r.Description like '%'+@SearchKeyword+'%'
			                    OR @SearchKeyword = '' OR @SearchKeyword is null
		                    ) 
		                    AND ISNULL(r.IsDeleted,0) = 0 AND ISNULL(r.WorkspaceId,0) = 0
	                ORDER BY r.RoleCode
            ", new { request.SearchKeyword });

            return result.ToList();
        }
    }
}