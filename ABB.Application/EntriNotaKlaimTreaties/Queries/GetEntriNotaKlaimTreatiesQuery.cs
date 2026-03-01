using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.EntriNotaKlaimTreaties.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaimTreaties.Queries
{
    public class GetEntriNotaKlaimTreatiesQuery : IRequest<GridResponse<EntriNotaKlaimTreatyDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetEntriNotaKlaimTreatiesQueryHandler : IRequestHandler<GetEntriNotaKlaimTreatiesQuery, GridResponse<EntriNotaKlaimTreatyDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetEntriNotaKlaimTreatiesQueryHandler> _logger;

        public GetEntriNotaKlaimTreatiesQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetEntriNotaKlaimTreatiesQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<EntriNotaKlaimTreatyDto>> Handle(GetEntriNotaKlaimTreatiesQuery request,
            CancellationToken cancellationToken)
        {
            var config = EntriNotaKlaimTreatyConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<EntriNotaKlaimTreatyDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}