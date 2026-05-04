using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.NotaPremiXOLKeluars.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaPremiXOLKeluars.Queries
{
    public class GetNotaPremiXOLKeluarsQuery : IRequest<GridResponse<NotaPremiXOLKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    
    public class GetNotaPremiXOLKeluarsQueryHandler : IRequestHandler<GetNotaPremiXOLKeluarsQuery, GridResponse<NotaPremiXOLKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetNotaPremiXOLKeluarsQueryHandler> _logger;

        public GetNotaPremiXOLKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetNotaPremiXOLKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<NotaPremiXOLKeluarDto>> Handle(GetNotaPremiXOLKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = NotaPremiXOLKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<NotaPremiXOLKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}