using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmKoasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_share { get; set; }
        public decimal nilai_prm_leader { get; set; }
        public decimal pst_hf { get; set; }
    }

    public class GenerateNilaiPrmKoasQueryHandler : IRequestHandler<GenerateNilaiPrmKoasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiPrmKoasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiPrmKoasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_10", 
                new { request.pst_share, request.nilai_prm_leader, request.pst_hf })).FirstOrDefault();
        }
    }
}