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

        public string DatabaseName { get; set; }
    }

    public class GetPengajuanAkseptasiStatusQueryHandler : IRequestHandler<GetPengajuanAkseptasiStatusQuery, List<PengajuanAkseptasiStatusDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetPengajuanAkseptasiStatusQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PengajuanAkseptasiStatusDto>> Handle(GetPengajuanAkseptasiStatusQuery request,
            CancellationToken cancellationToken)
        {
            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            var result =
                (await _dbConnectionFactory.Query<PengajuanAkseptasiStatusDto>(@"Select *
	                                                            From v_TR_Akseptasi_history p 
	                                                            Where @kd_cb = p.kd_cb AND @kd_cob = p.kd_cob
			                                                            AND @kd_scob = p.kd_scob AND @kd_thn = p.kd_thn
			                                                            AND @no_aks = p.no_aks
			                                                            AND (CONVERT(varchar(15), p.tgl_status, 106) like '%'+@SearchKeyword+'%'
			                                                            OR p.nm_status like '%'+@SearchKeyword+'%'
			                                                            OR p.nm_user like '%'+@SearchKeyword+'%'
			                                                            OR @SearchKeyword = '' OR @SearchKeyword is null
		                                                            )",
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