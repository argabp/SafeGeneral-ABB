using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetKontrakTreatyKeluarsQuery : IRequest<GridResponse<KontrakTreatyKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetKontrakTreatyKeluarsQueryHandler : IRequestHandler<GetKontrakTreatyKeluarsQuery, GridResponse<KontrakTreatyKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKontrakTreatyKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KontrakTreatyKeluarDto>> Handle(
            GetKontrakTreatyKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = KontrakTreatyKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<KontrakTreatyKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}