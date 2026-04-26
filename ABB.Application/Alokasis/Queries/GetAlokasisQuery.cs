using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetAlokasisQuery : IRequest<GridResponse<AlokasiDto>>
    {
        public GridRequest Grid { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetAlokasisQueryHandler : IRequestHandler<GetAlokasisQuery, GridResponse<AlokasiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;

        public GetAlokasisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<AlokasiDto>> Handle(GetAlokasisQuery request, CancellationToken cancellationToken)
        {
            var config = AlokasiConfig.Create();

            return await _gridEngine.QueryAsyncPST<AlokasiDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt
                }
            );
        }
    }
}