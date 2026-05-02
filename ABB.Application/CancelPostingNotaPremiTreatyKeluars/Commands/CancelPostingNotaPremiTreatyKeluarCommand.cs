using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiTreatyKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiTreatyKeluars.Commands
{
    public class CancelPostingNotaPremiTreatyKeluarCommand : IRequest
    {
        public List<CancelPostingNotaPremiTreatyKeluarModel> Data { get; set; }
    }

    public class CancelPostingNotaPremiTreatyKeluarCommandHandler : IRequestHandler<CancelPostingNotaPremiTreatyKeluarCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<CancelPostingNotaPremiTreatyKeluarCommandHandler> _logger;

        public CancelPostingNotaPremiTreatyKeluarCommandHandler(IDbConnectionPst dbConnectionPst, 
            ILogger<CancelPostingNotaPremiTreatyKeluarCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(CancelPostingNotaPremiTreatyKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_ri06p_03",
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