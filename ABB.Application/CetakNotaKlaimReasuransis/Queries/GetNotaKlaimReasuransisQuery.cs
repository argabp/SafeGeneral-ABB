using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKlaimReasuransis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CetakNotaKlaimReasuransis.Queries
{
    public class GetNotaKlaimReasuransisQuery : IRequest<GridResponse<NotaKlaimReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetNotaKlaimReasuransisQueryHandler : IRequestHandler<GetNotaKlaimReasuransisQuery, GridResponse<NotaKlaimReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetNotaKlaimReasuransisQueryHandler> _logger;

        public GetNotaKlaimReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetNotaKlaimReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<NotaKlaimReasuransiDto>> Handle(GetNotaKlaimReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaKlaimReasuransiConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<NotaKlaimReasuransiDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}