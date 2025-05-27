using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasiStatusQuery : IRequest<List<PengajuanAkseptasiStatusDto>>
    {
        public string SearchKeyword { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
    }

    public class GetPengajuanAkseptasiStatusQueryHandler : IRequestHandler<GetPengajuanAkseptasiStatusQuery, List<PengajuanAkseptasiStatusDto>>
    {
        private readonly IDbConnection _db;

        public GetPengajuanAkseptasiStatusQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<List<PengajuanAkseptasiStatusDto>> Handle(GetPengajuanAkseptasiStatusQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                (await _db.QueryProc<PengajuanAkseptasiStatusDto>("sp_PENGAJUANAKSEPTASI_GetPengajuanAkseptasiStatus",
                    new
                    {
                        request.SearchKeyword, request.kd_cb, request.kd_cob,
                        request.kd_scob, request.kd_thn, request.no_aks,
                    })).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id =
                    $@"{result[i].kd_cb.Trim()}{result[i].kd_cob.Trim()}{result[i].kd_scob.Trim()}{result[i].kd_thn}{result[i].no_aks}{result[i].no_urut}";
            }
            
            return result;
        }
    }
}