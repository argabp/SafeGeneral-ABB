using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EntriNotas.Queries
{
    public class GenerateEntriNotaDataQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GenerateEntriNotaDataQueryHandler : IRequestHandler<GenerateEntriNotaDataQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateEntriNotaDataQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateEntriNotaDataQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_18", 
                new { request.kd_cb, request.kd_grp_rk, request.kd_rk })).FirstOrDefault();
        }
    }
}