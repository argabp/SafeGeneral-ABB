using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Approvals.Queries
{
    public class GetKodeUserSignQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKodeUserSignQueryHandler : IRequestHandler<GetKodeUserSignQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetKodeUserSignQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeUserSignQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select UserId Value, ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '') Text From MS_User")).ToList();
        }
    }
}