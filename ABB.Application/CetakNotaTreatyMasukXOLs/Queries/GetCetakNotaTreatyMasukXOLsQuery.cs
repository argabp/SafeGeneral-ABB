using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaTreatyMasukXOLs.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakNotaTreatyMasukXOLs.Queries
{
    public class GetCetakNotaTreatyMasukXOLsQuery : IRequest<GridResponse<CetakNotaTreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakNotaTreatyMasukXOLsQueryHandler : IRequestHandler<GetCetakNotaTreatyMasukXOLsQuery, GridResponse<CetakNotaTreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakNotaTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakNotaTreatyMasukXOLDto>> Handle(
            GetCetakNotaTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaTreatyMasukXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakNotaTreatyMasukXOLDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}