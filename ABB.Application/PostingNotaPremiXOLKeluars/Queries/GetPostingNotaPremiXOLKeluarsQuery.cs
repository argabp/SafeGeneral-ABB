using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaPremiXOLKeluars.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaPremiXOLKeluars.Queries
{
    public class GetPostingNotaPremiXOLKeluarsQuery : IRequest<GridResponse<PostingNotaPremiXOLKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaPremiXOLKeluarsQueryHandler : IRequestHandler<GetPostingNotaPremiXOLKeluarsQuery, GridResponse<PostingNotaPremiXOLKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaPremiXOLKeluarsQueryHandler> _logger;

        public GetPostingNotaPremiXOLKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaPremiXOLKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaPremiXOLKeluarDto>> Handle(GetPostingNotaPremiXOLKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaPremiXOLKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaPremiXOLKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}