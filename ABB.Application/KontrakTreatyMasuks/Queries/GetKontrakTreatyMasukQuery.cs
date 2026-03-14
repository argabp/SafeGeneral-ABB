using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasuks.Queries
{
    public class GetKontrakTreatyMasukQuery : IRequest<KontrakTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }
    }

    public class GetKontrakTreatyMasukQueryHandler : IRequestHandler<GetKontrakTreatyMasukQuery, KontrakTreatyMasuk>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKontrakTreatyMasukQuery> _logger;

        public GetKontrakTreatyMasukQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetKontrakTreatyMasukQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KontrakTreatyMasuk> Handle(GetKontrakTreatyMasukQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var kontrakTreatyMasuk =
                    _dbContextPst.KontrakTreatyMasuk.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_msk);

                return Task.FromResult(kontrakTreatyMasuk);
            }, _logger);
        }
    }
}