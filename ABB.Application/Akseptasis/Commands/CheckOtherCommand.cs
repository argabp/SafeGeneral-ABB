using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class CheckOtherCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public decimal pst_share { get; set; }
    }

    public class CheckOtherCommandHandler : IRequestHandler<CheckOtherCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<CheckOtherCommandHandler> _logger;

        public CheckOtherCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<CheckOtherCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<(string, string)> Handle(CheckOtherCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_04",
                    new
                    {
                        request.kd_cob, request.kd_scob, request.pst_share
                    })).First();

                return result;
            }, _logger);
        }
    }
}