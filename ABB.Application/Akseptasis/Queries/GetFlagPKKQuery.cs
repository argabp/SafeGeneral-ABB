using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetFlagPKKQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cvrg { get; set; }
    }

    public class GetFlagPKKQueryHandler : IRequestHandler<GetFlagPKKQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetFlagPKKQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetFlagPKKQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_48", new { request.kd_cvrg })).First();
        }
    }
}