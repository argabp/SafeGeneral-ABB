using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiDisKoasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_dis { get; set; }
        public decimal nilai_prm { get; set; }
    }

    public class GenerateNilaiDisKoasQueryHandler : IRequestHandler<GenerateNilaiDisKoasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiDisKoasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiDisKoasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_11", 
                new { request.pst_dis, request.nilai_prm })).FirstOrDefault();
        }
    }
}