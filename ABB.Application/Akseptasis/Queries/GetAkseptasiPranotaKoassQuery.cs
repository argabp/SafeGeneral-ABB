using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiPranotaKoassQuery : IRequest<List<AkseptasiPranotaKoasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetAkseptasiPranotaKoassQueryHandler : IRequestHandler<GetAkseptasiPranotaKoassQuery, List<AkseptasiPranotaKoasDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetAkseptasiPranotaKoassQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<AkseptasiPranotaKoasDto>> Handle(GetAkseptasiPranotaKoassQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<AkseptasiPranotaKoasDto>(@"SELECT p.*, p.kd_grp_pas + p.kd_rk_pas Id
				FROM uw03a p
				WHERE p.kd_cb = @KodeCabang AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_aks = @no_aks AND 
				      p.no_updt = @no_updt AND
				      p.kd_mtu = @kd_mtu", 
	            new { request.SearchKeyword, request.KodeCabang, 
		            request.kd_cob, request.kd_scob, request.kd_thn,
		            request.no_aks, request.no_updt, request.kd_mtu
	            })).ToList();
    //         return (await _connectionFactory.Query<AkseptasiResikoDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				// FROM uw04a p
				// 	INNER JOIN rf01 cb
				// 		ON p.kd_cb = cb.kd_cb
				// 	INNER JOIN rf04 cob
				// 		ON p.kd_cob = cob.kd_cob
				// 	INNER JOIN rf05 scob
				// 		ON p.kd_scob = scob.kd_scob
				// WHERE cb.kd_cb = @KodeCabang AND (p.no_aks like '%'+@SearchKeyword+'%' 
				// 	OR p.no_pol_pas like '%'+@SearchKeyword+'%' 
				// 	OR p.st_pas like '%'+@SearchKeyword+'%' 
				// 	OR cb.nm_cb like '%'+@SearchKeyword+'%' 
				// 	OR cob.nm_cob like '%'+@SearchKeyword+'%' 
				// 	OR scob.nm_scob like '%'+@SearchKeyword+'%' 
				// 	OR p.nm_ttg like '%'+@SearchKeyword+'%' 
				// 	OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}