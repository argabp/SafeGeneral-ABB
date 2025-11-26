using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiDiskonQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal pst_dis { get; set; }
        public decimal pst_kms { get; set; }
    }

    public class GenerateNilaiDiskonQueryHandler : IRequestHandler<GenerateNilaiDiskonQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiDiskonQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiDiskonQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_21", 
                new { request.nilai_prm, request.pst_dis, request.pst_kms })).FirstOrDefault();
        }
    }
}