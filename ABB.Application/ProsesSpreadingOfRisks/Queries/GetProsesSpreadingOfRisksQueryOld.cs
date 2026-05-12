using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesSpreadingOfRisks.Configs;
using MediatR;

namespace ABB.Application.ProsesSpreadingOfRisks.Queries
{
    public class GetProsesSpreadingOfRisksQueryOld : IRequest<GridResponse<ProsesSpreadingOfRiskDto>>
    {
        public GridRequest Grid { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetProsesSpreadingOfRisksQueryOldHandler : IRequestHandler<GetProsesSpreadingOfRisksQueryOld, GridResponse<ProsesSpreadingOfRiskDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetProsesSpreadingOfRisksQueryOldHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ProsesSpreadingOfRiskDto>> Handle(
            GetProsesSpreadingOfRisksQueryOld request,
            CancellationToken cancellationToken)
        {
            var config = ProsesSpreadingOfRiskConfig.Create();

            return await _gridEngine.QueryAsyncPST<ProsesSpreadingOfRiskDto>(
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