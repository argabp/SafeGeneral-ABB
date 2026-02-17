using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNamaAngkutQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_angkut { get; set; }
    }

    public class GenerateNamaAngkutQueryHandler : IRequestHandler<GenerateNamaAngkutQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNamaAngkutQueryHandler> _logger;

        public GenerateNamaAngkutQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNamaAngkutQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNamaAngkutQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_82", new { kd_kapal = request.kd_angkut })).FirstOrDefault();
            }, _logger);
        }
    }
}