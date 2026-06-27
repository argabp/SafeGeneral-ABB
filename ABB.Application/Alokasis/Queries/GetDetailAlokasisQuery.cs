using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetDetailAlokasisQuery : IRequest<GridResponse<DetailAlokasiDto>>
    {
        public GridRequest Grid { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt_reas { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }
        
        public string kd_endt { get; set; }
    }

    public class GetDetailAlokasisQueryHandler : IRequestHandler<GetDetailAlokasisQuery, GridResponse<DetailAlokasiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;

        public GetDetailAlokasisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<DetailAlokasiDto>> Handle(GetDetailAlokasisQuery request, CancellationToken cancellationToken)
        {
            var config = DetailAlokasiConfig.Create();

            return await _gridEngine.QueryAsyncPST<DetailAlokasiDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt_reas, 
                    request.no_rsk, request.kd_endt, request.no_updt 
                }
            );
        }
    }
}