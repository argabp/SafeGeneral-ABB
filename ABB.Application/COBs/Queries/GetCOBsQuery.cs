using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.COBs.Queries
{
    public class GetCOBsQuery : IRequest<List<COBDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetCOBsQueryHandler : IRequestHandler<GetCOBsQuery, List<COBDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCOBsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<COBDto>> Handle(GetCOBsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<COBDto>("sp_COB_GetCOBs", new { request.SearchKeyword })).ToList();
        }
    }
}