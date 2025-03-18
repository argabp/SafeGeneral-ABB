using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleModules.Queries
{
    public class GetRolesQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetRolesQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>("SELECT RoleId Value, RoleName Text FROM MS_Role"))
                .ToList();
        }
    }
}