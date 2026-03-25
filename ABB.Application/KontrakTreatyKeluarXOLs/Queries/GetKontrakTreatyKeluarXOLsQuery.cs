using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluarXOLs.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class GetKontrakTreatyKeluarXOLsQuery : IRequest<GridResponse<KontrakTreatyKeluarXOLDto>>
    {
        public GridRequest Grid { get; set; }
        
        public string kd_cb { get; set; }
        
        public string kd_jns_sor { get; set; }
        
        public string kd_tty_npps { get; set; }
    }

    public class GetKontrakTreatyKeluarXOLsQueryHandler : IRequestHandler<GetKontrakTreatyKeluarXOLsQuery, GridResponse<KontrakTreatyKeluarXOLDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKontrakTreatyKeluarXOLsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KontrakTreatyKeluarXOLDto>> Handle(
            GetKontrakTreatyKeluarXOLsQuery request,
            CancellationToken cancellationToken)
        {
            var config = KontrakTreatyKeluarXOLConfig.Create();

            return await _gridEngine.QueryAsyncPST<KontrakTreatyKeluarXOLDto>(
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