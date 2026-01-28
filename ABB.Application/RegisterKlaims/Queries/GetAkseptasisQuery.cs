using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.RegisterKlaims.Configs;
using MediatR;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetAkseptasisQuery : IRequest<GridResponse<AkseptasiDto>>
    {
        public GridRequest Grid { get; set; }
        public string KodeCabang { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetAkseptasisQueryHandler : IRequestHandler<GetAkseptasisQuery, GridResponse<AkseptasiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetAkseptasisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<AkseptasiDto>> Handle(
            GetAkseptasisQuery request,
            CancellationToken cancellationToken)
        {
            var config = AkseptasiGridConfig.Create();

            return await _gridEngine.QueryAsync<AkseptasiDto>(
                request.Grid,
                config,
                new
                {
                    request.KodeCabang,
                    request.kd_cob,
                    request.kd_scob
                },
                request.DatabaseName
            );
        }
    }
}