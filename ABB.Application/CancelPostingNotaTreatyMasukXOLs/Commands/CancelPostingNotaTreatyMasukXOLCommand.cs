using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaTreatyMasukXOLs.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaTreatyMasukXOLs.Commands
{
    public class CancelPostingNotaTreatyMasukXOLCommand : IRequest
    {
        public List<CancelPostingNotaTreatyMasukXOLModel> Data { get; set; }
    }

    public class CancelPostingNotaTreatyMasukXOLCommandHandler : IRequestHandler<CancelPostingNotaTreatyMasukXOLCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelPostingNotaTreatyMasukXOLCommandHandler> _logger;

        public CancelPostingNotaTreatyMasukXOLCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelPostingNotaTreatyMasukXOLCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(CancelPostingNotaTreatyMasukXOLCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_ri01p_03",
                        new
                        {
                            data.kd_cb, data.jns_tr, data.jns_nt_msk,
                            data.kd_thn, data.kd_bln, data.no_nt_msk,
                            data.jns_nt_kel, data.no_nt_kel
                        });
                }
            
                return Unit.Value;
            }, _logger);
        }
    }
}