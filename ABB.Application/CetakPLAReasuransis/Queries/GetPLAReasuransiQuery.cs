using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakPLAReasuransis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CetakPLAReasuransis.Queries
{
    public class GetPLAReasuransisQuery : IRequest<GridResponse<PLAReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPLAReasuransisQueryHandler : IRequestHandler<GetPLAReasuransisQuery, GridResponse<PLAReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPLAReasuransisQueryHandler> _logger;

        public GetPLAReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPLAReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PLAReasuransiDto>> Handle(GetPLAReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakPLAReasuransiConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PLAReasuransiDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}