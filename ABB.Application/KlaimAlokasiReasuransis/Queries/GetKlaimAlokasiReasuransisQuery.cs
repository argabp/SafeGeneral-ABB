using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.KlaimAlokasiReasuransis.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetKlaimAlokasiReasuransisQuery : IRequest<GridResponse<KlaimAlokasiReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKlaimAlokasiReasuransisQueryHandler : IRequestHandler<GetKlaimAlokasiReasuransisQuery, GridResponse<KlaimAlokasiReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetKlaimAlokasiReasuransisQueryHandler> _logger;

        public GetKlaimAlokasiReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetKlaimAlokasiReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }
        
        public async Task<GridResponse<KlaimAlokasiReasuransiDto>> Handle(
            GetKlaimAlokasiReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var config = KlaimAlokasiReasuransiConfig.Create();

                var response = await _gridEngine.QueryAsyncPST<KlaimAlokasiReasuransiDto>(
                    request.Grid,
                    config,
                    new
                    {
                        request.kd_cb,
                        request.kd_cob,
                        request.kd_scob,
                        request.kd_thn,
                        request.no_kl,
                        request.no_mts,
                        request.SearchKeyword
                    }
                );

                return response;
            }, _logger);
        }
    }
}