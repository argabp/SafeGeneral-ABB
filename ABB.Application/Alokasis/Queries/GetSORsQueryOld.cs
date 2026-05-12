using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetSORsQueryOld : IRequest<GridResponse<SORDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetSORsQueryOldHandler : IRequestHandler<GetSORsQueryOld, GridResponse<SORDto>>
    {
        private readonly IGridQueryEngine _gridEngine;

        public GetSORsQueryOldHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<SORDto>> Handle(GetSORsQueryOld request, CancellationToken cancellationToken)
        {
            var config = SORConfig.Create();

            return await _gridEngine.QueryAsyncPST<SORDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}