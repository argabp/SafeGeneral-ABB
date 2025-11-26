using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace ABB.Application.ReopenMutasiKlaims.Queries
{
    public class GetReopenMutasiKlaimQuery : IRequest<List<ReopenMutasiKlaimDto>>
    {
	    public string SearchKeyword { get; set; }
	    public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetReopenMutasiKlaimQueryHandler : IRequestHandler<GetReopenMutasiKlaimQuery, List<ReopenMutasiKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetReopenMutasiKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<List<ReopenMutasiKlaimDto>> Handle(GetReopenMutasiKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<ReopenMutasiKlaimDto>(@"SELECT 
    		p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob 
				FROM v_cl03 p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND (p.no_mts like '%'+@SearchKeyword+'%' 
						OR p.tgl_mts like '%'+@SearchKeyword+'%' 
						OR cb.nm_cb like '%'+@SearchKeyword+'%' 
						OR cob.nm_cob like '%'+@SearchKeyword+'%' 
						OR scob.nm_scob like '%'+@SearchKeyword+'%' 
						OR cb.kd_cb like '%'+@SearchKeyword+'%' 
						OR cob.kd_cob like '%'+@SearchKeyword+'%' 
						OR scob.kd_scob like '%'+@SearchKeyword+'%' 
						OR p.nm_ttg like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}