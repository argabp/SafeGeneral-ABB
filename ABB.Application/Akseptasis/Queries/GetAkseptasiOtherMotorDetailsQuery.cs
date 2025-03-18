using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiOtherMotorDetailsQuery : IRequest<List<AkseptasiOtherMotorDetailDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }
    }

    public class GetAkseptasiOtherMotorDetailsQueryHandler : IRequestHandler<GetAkseptasiOtherMotorDetailsQuery, List<AkseptasiOtherMotorDetailDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetAkseptasiOtherMotorDetailsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<AkseptasiOtherMotorDetailDto>> Handle(GetAkseptasiOtherMotorDetailsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<AkseptasiOtherMotorDetailDto>(@"SELECT p.*
				FROM uw04a02d p
				WHERE p.kd_cb = @kd_cb AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_aks = @no_aks AND 
				      p.no_updt = @no_updt AND
				      p.no_rsk = @no_rsk AND
				      p.kd_endt = @kd_endt", 
	            new { request.SearchKeyword, request.kd_cb, 
		            request.kd_cob, request.kd_scob, request.kd_thn,
		            request.no_aks, request.no_updt, request.no_rsk,
		            request.kd_endt
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