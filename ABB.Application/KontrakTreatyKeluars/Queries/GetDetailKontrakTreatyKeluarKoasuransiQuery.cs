using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarKoasuransiQuery : IRequest<DetailKontrakTreatyKeluarKoasuransi>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public int no_urut { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarKoasuransiQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarKoasuransiQuery, DetailKontrakTreatyKeluarKoasuransi>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetDetailKontrakTreatyKeluarKoasuransiQuery> _logger;

        public GetDetailKontrakTreatyKeluarKoasuransiQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetDetailKontrakTreatyKeluarKoasuransiQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<DetailKontrakTreatyKeluarKoasuransi> Handle(GetDetailKontrakTreatyKeluarKoasuransiQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var detailKontrakTreatyKeluarKoasuransi =
                    _dbContextPst.DetailKontrakTreatyKeluarKoasuransi.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.no_urut);

                return Task.FromResult(detailKontrakTreatyKeluarKoasuransi);
            }, _logger);
        }
    }
}