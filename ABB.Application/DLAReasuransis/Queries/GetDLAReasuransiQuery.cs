using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DLAReasuransis.Queries
{
    public class GetDLAReasuransiQuery : IRequest<DLAReasuransi>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_dla { get; set; }
    }

    public class GetDLAReasuransiQueryHandler : IRequestHandler<GetDLAReasuransiQuery, DLAReasuransi>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetDLAReasuransiQuery> _logger;

        public GetDLAReasuransiQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetDLAReasuransiQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<DLAReasuransi> Handle(GetDLAReasuransiQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
                {
                    var dLAReasuransi = _dbContextPst.DLAReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_kl, request.no_mts, request.no_dla);

                    return Task.FromResult(dLAReasuransi);
                }, _logger);
        }
    }
}