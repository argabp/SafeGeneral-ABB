using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Cabangs.Queries
{
    public class GetCabangsQuery : IRequest<List<CabangDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetCabangsQueryHandler : IRequestHandler<GetCabangsQuery, List<CabangDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCabangsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CabangDto>> Handle(GetCabangsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<CabangDto>("sp_CABANG_GetCabangs", new { request.SearchKeyword })).ToList();
        }
    }
}