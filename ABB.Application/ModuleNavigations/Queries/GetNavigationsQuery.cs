using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class GetNavigationsQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetNavigationsQueryHandler : IRequestHandler<GetNavigationsQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetNavigationsQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetNavigationsQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("SELECT NavigationId Value, Text FROM MS_Navigation WHERE ParentId = 0"))
                .ToList();
        }
    }
}