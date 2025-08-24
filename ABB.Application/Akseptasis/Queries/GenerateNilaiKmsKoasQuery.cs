using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiKmsKoasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_kms { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal nilai_dis { get; set; }
    }

    public class GenerateNilaiKmsKoasQueryHandler : IRequestHandler<GenerateNilaiKmsKoasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiKmsKoasQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiKmsKoasQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_12", 
                new { request.pst_kms, request.nilai_prm, request.nilai_dis })).FirstOrDefault();
        }
    }
}