using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RoleModules.Queries
{
    public class GetModulesByRoleQuery : IRequest<List<DropdownOptionDto>>
    {
        public string RoleId { get; set; }
    }

    public class GetModulesByRoleQueryHandler : IRequestHandler<GetModulesByRoleQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetModulesByRoleQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetModulesByRoleQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>(@"SELECT m.Id Value, m.Name Text
		FROM TR_RoleModule rm
			INNER JOIN MS_Module m
				ON rm.ModuleId = m.Id
			WHERE rm.RoleId = @RoleId", new { request.RoleId }))
                .ToList();
        }
    }
}