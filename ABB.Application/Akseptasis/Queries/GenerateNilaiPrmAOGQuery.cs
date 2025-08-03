using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmAOGQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_casco { get; set; }
        public decimal pst_rate_aog { get; set; }
        public decimal stn_rate_aog { get; set; }
    }

    public class GenerateNilaiPrmAOGQueryHandler : IRequestHandler<GenerateNilaiPrmAOGQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiPrmAOGQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiPrmAOGQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_52", 
                new { request.nilai_casco, request.pst_rate_aog, request.stn_rate_aog })).FirstOrDefault();
        }
    }
}