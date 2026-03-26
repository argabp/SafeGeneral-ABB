using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarCoveragesQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarCoverageDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarCoveragesQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarCoveragesQuery, GridResponse<DetailKontrakTreatyKeluarCoverageDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarCoveragesQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarCoverageDto>> Handle(
            GetDetailKontrakTreatyKeluarCoveragesQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarCoverageConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarCoverageDto>(
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