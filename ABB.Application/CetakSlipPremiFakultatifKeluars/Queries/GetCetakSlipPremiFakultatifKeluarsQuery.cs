using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakSlipPremiFakultatifKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakSlipPremiFakultatifKeluars.Queries
{
    public class GetCetakSlipPremiFakultatifKeluarsQuery : IRequest<GridResponse<CetakSlipPremiFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakSlipPremiFakultatifKeluarsQueryHandler : IRequestHandler<GetCetakSlipPremiFakultatifKeluarsQuery, GridResponse<CetakSlipPremiFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakSlipPremiFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakSlipPremiFakultatifKeluarDto>> Handle(
            GetCetakSlipPremiFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakSlipPremiFakultatifKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakSlipPremiFakultatifKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}