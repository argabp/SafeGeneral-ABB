using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelProsesNotaPremiTreaties.Commands
{
    public class CancelProsesNotaPremiTreatyCommand : IRequest<(string, string, string)>
    {
        public DateTime tgl_proses { get; set; }

        public string kd_cob { get; set; }
    }

    public class CancelProsesNotaPremiTreatyCommandHandler : IRequestHandler<CancelProsesNotaPremiTreatyCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelProsesNotaPremiTreatyCommandHandler> _logger;

        public CancelProsesNotaPremiTreatyCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelProsesNotaPremiTreatyCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(CancelProsesNotaPremiTreatyCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spp_ri04p_03",
                    new
                    {
                        request.tgl_proses, request.kd_cob
                    })).FirstOrDefault(), _logger);
        }
    }
}