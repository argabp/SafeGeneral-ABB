using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ReopenAlokasiKlaimReasuransis.Commands
{
    public class ReopenAlokasiKlaimReasuransiCommand : IRequest<(string, string, string)>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
    }

    public class ReopenAlokasiKlaimReasuransiCommandHandler : IRequestHandler<ReopenAlokasiKlaimReasuransiCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<ReopenAlokasiKlaimReasuransiCommandHandler> _logger;

        public ReopenAlokasiKlaimReasuransiCommandHandler(IDbConnectionPst connectionPst,
            ILogger<ReopenAlokasiKlaimReasuransiCommandHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ReopenAlokasiKlaimReasuransiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _connectionPst.QueryProc<(string, string, string)>("spp_cl04p_03",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts
                })).FirstOrDefault(), _logger);
        }
    }
}