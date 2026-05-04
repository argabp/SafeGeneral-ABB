using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.NotaPremiTreatyKeluars.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaPremiTreatyKeluars.Queries
{
    public class GetNotaPremiTreatyKeluarsQuery : IRequest<GridResponse<NotaPremiTreatyKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    
    public class GetNotaPremiTreatyKeluarsQueryHandler : IRequestHandler<GetNotaPremiTreatyKeluarsQuery, GridResponse<NotaPremiTreatyKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetNotaPremiTreatyKeluarsQueryHandler> _logger;

        public GetNotaPremiTreatyKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetNotaPremiTreatyKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<NotaPremiTreatyKeluarDto>> Handle(GetNotaPremiTreatyKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = NotaPremiTreatyKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<NotaPremiTreatyKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}