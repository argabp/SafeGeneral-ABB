using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ReopenSpreadingOfRisks.Commands
{
    public class ReopenSpreadingOfRiskCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_updt_reas { get; set; }
    }

    public class ReopenSpreadingOfRiskCommandHandler : IRequestHandler<ReopenSpreadingOfRiskCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<ReopenSpreadingOfRiskCommandHandler> _logger;

        public ReopenSpreadingOfRiskCommandHandler(IDbConnectionPst connectionPst,
            ILogger<ReopenSpreadingOfRiskCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ReopenSpreadingOfRiskCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.QueryProc<(string, string, string)>("spp_ri02p_03",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt,
                        request.no_updt_reas
                    })).FirstOrDefault();
            }, _logger);
        }
    }
}