using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasukXOLs.Queries
{
    public class GetNotaTreatyMasukXOLQuery : IRequest<TransaksiTreatyMasuk>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }
    }

    public class GetNotaTreatyMasukXOLQueryHandler : IRequestHandler<GetNotaTreatyMasukXOLQuery, TransaksiTreatyMasuk>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetNotaTreatyMasukXOLQuery> _logger;

        public GetNotaTreatyMasukXOLQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetNotaTreatyMasukXOLQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<TransaksiTreatyMasuk> Handle(GetNotaTreatyMasukXOLQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var notaTreatyMasuk =
                    _dbContextPst.TransaksiTreatyMasuk.Find(request.kd_cb, request.kd_thn, request.kd_bln, request.kd_jns_sor,
                        request.kd_tty_msk, request.kd_mtu, request.no_tr);
                
                return Task.FromResult(notaTreatyMasuk);
            }, _logger);
        }
    }
}