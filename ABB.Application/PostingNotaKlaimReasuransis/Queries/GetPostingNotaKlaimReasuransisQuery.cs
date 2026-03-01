using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using ABB.Application.PostingNotaKlaimReasuransis.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaKlaimReasuransis.Queries
{
    public class GetPostingNotaKlaimReasuransisQuery : IRequest<GridResponse<PostingNotaKlaimReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetPostingNotaKlaimReasuransisQueryHandler : IRequestHandler<GetPostingNotaKlaimReasuransisQuery, GridResponse<PostingNotaKlaimReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetPostingNotaKlaimReasuransisQueryHandler> _logger;

        public GetPostingNotaKlaimReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetPostingNotaKlaimReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<PostingNotaKlaimReasuransiDto>> Handle(GetPostingNotaKlaimReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = PostingNotaKlaimReasuransiConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<PostingNotaKlaimReasuransiDto>(
                    request.Grid,
                    config,
                    new
                    {
                    }), _logger);
        }
    }
}