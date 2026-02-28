using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.PLAReasuransis.Configs;
using MediatR;

namespace ABB.Application.PLAReasuransis.Queries
{
    public class GetPLAReasuransisQuery : IRequest<GridResponse<PLAReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetPLAReasuransisQueryHandler : IRequestHandler<GetPLAReasuransisQuery, GridResponse<PLAReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetPLAReasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<PLAReasuransiDto>> Handle(
            GetPLAReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = PLAReasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<PLAReasuransiDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}