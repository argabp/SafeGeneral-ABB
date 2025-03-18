using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetRouteDropdownQuery : IRequest<List<DropdownDto>>
    {
    }
    public class GetRouteDropdownHandler : IRequestHandler<GetRouteDropdownQuery, List<DropdownDto>>
    {
        private readonly IDbConnection _db;
        public GetRouteDropdownHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<DropdownDto>> Handle(GetRouteDropdownQuery request, CancellationToken cancellationToken)
        {
            var sql = @"SELECT RouteId as Id, Controller + ' - ' + [Action] as Description FROM [MS_Route]
                        ORDER BY Controller,Action ASC";
            var result = await _db.Query<DropdownDto>(sql);
            return result.ToList();
        }
    }
}