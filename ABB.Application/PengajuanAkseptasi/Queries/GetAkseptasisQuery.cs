using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.PengajuanAkseptasi.Configs;
using ABB.Application.RegisterKlaims.Configs;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetAkseptasisQuery : IRequest<GridResponse<AkseptasiDto>>
    {
        public GridRequest Grid { get; set; }
        public string DatabaseName { get; set; }

        public string jns_pengajuan { get; set; }
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
            GridConfig? config = null;
            switch (request.jns_pengajuan)
            {
                case "2":
                case "3":
                    config = AkseptasiGridEndorseConfig.Create();
                    break;
                case "4":
                    config = AkseptasiGridRenewalConfig.Create();
                    break;
            }

            if (config == null)
            {
                config = new GridConfig();
            }
            
            return await _gridEngine.QueryAsync<AkseptasiDto>(
                request.Grid,
                config,
                new
                {
                },
                request.DatabaseName
            );
        }
    }
}