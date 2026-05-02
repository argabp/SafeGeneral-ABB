using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiXOLKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiXOLKeluars.Queries
{
    public class GetCancelPostingNotaPremiXOLKeluarsQuery : IRequest<GridResponse<CancelPostingNotaPremiXOLKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaPremiXOLKeluarsQueryHandler : IRequestHandler<GetCancelPostingNotaPremiXOLKeluarsQuery, GridResponse<CancelPostingNotaPremiXOLKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaPremiXOLKeluarsQueryHandler> _logger;

        public GetCancelPostingNotaPremiXOLKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaPremiXOLKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaPremiXOLKeluarDto>> Handle(GetCancelPostingNotaPremiXOLKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaPremiXOLKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaPremiXOLKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}