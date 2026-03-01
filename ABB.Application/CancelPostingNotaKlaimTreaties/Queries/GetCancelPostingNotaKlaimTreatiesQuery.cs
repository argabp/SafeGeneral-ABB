using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKlaimTreaties.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKlaimTreaties.Queries
{
    public class GetCancelPostingNotaKlaimTreatiesQuery : IRequest<GridResponse<CancelPostingNotaKlaimTreatyDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetCancelPostingNotaKlaimTreatiesQueryHandler : IRequestHandler<GetCancelPostingNotaKlaimTreatiesQuery, GridResponse<CancelPostingNotaKlaimTreatyDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaKlaimTreatiesQueryHandler> _logger;

        public GetCancelPostingNotaKlaimTreatiesQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaKlaimTreatiesQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaKlaimTreatyDto>> Handle(GetCancelPostingNotaKlaimTreatiesQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaKlaimTreatyConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaKlaimTreatyDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}