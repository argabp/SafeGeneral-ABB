using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.ProsesCSM.Configs;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.ProsesCSM.Queries
{
    public class GetViewSourceDataQuery : IRequest<GridResponse<ViewSourceData>>
    {
        public GridRequest Grid { get; set; }

        public string KodeMetode { get; set; }
    }

    public class GetViewSourceDataQueryHandler : IRequestHandler<GetViewSourceDataQuery, GridResponse<ViewSourceData>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly IGridQueryEngine _gridEngine;

        public GetViewSourceDataQueryHandler(IDbContextCSM dbContextCsm, IGridQueryEngine gridEngine)
        {
            _dbContextCsm = dbContextCsm;
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<ViewSourceData>> Handle(GetViewSourceDataQuery request,
            CancellationToken cancellationToken)
        {
            var config = ProsesCSMGridConfig.Create();

            return await _gridEngine.QueryAsyncCSM<ViewSourceData>(
                request.Grid,
                config,
                new
                {
                    request.KodeMetode
                }
            );
        }
    }
}