using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakDLAReasuransis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CetakDLAReasuransis.Queries
{
    public class GetDLAReasuransisQuery : IRequest<GridResponse<DLAReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetDLAReasuransisQueryHandler : IRequestHandler<GetDLAReasuransisQuery, GridResponse<DLAReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetDLAReasuransisQueryHandler> _logger;

        public GetDLAReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetDLAReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<DLAReasuransiDto>> Handle(GetDLAReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakDLAReasuransiConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<DLAReasuransiDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}