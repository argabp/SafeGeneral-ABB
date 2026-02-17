using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetFlagPKKQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cvrg { get; set; }
    }

    public class GetFlagPKKQueryHandler : IRequestHandler<GetFlagPKKQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetFlagPKKQueryHandler> _logger;

        public GetFlagPKKQueryHandler(IDbConnectionFactory connectionFactory,
             ILogger<GetFlagPKKQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetFlagPKKQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_48", new { request.kd_cvrg })).First();
            }, _logger);
        }
    }
}   