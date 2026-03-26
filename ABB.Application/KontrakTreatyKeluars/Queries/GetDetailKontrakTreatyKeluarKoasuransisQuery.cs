using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KontrakTreatyKeluars.Configs;
using MediatR;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetDetailKontrakTreatyKeluarKoasuransisQuery : IRequest<GridResponse<DetailKontrakTreatyKeluarKoasuransiDto>>
    {
        public GridRequest Grid { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class GetDetailKontrakTreatyKeluarKoasuransisQueryHandler : IRequestHandler<GetDetailKontrakTreatyKeluarKoasuransisQuery, GridResponse<DetailKontrakTreatyKeluarKoasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailKontrakTreatyKeluarKoasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailKontrakTreatyKeluarKoasuransiDto>> Handle(
            GetDetailKontrakTreatyKeluarKoasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailKontrakTreatyKeluarKoasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailKontrakTreatyKeluarKoasuransiDto>(
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