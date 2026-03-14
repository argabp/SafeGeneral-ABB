using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyMasuks.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyMasuks.Queries
{
    public class GetKontrakTreatyMasuksQuery : IRequest<GridResponse<KontrakTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetKontrakTreatyMasuksQueryHandler : IRequestHandler<GetKontrakTreatyMasuksQuery, GridResponse<KontrakTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKontrakTreatyMasuksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KontrakTreatyMasukDto>> Handle(
            GetKontrakTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = KontrakTreatyMasukConfig.Create();

            return await _gridEngine.QueryAsyncPST<KontrakTreatyMasukDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}