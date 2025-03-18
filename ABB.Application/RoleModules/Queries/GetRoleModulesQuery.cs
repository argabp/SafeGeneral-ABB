using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Cabangs.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleModules.Queries
{
    public class GetRoleModulesQuery : IRequest<List<RoleModuleViewDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetRoleModulesQueryHandler : IRequestHandler<GetRoleModulesQuery, List<RoleModuleViewDto>>
    {
        private readonly IDbConnection _connection;

        public GetRoleModulesQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<RoleModuleViewDto>> Handle(GetRoleModulesQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.QueryProc<RoleModuleViewDto>("sp_ROLE_MODULE_GetRoleModules", new { request.SearchKeyword })).ToList();
        }
    }
}