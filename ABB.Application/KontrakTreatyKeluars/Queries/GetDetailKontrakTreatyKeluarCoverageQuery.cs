using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarCoverageQuery : IRequest<DetailKontrakTreatyKeluarCoverage>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cvrg { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarCoverageQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarCoverageQuery, DetailKontrakTreatyKeluarCoverage>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetDetailKontrakTreatyKeluarCoverageQuery> _logger;

        public GetDetailKontrakTreatyKeluarCoverageQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetDetailKontrakTreatyKeluarCoverageQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<DetailKontrakTreatyKeluarCoverage> Handle(GetDetailKontrakTreatyKeluarCoverageQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var detailKontrakTreatyKeluarCoverage =
                    _dbContextPst.DetailKontrakTreatyKeluarCoverage.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_cvrg);

                return Task.FromResult(detailKontrakTreatyKeluarCoverage);
            }, _logger);
        }
    }
}