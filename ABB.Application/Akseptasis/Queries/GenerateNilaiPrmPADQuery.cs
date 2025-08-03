using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmPADQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_pad { get; set; }
        public decimal pst_rate_pad { get; set; }
        public decimal stn_rate_pad { get; set; }
    }

    public class GenerateNilaiPrmPADQueryHandler : IRequestHandler<GenerateNilaiPrmPADQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiPrmPADQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiPrmPADQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw09e01_13", 
                new { request.nilai_pad, request.pst_rate_pad, request.stn_rate_pad })).FirstOrDefault();
        }
    }
}