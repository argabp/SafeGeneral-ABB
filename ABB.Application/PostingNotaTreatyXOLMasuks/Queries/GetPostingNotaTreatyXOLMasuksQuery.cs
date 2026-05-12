using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaTreatyMasuks.Configs;
using ABB.Application.PostingNotaTreatyXOLMasuks.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaTreatyXOLMasuks.Queries
{
    public class GetPostingNotaTreatyXOLMasuksQuery : IRequest<GridResponse<PostingNotaTreatyXOLMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaTreatyXOLMasuksQueryHandler : IRequestHandler<GetPostingNotaTreatyXOLMasuksQuery, GridResponse<PostingNotaTreatyXOLMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaTreatyXOLMasuksQueryHandler> _logger;

        public GetPostingNotaTreatyXOLMasuksQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaTreatyXOLMasuksQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaTreatyXOLMasukDto>> Handle(GetPostingNotaTreatyXOLMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaTreatyXOLMasukConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaTreatyXOLMasukDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}