using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaTreatyMasuks.Configs;
using MediatR;

namespace ABB.Application.NotaTreatyMasuks.Queries
{
    public class GetNotaTreatyMasuksQuery : IRequest<GridResponse<NotaTreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetNotaTreatyMasuksQueryHandler : IRequestHandler<GetNotaTreatyMasuksQuery, GridResponse<NotaTreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetNotaTreatyMasuksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<NotaTreatyMasukDto>> Handle(
            GetNotaTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = NotaTreatyMasukConfig.Create();

            return await _gridEngine.QueryAsyncPST<NotaTreatyMasukDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}