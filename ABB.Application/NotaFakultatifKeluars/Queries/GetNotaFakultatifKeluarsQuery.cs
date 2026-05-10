using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.NotaFakultatifKeluars.Configs;
using MediatR;

namespace ABB.Application.NotaFakultatifKeluars.Queries
{
    public class GetNotaFakultatifKeluarsQuery : IRequest<GridResponse<NotaFakultatifKeluarDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetNotaFakultatifKeluarsQueryHandler : IRequestHandler<GetNotaFakultatifKeluarsQuery, GridResponse<NotaFakultatifKeluarDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetNotaFakultatifKeluarsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<NotaFakultatifKeluarDto>> Handle(
            GetNotaFakultatifKeluarsQuery request,
            CancellationToken cancellationToken)
        {
            var config = NotaFakultatifKeluarConfig.Create();

            return await _gridEngine.QueryAsyncPST<NotaFakultatifKeluarDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}