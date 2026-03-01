using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesNotaKlaimTreaties.Commands
{
    public class ProsesNotaKlaimTreatyCommand : IRequest<(string, string, string)>
    {
        public DateTime tgl_proses { get; set; }
    }

    public class ProsesNotaKlaimTreatyCommandHandler : IRequestHandler<ProsesNotaKlaimTreatyCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<ProsesNotaKlaimTreatyCommandHandler> _logger;

        public ProsesNotaKlaimTreatyCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<ProsesNotaKlaimTreatyCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ProsesNotaKlaimTreatyCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spp_ri05p_02",
                new
                {
                    request.tgl_proses
                })).FirstOrDefault(), _logger);
        }
    }
}