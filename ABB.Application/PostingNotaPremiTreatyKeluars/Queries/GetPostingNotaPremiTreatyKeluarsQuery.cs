using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaPremiTreatyKeluars.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaPremiTreatyKeluars.Queries
{
    public class GetPostingNotaPremiTreatyKeluarsQuery : IRequest<GridResponse<PostingNotaPremiTreatyKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaPremiTreatyKeluarsQueryHandler : IRequestHandler<GetPostingNotaPremiTreatyKeluarsQuery, GridResponse<PostingNotaPremiTreatyKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaPremiTreatyKeluarsQueryHandler> _logger;

        public GetPostingNotaPremiTreatyKeluarsQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaPremiTreatyKeluarsQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaPremiTreatyKeluarDto>> Handle(GetPostingNotaPremiTreatyKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaPremiTreatyKeluarConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaPremiTreatyKeluarDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}