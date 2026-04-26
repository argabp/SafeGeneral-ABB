using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetSORsQuery : IRequest<GridResponse<SORDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetSORsQueryHandler : IRequestHandler<GetSORsQuery, GridResponse<SORDto>>
    {
        private readonly IGridQueryEngine _gridEngine;

        public GetSORsQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<SORDto>> Handle(GetSORsQuery request, CancellationToken cancellationToken)
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