using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifMasuks.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakNotaPremiFakultatifMasuks.Queries
{
    public class GetCetakNotaPremiFakultatifMasuksQuery : IRequest<GridResponse<CetakNotaPremiFakultatifMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakNotaPremiFakultatifMasuksQueryHandler : IRequestHandler<GetCetakNotaPremiFakultatifMasuksQuery, GridResponse<CetakNotaPremiFakultatifMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakNotaPremiFakultatifMasuksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakNotaPremiFakultatifMasukDto>> Handle(
            GetCetakNotaPremiFakultatifMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaPremiFakultatifMasukConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakNotaPremiFakultatifMasukDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}