using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ApprovalKlaims.Queries
{
    public class GetKodeStatusKlaimQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKodeStatusKlaimQueryHandler : IRequestHandler<GetKodeStatusKlaimQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetKodeStatusKlaimQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeStatusKlaimQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select kd_status Value, nm_status Text From MS_StatusKlaim")).ToList();
        }
    }
}