using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ReopenSpreadingOfRisks.Configs;
using MediatR;

namespace ABB.Application.ReopenSpreadingOfRisks.Queries
{
    public class GetReopenSpreadingOfRisksQuery : IRequest<GridResponse<ReopenSpreadingOfRiskDto>>
    {
        public GridRequest Grid { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetReopenSpreadingOfRisksQueryHandler : IRequestHandler<GetReopenSpreadingOfRisksQuery, GridResponse<ReopenSpreadingOfRiskDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetReopenSpreadingOfRisksQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ReopenSpreadingOfRiskDto>> Handle(
            GetReopenSpreadingOfRisksQuery request,
            CancellationToken cancellationToken)
        {
            var config = ReopenSpreadingOfRiskConfig.Create();

            return await _gridEngine.QueryAsyncPST<ReopenSpreadingOfRiskDto>(
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