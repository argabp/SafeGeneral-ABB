using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ModuleNavigations.Queries
{
    public class GetNavigationByModuleQuery : IRequest<List<DropdownOptionDto>>
    {
        public int ModuleId { get; set; }
    }

    public class GetNavigationByModuleQueryHandler : IRequestHandler<GetNavigationByModuleQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _connection;

        public GetNavigationByModuleQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetNavigationByModuleQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<DropdownOptionDto>(@"SELECT n.NavigationId Value, n.Text
		FROM TR_ModuleNavigation mn
			INNER JOIN MS_Navigation n
				ON mn.NavigationId = n.NavigationId
			WHERE mn.ModuleId = @ModuleId", new { request.ModuleId }))
                .ToList();
        }
    }
}