using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiTreatyXOLKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CetakNotaPremiTreatyXOLKeluars.Queries
{
    public class GetCetakNotaPremiTreatyXOLKeluarsQuery : IRequest<GridResponse<CetakNotaPremiTreatyXOLKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    
    public class CetakNotaPremiTreatyXOLKeluarsQueryHandler : IRequestHandler<GetCetakNotaPremiTreatyXOLKeluarsQuery, GridResponse<CetakNotaPremiTreatyXOLKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<CetakNotaPremiTreatyXOLKeluarsQueryHandler> _logger;

        public CetakNotaPremiTreatyXOLKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<CetakNotaPremiTreatyXOLKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CetakNotaPremiTreatyXOLKeluarDto>> Handle(GetCetakNotaPremiTreatyXOLKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaPremiTreatyXOLKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CetakNotaPremiTreatyXOLKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}