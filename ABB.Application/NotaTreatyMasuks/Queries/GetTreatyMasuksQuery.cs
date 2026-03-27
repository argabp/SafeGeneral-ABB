using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaTreatyMasuks.Configs;
using MediatR;

namespace ABB.Application.NotaTreatyMasuks.Queries
{
    public class GetTreatyMasuksQuery : IRequest<GridResponse<TreatyMasukDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }
    }

    public class GetTreatyMasuksQueryHandler : IRequestHandler<GetTreatyMasuksQuery, GridResponse<TreatyMasukDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetTreatyMasuksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<TreatyMasukDto>> Handle(
            GetTreatyMasuksQuery request,
            CancellationToken cancellationToken)
        {
            var config = TreatyMasukConfig.Create();

            return await _gridEngine.QueryAsyncPST<TreatyMasukDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_jns_sor
                }
            );
        }
    }
}