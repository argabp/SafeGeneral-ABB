using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNamaKapalQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }
    }

    public class GenerateNamaKapalQueryHandler : IRequestHandler<GenerateNamaKapalQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNamaKapalQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNamaKapalQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_03a", new { request.kd_kapal })).FirstOrDefault();
        }
    }
}