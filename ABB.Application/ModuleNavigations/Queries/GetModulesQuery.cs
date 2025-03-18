using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class GetModulesQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetModulesQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("SELECT Id Value, Name Text FROM MS_Module"))
                .ToList();
        }
    }
}