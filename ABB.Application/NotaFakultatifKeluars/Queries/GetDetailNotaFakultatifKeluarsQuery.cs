using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaFakultatifKeluars.Configs;
using MediatR;

namespace ABB.Application.NotaFakultatifKeluars.Queries
{
    public class GetDetailNotaFakultatifKeluarsQuery : IRequest<GridResponse<DetailNotaFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetDetailNotaFakultatifKeluarsQueryHandler : IRequestHandler<GetDetailNotaFakultatifKeluarsQuery, GridResponse<DetailNotaFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDetailNotaFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DetailNotaFakultatifKeluarDto>> Handle(
            GetDetailNotaFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = DetailNotaFakultatifKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailNotaFakultatifKeluarDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk,
                    request.jns_nt_kel, request.no_nt_kel
                }
            );
        }
    }
}