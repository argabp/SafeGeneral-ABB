using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleNavigations.Queries
{
    public class GetRoleNavigationsQuery : IRequest<List<RoleNavigationDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetRoleNavigationsQueryHandler : IRequestHandler<GetRoleNavigationsQuery, List<RoleNavigationDto>>
    {
        private readonly IDbConnection _db;

        public GetRoleNavigationsQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<RoleNavigationDto>> Handle(GetRoleNavigationsQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _db.QueryProc<RoleNavigationDto>("sp_ROLE_GetRoleNavigations", new { request.SearchKeyword });

            return result.OrderBy(o => o.RoleName).ToList();
        }
    }
}