using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNamaKapalQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }
    }

    public class GenerateNamaKapalQueryHandler : IRequestHandler<GenerateNamaKapalQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNamaKapalQueryHandler> _logger;

        public GenerateNamaKapalQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNamaKapalQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNamaKapalQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_03a", new { request.kd_kapal })).FirstOrDefault();
            }, _logger);
        }
    }
}