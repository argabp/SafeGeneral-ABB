using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaTreatyMasukXOLs.Configs;
using MediatR;

namespace ABB.Application.NotaTreatyMasukXOLs.Queries
{
    public class GetTreatyMasukXOLsQuery : IRequest<GridResponse<TreatyMasukXOLDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }
    }

    public class GetTreatyMasukXOLsQueryHandler : IRequestHandler<GetTreatyMasukXOLsQuery, GridResponse<TreatyMasukXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetTreatyMasukXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<TreatyMasukXOLDto>> Handle(
            GetTreatyMasukXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = TreatyMasukXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<TreatyMasukXOLDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_jns_sor
                }
            );
        }
    }
}