using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.Modules.Queries
{
    public class GetModulesQuery : IRequest<List<ModuleDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, List<ModuleDto>>
    {
        private readonly IDbConnection _connection;
        private readonly ICurrentUserService _userService;

        public GetModulesQueryHandler(IDbConnection connection, ICurrentUserService userService)
        {
            _connection = connection;
            _userService = userService;
        }

        public async Task<List<ModuleDto>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.QueryProc<ModuleDto>("sp_MODULE_GetModules", new { request.SearchKeyword })).ToList();
        }
    }
}