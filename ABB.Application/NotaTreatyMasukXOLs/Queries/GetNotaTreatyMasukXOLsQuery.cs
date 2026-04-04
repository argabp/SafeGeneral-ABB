using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaTreatyMasukXOLs.Configs;
using MediatR;

namespace ABB.Application.NotaTreatyMasukXOLs.Queries
{
    public class GetNotaTreatyMasukXOLsQuery : IRequest<GridResponse<NotaTreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetNotaTreatyMasukXOLsQueryHandler : IRequestHandler<GetNotaTreatyMasukXOLsQuery, GridResponse<NotaTreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetNotaTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<NotaTreatyMasukXOLDto>> Handle(
            GetNotaTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = NotaTreatyMasukXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<NotaTreatyMasukXOLDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}