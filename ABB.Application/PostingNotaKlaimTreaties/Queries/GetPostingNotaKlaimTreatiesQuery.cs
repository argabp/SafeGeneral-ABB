using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaKlaimTreaties.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaKlaimTreaties.Queries
{
    public class GetPostingNotaKlaimTreatiesQuery : IRequest<GridResponse<PostingNotaKlaimTreatyDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaKlaimTreatiesQueryHandler : IRequestHandler<GetPostingNotaKlaimTreatiesQuery, GridResponse<PostingNotaKlaimTreatyDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaKlaimTreatiesQueryHandler> _logger;

        public GetPostingNotaKlaimTreatiesQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaKlaimTreatiesQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaKlaimTreatyDto>> Handle(GetPostingNotaKlaimTreatiesQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaKlaimTreatyConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaKlaimTreatyDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}