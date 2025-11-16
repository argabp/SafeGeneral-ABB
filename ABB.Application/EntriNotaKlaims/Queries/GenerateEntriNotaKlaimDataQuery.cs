using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EntriNotaKlaims.Queries
{
    public class GenerateEntriNotaKlaimDataQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GenerateEntriNotaKlaimDataQueryHandler : IRequestHandler<GenerateEntriNotaKlaimDataQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateEntriNotaKlaimDataQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateEntriNotaKlaimDataQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_cl07e_01", 
                new { request.kd_cb, request.kd_grp_rk, request.kd_rk })).FirstOrDefault();
        }
    }
}