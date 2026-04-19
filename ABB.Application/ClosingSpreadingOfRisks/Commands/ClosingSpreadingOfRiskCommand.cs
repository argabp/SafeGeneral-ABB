using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ClosingSpreadingOfRisks.Commands
{
    public class ClosingSpreadingOfRiskCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public short no_updt_reas { get; set; }

        public int jk_bln { get; set; }
    }

    public class ClosingSpreadingOfRiskCommandHandler : IRequestHandler<ClosingSpreadingOfRiskCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<ClosingSpreadingOfRiskCommandHandler> _logger;

        public ClosingSpreadingOfRiskCommandHandler(IDbConnectionPst connectionPst,
            ILogger<ClosingSpreadingOfRiskCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ClosingSpreadingOfRiskCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.QueryProc<(string, string, string)>("spp_ri02p_02",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt,
                        request.tgl_closing, request.no_updt_reas,
                        request.jk_bln, user = "sa"
                    })).FirstOrDefault();
            }, _logger);
        }
    }
}