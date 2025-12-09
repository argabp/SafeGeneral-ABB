using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ApprovalMutasiKlaims.Queries
{
    public class GetPengajuanKlaimStatusQuery : IRequest<List<PengajuanKlaimStatusDto>>
    {
        public string SearchKeyword { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string DatabaseName { get; set; }
    }

    public class GetPengajuanKlaimStatusQueryHandler : IRequestHandler<GetPengajuanKlaimStatusQuery, List<PengajuanKlaimStatusDto>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetPengajuanKlaimStatusQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PengajuanKlaimStatusDto>> Handle(GetPengajuanKlaimStatusQuery request,
            CancellationToken cancellationToken)
        {
            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            var result =
                (await _dbConnectionFactory.Query<PengajuanKlaimStatusDto>(@"Select *
	                                                            From v_TR_Klaim_history p 
	                                                            Where @kd_cb = p.kd_cb AND @kd_cob = p.kd_cob
			                                                            AND @kd_scob = p.kd_scob AND @kd_thn = p.kd_thn
			                                                            AND @no_kl = p.no_kl AND @no_mts = p.no_mts  
			                                                            AND (CONVERT(varchar(15), p.tgl_status, 106) like '%'+@SearchKeyword+'%'
			                                                            OR p.nm_status like '%'+@SearchKeyword+'%'
			                                                            OR p.nm_user like '%'+@SearchKeyword+'%'
			                                                            OR @SearchKeyword = '' OR @SearchKeyword is null
		                                                            )",
                    new
                    {
                        request.SearchKeyword, request.kd_cb, request.kd_cob,
                        request.kd_scob, request.kd_thn, request.no_kl, request.no_mts, request.DatabaseName
                    })).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id =
                    $@"{result[i].kd_cb.Trim()}{result[i].kd_cob.Trim()}{result[i].kd_scob.Trim()}{result[i].kd_thn}{result[i].no_kl}{result[i].no_mts}{result[i].no_urut}";
            }
            
            return result;
        }
    }
}