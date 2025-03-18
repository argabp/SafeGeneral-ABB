using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.Modules.Queries
{
    public class GetModuleByUserQuery : IRequest<List<ModuleDto>>
    {
    }

    public class GetModuleByUserQueryHandler : IRequestHandler<GetModuleByUserQuery, List<ModuleDto>>
    {
        private readonly IDbConnection _connection;
        private readonly ICurrentUserService _userService;

        public GetModuleByUserQueryHandler(IDbConnection connection, ICurrentUserService userService)
        {
            _connection = connection;
            _userService = userService;
        }

        public async Task<List<ModuleDto>> Handle(GetModuleByUserQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.QueryProc<ModuleDto>("sp_MODULE_GetModulesByUser", new { _userService.UserId })).ToList();
        }
    }
}