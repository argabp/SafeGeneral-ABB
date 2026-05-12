using System.Threading;
using System.Threading.Tasks;
using ABB.Application.ClosingSpreadingOfRisks.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.ClosingSpreadingOfRisks.Queries
{
    public class GetClosingSpreadingOfRisksQueryOld : IRequest<GridResponse<ClosingSpreadingOfRiskDto>>
    {
        public GridRequest Grid { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetClosingSpreadingOfRisksQueryOldHandler : IRequestHandler<GetClosingSpreadingOfRisksQueryOld, GridResponse<ClosingSpreadingOfRiskDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetClosingSpreadingOfRisksQueryOldHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ClosingSpreadingOfRiskDto>> Handle(
            GetClosingSpreadingOfRisksQueryOld request,
            CancellationToken cancellationToken)
        {
            var config = ClosingSpreadingOfRiskConfig.Create();

            return await _gridEngine.QueryAsyncPST<ClosingSpreadingOfRiskDto>(
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