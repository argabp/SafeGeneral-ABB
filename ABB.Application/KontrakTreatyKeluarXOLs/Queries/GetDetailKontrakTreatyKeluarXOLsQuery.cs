using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluarXOLs.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class GetDetailKontrakTreatyKeluarXOLsQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarXOLDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarXOLsQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarXOLsQuery, GridResponse<DetailKontrakTreatyKeluarXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarXOLDto>> Handle(
            GetDetailKontrakTreatyKeluarXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarXOLDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_jns_sor, request.kd_tty_npps
                }
            );
        }
    }
}