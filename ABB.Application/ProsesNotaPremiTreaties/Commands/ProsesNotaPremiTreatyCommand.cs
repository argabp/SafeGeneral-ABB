using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesNotaPremiTreaties.Commands
{
    public class ProsesNotaPremiTreatyCommand : IRequest<(string, string, string)>
    {
        public DateTime tgl_proses { get; set; }

        public string kd_cob { get; set; }
    }

    public class ProsesNotaPremiTreatyCommandHandler : IRequestHandler<ProsesNotaPremiTreatyCommand, (string, string, string)>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<ProsesNotaPremiTreatyCommandHandler> _logger;

        public ProsesNotaPremiTreatyCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<ProsesNotaPremiTreatyCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<(string, string, string)> Handle(ProsesNotaPremiTreatyCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _dbConnectionPst.QueryProc<(string, string, string)>("spp_ri04p_04",
                    new
                    {
                        request.tgl_proses, request.kd_cob
                    })).FirstOrDefault(), _logger);
        }
    }
}