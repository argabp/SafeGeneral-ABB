using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GetMutasiKlaimsQuery : IRequest<List<MutasiKlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetMutasiKlaimsQueryHandler : IRequestHandler<GetMutasiKlaimsQuery, List<MutasiKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetMutasiKlaimsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<MutasiKlaimDto>> Handle(GetMutasiKlaimsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<MutasiKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM v_cl01 p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND (cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_ttg like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new
            {
                request.SearchKeyword, request.KodeCabang
            })).ToList();
        }
    }
}