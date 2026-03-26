using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetKontrakTreatyKeluarQuery : IRequest<KontrakTreatyKeluar>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetKontrakTreatyKeluarQueryHandler : IRequestHandler<GetKontrakTreatyKeluarQuery, KontrakTreatyKeluar>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKontrakTreatyKeluarQuery> _logger;

        public GetKontrakTreatyKeluarQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetKontrakTreatyKeluarQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KontrakTreatyKeluar> Handle(GetKontrakTreatyKeluarQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var kontrakTreatyKeluar =
                    _dbContextPst.KontrakTreatyKeluar.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps);

                return Task.FromResult(kontrakTreatyKeluar);
            }, _logger);
        }
    }
}