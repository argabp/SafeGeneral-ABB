using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaTreatyMasuks.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.CetakNotaTreatyMasuks.Queries
{
    public class GetCetakNotaTreatyMasuksQuery : IRequest<GridResponse<CetakNotaTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetCetakNotaTreatyMasuksQueryHandler : IRequestHandler<GetCetakNotaTreatyMasuksQuery, GridResponse<CetakNotaTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetCetakNotaTreatyMasuksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<CetakNotaTreatyMasukDto>> Handle(
            GetCetakNotaTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = CetakNotaTreatyMasukConfig.Create();

            return await _gridEngine.QueryAsyncPST<CetakNotaTreatyMasukDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}