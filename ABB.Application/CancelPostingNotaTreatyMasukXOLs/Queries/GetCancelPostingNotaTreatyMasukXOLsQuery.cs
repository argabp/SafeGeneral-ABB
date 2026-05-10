using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaTreatyMasukXOLs.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaTreatyMasukXOLs.Queries
{
    public class GetCancelPostingNotaTreatyMasukXOLsQuery : IRequest<GridResponse<CancelPostingNotaTreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaTreatyMasukXOLsQueryHandler : IRequestHandler<GetCancelPostingNotaTreatyMasukXOLsQuery, GridResponse<CancelPostingNotaTreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaTreatyMasukXOLsQueryHandler> _logger;

        public GetCancelPostingNotaTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaTreatyMasukXOLsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaTreatyMasukXOLDto>> Handle(GetCancelPostingNotaTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaTreatyMasukXOLConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaTreatyMasukXOLDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}