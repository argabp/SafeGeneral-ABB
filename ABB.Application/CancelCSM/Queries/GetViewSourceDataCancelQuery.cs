using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelCSM.Configs;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.CancelCSM.Queries
{
    public class GetViewSourceDataCancelQuery : IRequest<GridResponse<ViewSourceDataCancel>>
    {
        public GridRequest Grid { get; set; }
        public string KodeMetode { get; set; }
    }

    public class GetViewSourceDataCancelQueryHandler : IRequestHandler<GetViewSourceDataCancelQuery, GridResponse<ViewSourceDataCancel>>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly IGridQueryEngine _gridEngine;

        public GetViewSourceDataCancelQueryHandler(IDbContextCSM dbContextCsm, IGridQueryEngine gridEngine)
        {
            _dbContextCsm = dbContextCsm;
            _gridEngine = gridEngine;
        }

        public async Task<GridResponse<ViewSourceDataCancel>> Handle(GetViewSourceDataCancelQuery request,
            CancellationToken cancellationToken)
        {
            var config = CancelCSMGridConfig.Create();

            return await _gridEngine.QueryAsyncCSM<ViewSourceDataCancel>(
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