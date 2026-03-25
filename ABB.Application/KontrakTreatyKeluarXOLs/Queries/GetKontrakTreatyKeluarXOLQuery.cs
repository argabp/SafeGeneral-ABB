using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class GetKontrakTreatyKeluarXOLQuery : IRequest<KontrakTreatyKeluarXOL>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }
    }

    public class GetKontrakTreatyKeluarXOLQueryHandler : IRequestHandler<GetKontrakTreatyKeluarXOLQuery, KontrakTreatyKeluarXOL>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKontrakTreatyKeluarXOLQuery> _logger;

        public GetKontrakTreatyKeluarXOLQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetKontrakTreatyKeluarXOLQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KontrakTreatyKeluarXOL> Handle(GetKontrakTreatyKeluarXOLQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var kontrakTreatyKeluarXOL =
                    _dbContextPst.KontrakTreatyKeluarXOL.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_npps);

                return Task.FromResult(kontrakTreatyKeluarXOL);
            }, _logger);
        }
    }
}