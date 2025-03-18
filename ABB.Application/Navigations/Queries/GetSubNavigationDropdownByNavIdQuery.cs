using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Navigations.Queries
{
    public class GetSubNavigationDropdownByNavIdQuery : IRequest<List<DropdownDto>>
    {
        public int NavigationId { get; set; }
    }

    public class
        GetSubNavigationDropdownByNavIdHandler : IRequestHandler<GetSubNavigationDropdownByNavIdQuery,
            List<DropdownDto>>
    {
        private readonly IDbConnection _db;

        public GetSubNavigationDropdownByNavIdHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<DropdownDto>> Handle(GetSubNavigationDropdownByNavIdQuery request,
            CancellationToken cancellationToken)
        {
            var sql =
                @$"SELECT NavigationId as Id,Text as Description FROM [MS_Navigation] WHERE IsActive=1 AND ParentId = {request.NavigationId}
                        ORDER BY Text ASC";
            var result = await _db.Query<DropdownDto>(sql);
            return result.ToList();
        }
    }
}