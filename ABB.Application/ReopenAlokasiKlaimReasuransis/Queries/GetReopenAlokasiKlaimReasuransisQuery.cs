using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ReopenAlokasiKlaimReasuransis.Configs;
using MediatR;

namespace ABB.Application.ReopenAlokasiKlaimReasuransis.Queries
{    
    public class GetReopenAlokasiKlaimReasuransisQuery : IRequest<GridResponse<ReopenAlokasiKlaimReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
    }

    public class GetReopenAlokasiKlaimReasuransisQueryHandler : IRequestHandler<GetReopenAlokasiKlaimReasuransisQuery, GridResponse<ReopenAlokasiKlaimReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetReopenAlokasiKlaimReasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ReopenAlokasiKlaimReasuransiDto>> Handle(
            GetReopenAlokasiKlaimReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = ReopenAlokasiKlaimReasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<ReopenAlokasiKlaimReasuransiDto>(
                request.Grid,
                config,
                new
                {
                }
            );
        }
    }
}