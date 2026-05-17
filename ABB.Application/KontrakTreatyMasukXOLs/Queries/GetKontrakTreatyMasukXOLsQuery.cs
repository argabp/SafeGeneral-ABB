using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyMasukXOLs.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyMasukXOLs.Queries
{
    public class GetKontrakTreatyMasukXOLsQuery : IRequest<GridResponse<KontrakTreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetKontrakTreatyMasukXOLsQueryHandler : IRequestHandler<GetKontrakTreatyMasukXOLsQuery, GridResponse<KontrakTreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKontrakTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KontrakTreatyMasukXOLDto>> Handle(
            GetKontrakTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = KontrakTreatyMasukXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<KontrakTreatyMasukXOLDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}