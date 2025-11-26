using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GenerateNilaiRecoveryQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }
        public double nilai_org { get; set; }
    }

    public class GenerateNilaiRecoveryQueryHandler : IRequestHandler<GenerateNilaiRecoveryQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNilaiRecoveryQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNilaiRecoveryQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_cl20e_01",
                new { request.kd_mtu, request.nilai_org })).FirstOrDefault();
        }
    }
}