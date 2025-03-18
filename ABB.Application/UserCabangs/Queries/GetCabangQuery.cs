using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.UserCabangs.Queries
{
    public class GetCabangQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetCabangQueryHandler : IRequestHandler<GetCabangQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetCabangQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCabangQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("Select kd_cb Value, nm_cb Text From rf01")).ToList();
        }
    }
}