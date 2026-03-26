using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarTableOfLimitsQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarTableOfLimitDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarTableOfLimitsQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarTableOfLimitsQuery, GridResponse<DetailKontrakTreatyKeluarTableOfLimitDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarTableOfLimitsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarTableOfLimitDto>> Handle(
            GetDetailKontrakTreatyKeluarTableOfLimitsQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarTableOfLimitConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarTableOfLimitDto>(
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