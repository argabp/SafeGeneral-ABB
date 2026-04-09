using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesAlokasiKlaimReasuransis.Configs;
using MediatR;

namespace ABB.Application.ProsesAlokasiKlaimReasuransis.Queries
{
    public class GetProsesAlokasiKlaimReasuransisQuery : IRequest<GridResponse<ProsesAlokasiKlaimReasuransiDto>>
    {
        public GridRequest Grid { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetProsesAlokasiKlaimReasuransisQueryHandler : IRequestHandler<GetProsesAlokasiKlaimReasuransisQuery, GridResponse<ProsesAlokasiKlaimReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetProsesAlokasiKlaimReasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<ProsesAlokasiKlaimReasuransiDto>> Handle(
            GetProsesAlokasiKlaimReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = ProsesAlokasiKlaimReasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<ProsesAlokasiKlaimReasuransiDto>(
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