using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluarXOLs.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class GetKontrakTreatyKeluarXOLsQuery : IRequest<GridResponse<KontrakTreatyKeluarXOLDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetKontrakTreatyKeluarXOLsQueryHandler : IRequestHandler<GetKontrakTreatyKeluarXOLsQuery, GridResponse<KontrakTreatyKeluarXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKontrakTreatyKeluarXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KontrakTreatyKeluarXOLDto>> Handle(
            GetKontrakTreatyKeluarXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = KontrakTreatyKeluarXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<KontrakTreatyKeluarXOLDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}