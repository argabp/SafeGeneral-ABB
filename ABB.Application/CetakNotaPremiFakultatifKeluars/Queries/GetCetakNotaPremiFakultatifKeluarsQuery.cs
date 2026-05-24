using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifKeluars.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakNotaPremiFakultatifKeluars.Queries
{
    public class GetCetakNotaPremiFakultatifKeluarsQuery : IRequest<GridResponse<CetakNotaPremiFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakNotaPremiFakultatifKeluarsQueryHandler : IRequestHandler<GetCetakNotaPremiFakultatifKeluarsQuery, GridResponse<CetakNotaPremiFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakNotaPremiFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakNotaPremiFakultatifKeluarDto>> Handle(
            GetCetakNotaPremiFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaPremiFakultatifKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakNotaPremiFakultatifKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}