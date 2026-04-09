using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KlaimAlokasiReasuransis.Configs;
using MediatR;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{    
    public class GetMutasiKlaimsQuery : IRequest<GridResponse<MutasiKlaimDto>>
    {
        public GridRequest Grid { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetMutasiKlaimsQueryHandler : IRequestHandler<GetMutasiKlaimsQuery, GridResponse<MutasiKlaimDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetMutasiKlaimsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<MutasiKlaimDto>> Handle(
            GetMutasiKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            var config = MutasiKlaimConfig.Create();

            return await _gridEngine.QueryAsyncPST<MutasiKlaimDto>(
                request.Grid,
                config,
                new
                {
                    request.SearchKeyword
                }
            );
        }
    }
}