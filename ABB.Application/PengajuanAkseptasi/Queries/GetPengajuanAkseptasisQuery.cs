using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasisQuery : IRequest<List<PengajuanAkseptasiDto>>
    {
        public string SearchKeyword { get; set; }

        public string kd_cb { get; set; }
    }

    public class GetPengajuanAkseptasisQueryHandler : IRequestHandler<GetPengajuanAkseptasisQuery, List<PengajuanAkseptasiDto>>
    {
        private readonly IDbConnection _db;

        public GetPengajuanAkseptasisQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<PengajuanAkseptasiDto>> Handle(GetPengajuanAkseptasisQuery request,
            CancellationToken cancellationToken)
        {
            var results =
                await _db.QueryProc<PengajuanAkseptasiDto>("sp_PENGAJUANAKSEPTASI_GetPengajuanAkseptasi", new { request.SearchKeyword, request.kd_cb });

            var pengajuanAkseptasiDtos = results as PengajuanAkseptasiDto[] ?? results.ToArray();
            foreach (var result in pengajuanAkseptasiDtos)
            {
                result.Id =
                    $"{result.kd_cb.Trim()}{result.kd_cob.Trim()}{result.kd_scob.Trim()}{result.kd_thn}{result.no_aks}";
            }
            
            return pengajuanAkseptasiDtos.ToList();
        }
    }
}