using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class CopyEndorsInsertCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public int no_updt { get; set; }
        public int no_rsk { get; set; }
        public string kd_endt { get; set; }
        public string flag_endt { get; set; }
    }

    public class CopyEndorsInsertCommandHandler : IRequestHandler<CopyEndorsInsertCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<CopyEndorsInsertCommandHandler> _logger;

        public CopyEndorsInsertCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<CopyEndorsInsertCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CopyEndorsInsertCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("spe_uw02e_49",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_aks, request.no_updt,
                    request.no_rsk, request.kd_endt, request.flag_endt
                });

                return Unit.Value;
            }, _logger);

        }
    }
}