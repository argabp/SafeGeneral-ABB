using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.SCOBs.Queries
{
    public class GetSCOBsQuery : IRequest<List<SCOBDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetSCOBsQueryHandler : IRequestHandler<GetSCOBsQuery, List<SCOBDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetSCOBsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<SCOBDto>> Handle(GetSCOBsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<SCOBDto>("sp_SCOB_GetSCOBs", new { request.SearchKeyword })).ToList();
        }
    }
}