using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.DLAReasuransis.Configs;
using MediatR;

namespace ABB.Application.DLAReasuransis.Queries
{
    public class GetDLAReasuransisQuery : IRequest<GridResponse<DLAReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetDLAReasuransisQueryHandler : IRequestHandler<GetDLAReasuransisQuery, GridResponse<DLAReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetDLAReasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<DLAReasuransiDto>> Handle(
            GetDLAReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = DLAReasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<DLAReasuransiDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}