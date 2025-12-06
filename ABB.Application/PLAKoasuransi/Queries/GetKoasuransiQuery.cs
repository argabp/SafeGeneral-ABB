using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PLAKoasuransi.Queries
{
    public class GetKoasuransiQuery : IRequest<List<KoasuransiDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetKoasuransiQueryHandler : IRequestHandler<GetKoasuransiQuery, List<KoasuransiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKoasuransiQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<KoasuransiDto>> Handle(GetKoasuransiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<KoasuransiDto>(@"SELECT 
    		RTRIM(LTRIM(p.kd_cb)) + RTRIM(LTRIM(p.kd_cob)) + RTRIM(LTRIM(p.kd_scob)) + RTRIM(LTRIM(p.kd_thn)) + RTRIM(LTRIM(p.no_kl)) + CONVERT(varchar(max), p.no_mts) + RTRIM(LTRIM(p.st_tipe_pla)) + CONVERT(varchar(max), p.no_pla) Id,
    		p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob, r.nm_rk nm_ttj, p2.nm_ttg, p3.no_pol_lama
				FROM cl08 p 
				    INNER JOIN cl01 p3
						ON p.kd_cb = p3.kd_cb
							AND p.kd_cob=p3.kd_cob
							AND p.kd_scob=p3.kd_scob
							AND p.kd_thn =p3.kd_thn 
							AND p.no_kl=p3.no_kl
				    INNER JOIN uw01e p2
						ON p3.kd_cb = p2.kd_cb
							AND p3.kd_cob=p2.kd_cob
							AND p3.kd_scob=p2.kd_scob
							AND p3.kd_thn_pol =p2.kd_thn 
							AND p3.no_pol=p2.no_pol
							AND p3.no_updt=p2.no_updt
				    INNER JOIN rf03 r
						ON p.kd_cb = r.kd_cb
							AND p.kd_grp_pas = r.kd_grp_rk
							AND p.kd_rk_pas = r.kd_rk
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND p.st_tipe_pla = 'K' AND (p.no_mts like '%'+@SearchKeyword+'%' 
					OR p.no_kl like '%'+@SearchKeyword+'%' 
					OR p.st_tipe_pla like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR cob.kd_cob like '%'+@SearchKeyword+'%' 
					OR scob.kd_scob like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}