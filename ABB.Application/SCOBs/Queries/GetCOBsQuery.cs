using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.SCOBs.Queries
{
    public class GetCOBsQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetCOBsQueryHandler : IRequestHandler<GetCOBsQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCOBsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCOBsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<DropdownOptionDto>("SELECT kd_cob Value, nm_cob + ' / ' + nm_cob_ing Text FROM rf04"))
                .ToList();
        }
    }
}