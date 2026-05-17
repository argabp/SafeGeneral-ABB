using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasukXOLs.Queries
{
    public class GetKontrakTreatyMasukXOLQuery : IRequest<KontrakTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }
    }

    public class GetKontrakTreatyMasukXOLQueryHandler : IRequestHandler<GetKontrakTreatyMasukXOLQuery, KontrakTreatyMasuk>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKontrakTreatyMasukXOLQuery> _logger;

        public GetKontrakTreatyMasukXOLQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetKontrakTreatyMasukXOLQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KontrakTreatyMasuk> Handle(GetKontrakTreatyMasukXOLQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var kontrakTreatyMasukXOL =
                    _dbContextPst.KontrakTreatyMasuk.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_msk);

                return Task.FromResult(kontrakTreatyMasukXOL);
            }, _logger);
        }
    }
}