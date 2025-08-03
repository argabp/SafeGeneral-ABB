using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNamaAngkutQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_angkut { get; set; }
    }

    public class GenerateNamaAngkutQueryHandler : IRequestHandler<GenerateNamaAngkutQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNamaAngkutQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNamaAngkutQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_82", new { kd_kapal = request.kd_angkut })).FirstOrDefault();
        }
    }
}