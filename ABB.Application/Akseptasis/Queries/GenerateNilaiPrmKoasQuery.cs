using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmKoasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_share { get; set; }
        public decimal nilai_prm_leader { get; set; }
        public decimal pst_hf { get; set; }
    }

    public class GenerateNilaiPrmKoasQueryHandler : IRequestHandler<GenerateNilaiPrmKoasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmKoasQueryHandler> _logger;

        public GenerateNilaiPrmKoasQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmKoasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPrmKoasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_10", 
                    new { request.pst_share, request.nilai_prm_leader, request.pst_hf })).FirstOrDefault();
            }, _logger);
        }
    }
}