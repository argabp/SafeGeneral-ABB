using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKlaimReasuransis.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKlaimReasuransis.Commands
{
    public class CancelPostingNotaKlaimReasuransiCommand : IRequest
    {
        public List<CancelPostingNotaKlaimReasuransiModel> Data { get; set; }
    }

    public class CancelPostingNotaKlaimReasuransiCommandHandler : IRequestHandler<CancelPostingNotaKlaimReasuransiCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelPostingNotaKlaimReasuransiCommandHandler> _logger;

        public CancelPostingNotaKlaimReasuransiCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelPostingNotaKlaimReasuransiCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(CancelPostingNotaKlaimReasuransiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_cl05p_03",
                        new
                        {
                            data.kd_cb, data.kd_cob, data.kd_scob,
                            data.kd_thn, data.no_kl, data.no_mts
                        });
                }
            
                return Unit.Value;
            }, _logger);
        }
    }
}