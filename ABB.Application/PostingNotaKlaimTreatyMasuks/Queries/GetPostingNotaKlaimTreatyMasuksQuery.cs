using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaKlaimTreatyMasuks.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaKlaimTreatyMasuks.Queries
{
    public class GetPostingNotaKlaimTreatyMasuksQuery : IRequest<GridResponse<PostingNotaKlaimTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }
    public class GetPostingNotaKlaimTreatyMasuksQueryHandler : IRequestHandler<GetPostingNotaKlaimTreatyMasuksQuery, GridResponse<PostingNotaKlaimTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaKlaimTreatyMasuksQueryHandler> _logger;

        public GetPostingNotaKlaimTreatyMasuksQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaKlaimTreatyMasuksQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaKlaimTreatyMasukDto>> Handle(GetPostingNotaKlaimTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaKlaimTreatyMasukConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaKlaimTreatyMasukDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}