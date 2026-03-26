using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarSCOBsQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarSCOBDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarSCOBsQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarSCOBsQuery, GridResponse<DetailKontrakTreatyKeluarSCOBDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarSCOBsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarSCOBDto>> Handle(
            GetDetailKontrakTreatyKeluarSCOBsQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarSCOBConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarSCOBDto>(
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