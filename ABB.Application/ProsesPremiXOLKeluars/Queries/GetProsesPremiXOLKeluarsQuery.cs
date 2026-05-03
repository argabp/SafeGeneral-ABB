using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesPremiXOLKeluars.Configs;
using MediatR;

namespace ABB.Application.ProsesPremiXOLKeluars.Queries
{
    public class GetProsesPremiXOLKeluarsQuery : IRequest<GridResponse<ProsesPremiXOLKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetProsesPremiXOLKeluarsQueryHandler : IRequestHandler<GetProsesPremiXOLKeluarsQuery, GridResponse<ProsesPremiXOLKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetProsesPremiXOLKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ProsesPremiXOLKeluarDto>> Handle(
            GetProsesPremiXOLKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = ProsesPremiXOLKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<ProsesPremiXOLKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}