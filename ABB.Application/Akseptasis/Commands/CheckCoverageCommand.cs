using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class CheckCoverageCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
    }

    public class CheckCoverageCommandHandler : IRequestHandler<CheckCoverageCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<CheckCoverageCommandHandler> _logger;

        public CheckCoverageCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<CheckCoverageCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<(string, string)> Handle(CheckCoverageCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
             {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_06",
                    new
                    {
                        request.kd_cob, request.kd_scob
                    })).First();

                return result;
             }, _logger);
        }
    }
}