using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaTreatyMasuks.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaTreatyMasuks.Queries
{
    public class GetCancelPostingNotaTreatyMasuksQuery : IRequest<GridResponse<CancelPostingNotaTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaTreatyMasuksQueryHandler : IRequestHandler<GetCancelPostingNotaTreatyMasuksQuery, GridResponse<CancelPostingNotaTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaTreatyMasuksQueryHandler> _logger;

        public GetCancelPostingNotaTreatyMasuksQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaTreatyMasuksQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaTreatyMasukDto>> Handle(GetCancelPostingNotaTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaTreatyMasukConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaTreatyMasukDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}
