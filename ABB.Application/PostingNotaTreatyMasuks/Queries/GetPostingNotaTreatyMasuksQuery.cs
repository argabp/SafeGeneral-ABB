using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaTreatyMasuks.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaTreatyMasuks.Queries
{
    public class GetPostingNotaTreatyMasuksQuery : IRequest<GridResponse<PostingNotaTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaTreatyMasuksQueryHandler : IRequestHandler<GetPostingNotaTreatyMasuksQuery, GridResponse<PostingNotaTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaTreatyMasuksQueryHandler> _logger;

        public GetPostingNotaTreatyMasuksQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaTreatyMasuksQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaTreatyMasukDto>> Handle(GetPostingNotaTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaTreatyMasukConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaTreatyMasukDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}