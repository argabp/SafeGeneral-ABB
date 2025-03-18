using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.UserCabangs.Queries
{
    public class GetUserQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetUserQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select UserId Value, FirstName + ' ' + LastName Text From MS_User")).ToList();
        }
    }
}