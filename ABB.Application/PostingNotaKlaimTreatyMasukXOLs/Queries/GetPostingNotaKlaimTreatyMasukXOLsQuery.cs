using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaKlaimTreatyMasukXOLs.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaKlaimTreatyMasukXOLs.Queries
{
    public class GetPostingNotaKlaimTreatyMasukXOLsQuery : IRequest<GridResponse<PostingNotaKlaimTreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaKlaimTreatyMasukXOLsQueryHandler : IRequestHandler<GetPostingNotaKlaimTreatyMasukXOLsQuery, GridResponse<PostingNotaKlaimTreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaKlaimTreatyMasukXOLsQueryHandler> _logger;

        public GetPostingNotaKlaimTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaKlaimTreatyMasukXOLsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaKlaimTreatyMasukXOLDto>> Handle(GetPostingNotaKlaimTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaKlaimTreatyMasukXOLConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaKlaimTreatyMasukXOLDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}