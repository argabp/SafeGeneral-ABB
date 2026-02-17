using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmHHQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_casco { get; set; }
        public decimal pst_rate_hh { get; set; }
        public decimal stn_rate_hh { get; set; }
    }

    public class GenerateNilaiPrmHHQueryHandler : IRequestHandler<GenerateNilaiPrmHHQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmHHQueryHandler> _logger;

        public GenerateNilaiPrmHHQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmHHQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPrmHHQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_24", 
                    new { request.nilai_casco, request.pst_rate_hh, request.stn_rate_hh })).FirstOrDefault();
            }, _logger);
        }
    }
}