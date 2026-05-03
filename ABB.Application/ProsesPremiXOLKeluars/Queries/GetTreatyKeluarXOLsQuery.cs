using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesPremiXOLKeluars.Configs;
using MediatR;

namespace ABB.Application.ProsesPremiXOLKeluars.Queries
{
    public class GetTreatyKeluarXOLsQuery : IRequest<GridResponse<TreatyKeluarXOLDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }
    }

    public class GetTreatyKeluarXOLsQueryHandler : IRequestHandler<GetTreatyKeluarXOLsQuery, GridResponse<TreatyKeluarXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetTreatyKeluarXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<TreatyKeluarXOLDto>> Handle(
            GetTreatyKeluarXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = TreatyKeluarXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<TreatyKeluarXOLDto>(
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