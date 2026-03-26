using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarsQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarsQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarsQuery, GridResponse<DetailKontrakTreatyKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarDto>> Handle(
            GetDetailKontrakTreatyKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarDto>(
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