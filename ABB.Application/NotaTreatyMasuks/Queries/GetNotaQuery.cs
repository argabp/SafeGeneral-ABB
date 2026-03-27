using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasuks.Queries
{
    public class GetNotaQuery : IRequest<NotaTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }
    }

    public class GetNotaQueryHandler : IRequestHandler<GetNotaQuery, NotaTreatyMasuk>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetNotaQuery> _logger;

        public GetNotaQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetNotaQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<NotaTreatyMasuk> Handle(GetNotaQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var notaTreatyMasuk =
                    _dbContextPst.NotaTreatyMasuk.FirstOrDefault(w => w.kd_cb == request.kd_cb
                            && w.kd_thn == request.kd_thn && w.kd_bln == request.kd_bln 
                            && w.kd_jns_sor == request.kd_jns_sor && w.kd_rk_sor == request.kd_tty_msk
                            && w.kd_mtu == request.kd_mtu && w.no_ref_nt == request.no_tr);
                
                return Task.FromResult(notaTreatyMasuk);
            }, _logger);
        }
    }
}