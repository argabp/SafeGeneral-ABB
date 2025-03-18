using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetLoggedInNavigationQuery : IRequest<List<LoggedInNavigationDto>>
    {
        public string Username { get; set; }

        public string ModuleId { get; set; }
    }

    public class
        GetLoggedInNavigationQueryHandler : IRequestHandler<GetLoggedInNavigationQuery, List<LoggedInNavigationDto>>
    {
        private readonly IDbConnection _db;

        public GetLoggedInNavigationQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<LoggedInNavigationDto>> Handle(GetLoggedInNavigationQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _db.QueryProc<LoggedInNavigationDto>("sp_NAV_GetNavigation", new { request.Username, ModuleId = Convert.ToInt32(request.ModuleId) });
            return result.ToList().OrderBy(o => o.Sort).ToList();
        }
    }
}