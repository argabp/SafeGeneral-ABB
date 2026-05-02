using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiTreatyKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiTreatyKeluars.Queries
{
    public class GetCancelPostingNotaPremiTreatyKeluarsQuery : IRequest<GridResponse<CancelPostingNotaPremiTreatyKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaPremiTreatyKeluarsQueryHandler : IRequestHandler<GetCancelPostingNotaPremiTreatyKeluarsQuery, GridResponse<CancelPostingNotaPremiTreatyKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaPremiTreatyKeluarsQueryHandler> _logger;

        public GetCancelPostingNotaPremiTreatyKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaPremiTreatyKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaPremiTreatyKeluarDto>> Handle(GetCancelPostingNotaPremiTreatyKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaPremiTreatyKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaPremiTreatyKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}