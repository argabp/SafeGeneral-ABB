using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ApprovalKlaims.Queries
{
    public class GetKodeUserSignKlaimQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKodeUserSignKlaimQueryHandler : IRequestHandler<GetKodeUserSignKlaimQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetKodeUserSignKlaimQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeUserSignKlaimQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select UserId Value, ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '') Text From MS_User")).ToList();
        }
    }
}