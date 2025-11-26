using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiKomisiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal nilai_dis { get; set; }
        public decimal pst_kms { get; set; }
    }

    public class GenerateNilaiKomisiQueryHandler : IRequestHandler<GenerateNilaiKomisiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiKomisiQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiKomisiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_22", 
                new { request.nilai_prm, request.nilai_dis, request.pst_kms })).FirstOrDefault();
        }
    }
}