using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelProsesNotaKlaimTreaties.Commands
{
    public class CancelProsesNotaKlaimTreatyCommand : IRequest<(string, string, string)>
    {
        public DateTime tgl_proses { get; set; }
    }

    public class CancelProsesNotaKlaimTreatyCommandHandler : IRequestHandler<CancelProsesNotaKlaimTreatyCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelProsesNotaKlaimTreatyCommandHandler> _logger;

        public CancelProsesNotaKlaimTreatyCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelProsesNotaKlaimTreatyCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(CancelProsesNotaKlaimTreatyCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spp_ri05p_03",
                new
                {
                    request.tgl_proses
                })).FirstOrDefault(), _logger);
        }
    }
}