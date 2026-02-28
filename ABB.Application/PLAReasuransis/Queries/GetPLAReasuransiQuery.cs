using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PLAReasuransis.Queries
{
    public class GetPLAReasuransiQuery : IRequest<PLAReasuransi>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_pla { get; set; }
    }

    public class GetPLAReasuransiQueryHandler : IRequestHandler<GetPLAReasuransiQuery, PLAReasuransi>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetPLAReasuransiQuery> _logger;

        public GetPLAReasuransiQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetPLAReasuransiQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<PLAReasuransi> Handle(GetPLAReasuransiQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var plaReasuransi = _dbContextPst.PLAReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts, request.no_pla);

                return Task.FromResult(plaReasuransi);
            }, _logger);
        }
    }
}