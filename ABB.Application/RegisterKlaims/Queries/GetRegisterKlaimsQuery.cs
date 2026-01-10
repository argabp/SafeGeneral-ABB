using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetRegisterKlaimsQuery : IRequest<List<RegisterKlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetRegisterKlaimsQueryHandler : IRequestHandler<GetRegisterKlaimsQuery, List<RegisterKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetRegisterKlaimsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<RegisterKlaimDto>> Handle(GetRegisterKlaimsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var results = (await _connectionFactory.Query<RegisterKlaimDto>(@"SELECT p.*, p2.nm_ttg, cb.nm_cb, cob.nm_cob, scob.nm_scob, s.nm_status 
				FROM cl01 p
				    INNER JOIN uw01e p2
						ON p.kd_cb = p2.kd_cb
							AND p.kd_cob=p2.kd_cob
							AND p.kd_scob=p2.kd_scob
							AND p.kd_thn_pol =p2.kd_thn 
							AND p.no_pol=p2.no_pol
							AND p.no_updt=p2.no_updt
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
					INNER JOIN MS_StatusKlaim s
						ON s.kd_status = p.kd_status
				WHERE cb.kd_cb = @KodeCabang AND (cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p2.nm_ttg like '%'+@SearchKeyword+'%' 
					OR p.sebab_kerugian like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
            
            
            foreach (var result in results)
            {
	            result.Id =
		            $"{result.kd_cb.Trim()}{result.kd_cob.Trim()}{result.kd_scob.Trim()}{result.kd_thn}{result.no_kl}";
            }

            return results;
        }
    }
}