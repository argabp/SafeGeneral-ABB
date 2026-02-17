using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class ClosingAkseptasiCommand : IRequest<(string, string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public string no_updt { get; set; }
    }

    public class ClosingAkseptasiCommandHandler : IRequestHandler<ClosingAkseptasiCommand, (string, string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;
    
        private readonly ILogger<ClosingAkseptasiCommandHandler> _logger;

        public ClosingAkseptasiCommandHandler(IDbConnectionFactory connectionFactory,
            ILogger<ClosingAkseptasiCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ClosingAkseptasiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<(string, string, string)>("spp_uw01p_02",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_aks, request.no_updt
                })).FirstOrDefault();
            }, _logger);
        }
    }
}