using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Approvals.Queries
{
    public class GetKodeStatusQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKodeStatusQueryHandler : IRequestHandler<GetKodeStatusQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetKodeStatusQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeStatusQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select kd_status Value, nm_status Text From MS_Status")).ToList();
        }
    }
}