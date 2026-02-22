using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.KlaimAlokasiReasuransis.Configs;
using MediatR;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetKlaimAlokasiReasuransisQuery : IRequest<GridResponse<KlaimAlokasiReasuransiDto>>
    {
        public GridRequest Grid { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKlaimAlokasiReasuransisQueryHandler : IRequestHandler<GetKlaimAlokasiReasuransisQuery, GridResponse<KlaimAlokasiReasuransiDto>>
    {
        private readonly IGridQueryEngine _gridEngine;
        
        public GetKlaimAlokasiReasuransisQueryHandler(IGridQueryEngine gridEngine)
        {
            _gridEngine = gridEngine;
        }
        
        public async Task<GridResponse<KlaimAlokasiReasuransiDto>> Handle(
            GetKlaimAlokasiReasuransisQuery request,
            CancellationToken cancellationToken)
        {
            var config = KlaimAlokasiReasuransiConfig.Create();

            return await _gridEngine.QueryAsyncPST<KlaimAlokasiReasuransiDto>(
                request.Grid,
                config,
                new
                {
                    request.kd_cb,
                    request.kd_cob,
                    request.kd_scob,
                    request.kd_thn,
                    request.no_kl,
                    request.no_mts,
                    request.SearchKeyword
                }
            );
        }
    }
}