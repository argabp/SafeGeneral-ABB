using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class GetModuleNavigationsQuery : IRequest<List<ModuleNavigationViewDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetModuleNavigationsQueryHandler : IRequestHandler<GetModuleNavigationsQuery, List<ModuleNavigationViewDto>>
    {
        private readonly IDbConnection _connection;

        public GetModuleNavigationsQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<ModuleNavigationViewDto>> Handle(GetModuleNavigationsQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.QueryProc<ModuleNavigationViewDto>("sp_MODULE_NAVIGATION_GetModuleNavigations",
                new { request.SearchKeyword })).ToList();
        }
    }
}