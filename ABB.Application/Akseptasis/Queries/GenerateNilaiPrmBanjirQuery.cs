using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmBanjirQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_casco { get; set; }
        public decimal pst_rate_banjir { get; set; }
        public decimal stn_rate_banjir { get; set; }
    }

    public class GenerateNilaiPrmBanjirQueryHandler : IRequestHandler<GenerateNilaiPrmBanjirQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmBanjirQueryHandler> _logger;

        public GenerateNilaiPrmBanjirQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmBanjirQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPrmBanjirQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_56", 
                    new { request.nilai_casco, pst_rate_aog = request.pst_rate_banjir, stn_rate_aog = request.stn_rate_banjir })).FirstOrDefault();
            }, _logger);
        }
    }
}