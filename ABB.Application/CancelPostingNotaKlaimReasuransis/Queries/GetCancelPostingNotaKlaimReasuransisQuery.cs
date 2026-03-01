using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKlaimReasuransis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKlaimReasuransis.Queries
{
    public class GetCancelPostingNotaKlaimReasuransisQuery : IRequest<GridResponse<CancelPostingNotaKlaimReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCancelPostingNotaKlaimReasuransisQueryHandler : IRequestHandler<GetCancelPostingNotaKlaimReasuransisQuery, GridResponse<CancelPostingNotaKlaimReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        private readonly ILogger<GetCancelPostingNotaKlaimReasuransisQueryHandler> _logger;

        public GetCancelPostingNotaKlaimReasuransisQueryHandler(IGridQueryEngine gridEngine,
            ILogger<GetCancelPostingNotaKlaimReasuransisQueryHandler> logger)
        {
            _gridEngine = gridEngine;
            _logger = logger;
        }

        public async Task<GridResponse<CancelPostingNotaKlaimReasuransiDto>> Handle(GetCancelPostingNotaKlaimReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelPostingNotaKlaimReasuransiConfig.Create();
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                await _gridEngine.QueryAsyncPST<CancelPostingNotaKlaimReasuransiDto>(
                request.Grid,
                config,
                new
                {
                }), _logger);
        }
    }
}