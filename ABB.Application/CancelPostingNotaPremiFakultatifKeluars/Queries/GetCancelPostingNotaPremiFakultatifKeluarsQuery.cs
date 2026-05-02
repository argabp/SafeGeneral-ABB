using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Queries
{
    public class GetCancelPostingNotaPremiFakultatifKeluarsQuery : IRequest<GridResponse<CancelPostingNotaPremiFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaPremiFakultatifKeluarsQueryHandler : IRequestHandler<GetCancelPostingNotaPremiFakultatifKeluarsQuery, GridResponse<CancelPostingNotaPremiFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaPremiFakultatifKeluarsQueryHandler> _logger;

        public GetCancelPostingNotaPremiFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaPremiFakultatifKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaPremiFakultatifKeluarDto>> Handle(GetCancelPostingNotaPremiFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaPremiFakultatifKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaPremiFakultatifKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}