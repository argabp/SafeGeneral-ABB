using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarExcludesQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarExcludeDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarExcludesQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarExcludesQuery, GridResponse<DetailKontrakTreatyKeluarExcludeDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarExcludesQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarExcludeDto>> Handle(
            GetDetailKontrakTreatyKeluarExcludesQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarExcludeConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarExcludeDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_jns_sor, request.kd_tty_pps
                }
            );
        }
    }
}