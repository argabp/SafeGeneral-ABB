using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaTreatyMasuks.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaTreatyMasuks.Commands
{
    public class CancelPostingNotaTreatyMasukCommand : IRequest
    {
        public List<CancelPostingNotaTreatyMasukModel> Data { get; set; }
    }

    public class CancelPostingNotaTreatyMasukCommandHandler : IRequestHandler<CancelPostingNotaTreatyMasukCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelPostingNotaTreatyMasukCommandHandler> _logger;

        public CancelPostingNotaTreatyMasukCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelPostingNotaTreatyMasukCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(CancelPostingNotaTreatyMasukCommand request, CancellationToken cancellationToken)
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