using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarTableOfLimitQuery : IRequest<DetailKontrakTreatyKeluarTableOfLimit>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string category_rsk { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarTableOfLimitQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarTableOfLimitQuery, DetailKontrakTreatyKeluarTableOfLimit>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetDetailKontrakTreatyKeluarTableOfLimitQuery> _logger;

        public GetDetailKontrakTreatyKeluarTableOfLimitQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetDetailKontrakTreatyKeluarTableOfLimitQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<DetailKontrakTreatyKeluarTableOfLimit> Handle(GetDetailKontrakTreatyKeluarTableOfLimitQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var detailKontrakTreatyKeluarTableOfLimit =
                    _dbContextPst.DetailKontrakTreatyKeluarTableOfLimit.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_okup, request.category_rsk, request.kd_kls_konstr);

                return Task.FromResult(detailKontrakTreatyKeluarTableOfLimit);
            }, _logger);
        }
    }
}