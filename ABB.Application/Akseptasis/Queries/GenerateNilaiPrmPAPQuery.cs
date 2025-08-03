using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmPAPQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_pap { get; set; }
        public decimal pst_rate_pap { get; set; }
        public decimal stn_rate_pap { get; set; }
        public byte jml_pap { get; set; }
    }

    public class GenerateNilaiPrmPAPQueryHandler : IRequestHandler<GenerateNilaiPrmPAPQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiPrmPAPQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiPrmPAPQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw09e01_11", 
                new { request.nilai_pap, request.pst_rate_pap, request.stn_rate_pap, request.jml_pap })).FirstOrDefault();
        }
    }
}