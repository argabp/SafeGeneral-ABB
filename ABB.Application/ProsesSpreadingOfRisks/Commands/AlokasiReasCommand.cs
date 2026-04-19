using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesSpreadingOfRisks.Commands
{
    public class AlokasiReasCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public DateTime? tgl_closing { get; set; }
    }

    public class AlokasiReasCommandHandler : IRequestHandler<AlokasiReasCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<AlokasiReasCommandHandler> _logger;

        public AlokasiReasCommandHandler(IDbConnectionPst connectionPst,
            ILogger<AlokasiReasCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(AlokasiReasCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.QueryProc<(string, string, string)>("spp_ri04p_07",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_pol, request.no_updt,
                        tgl_closing_reas = request.tgl_closing, st_tty = 'Y',
                        flag_survey = 'Y'
                    })).FirstOrDefault();
            }, _logger);
        }
    }
}