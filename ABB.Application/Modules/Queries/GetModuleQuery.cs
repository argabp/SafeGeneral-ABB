using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Modules.Queries
{
    public class GetModuleQuery : IRequest<ModuleDto>
    {
        public int ModuleId { get; set; }
    }

    public class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, ModuleDto>
    {
        private readonly IDbConnection _connection;

        public GetModuleQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ModuleDto> Handle(GetModuleQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<ModuleDto>("Select * From MS_Module WHERE Id = @ModuleId", new { request.ModuleId })).FirstOrDefault();
        }
    }
}