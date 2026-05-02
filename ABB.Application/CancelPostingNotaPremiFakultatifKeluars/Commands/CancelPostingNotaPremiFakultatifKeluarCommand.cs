using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Commands
{
    public class CancelPostingNotaPremiFakultatifKeluarCommand : IRequest
    {
        public List<CancelPostingNotaPremiFakultatifKeluarModel> Data { get; set; }
    }

    public class CancelPostingNotaPremiFakultatifKeluarCommandHandler : IRequestHandler<CancelPostingNotaPremiFakultatifKeluarCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelPostingNotaPremiFakultatifKeluarCommandHandler> _logger;

        public CancelPostingNotaPremiFakultatifKeluarCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelPostingNotaPremiFakultatifKeluarCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(CancelPostingNotaPremiFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_ri03p_03",
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