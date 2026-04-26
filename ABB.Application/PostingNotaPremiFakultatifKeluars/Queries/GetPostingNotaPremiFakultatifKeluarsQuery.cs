using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaPremiFakultatifKeluars.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaPremiFakultatifKeluars.Queries
{
    public class GetPostingNotaPremiFakultatifKeluarsQuery : IRequest<GridResponse<PostingNotaPremiFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaPremiFakultatifKeluarsQueryHandler : IRequestHandler<GetPostingNotaPremiFakultatifKeluarsQuery, GridResponse<PostingNotaPremiFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaPremiFakultatifKeluarsQueryHandler> _logger;

        public GetPostingNotaPremiFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaPremiFakultatifKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaPremiFakultatifKeluarDto>> Handle(GetPostingNotaPremiFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaPremiFakultatifKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaPremiFakultatifKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}