using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakSlipKomisiFakultatifKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakSlipKomisiFakultatifKeluars.Queries
{
    public class GetCetakSlipKomisiFakultatifKeluarsQuery : IRequest<GridResponse<CetakSlipKomisiFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakSlipKomisiFakultatifKeluarsQueryHandler : IRequestHandler<GetCetakSlipKomisiFakultatifKeluarsQuery, GridResponse<CetakSlipKomisiFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakSlipKomisiFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakSlipKomisiFakultatifKeluarDto>> Handle(
            GetCetakSlipKomisiFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakSlipKomisiFakultatifKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakSlipKomisiFakultatifKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}