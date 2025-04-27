using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.CetakNotaKlaim.Queries
{
    public class GetKlaimQuery : IRequest<List<KlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetKlaimQueryHandler : IRequestHandler<GetKlaimQuery, List<KlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKlaimQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<KlaimDto>> Handle(GetKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<KlaimDto>(@"SELECT 
    		RTRIM(LTRIM(p.kd_cb)) + RTRIM(LTRIM(p.jns_tr)) + RTRIM(LTRIM(p.jns_nt_msk)) + RTRIM(LTRIM(p.kd_thn)) + RTRIM(LTRIM(p.kd_bln)) + RTRIM(LTRIM(p.no_nt_msk)) + RTRIM(LTRIM(p.jns_nt_kel)) + RTRIM(LTRIM(p.no_nt_kel))   Id,
    		p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM cl10 p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND (p.no_nt_msk like '%'+@SearchKeyword+'%' 
					OR p.kd_bln like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR p.jns_tr like '%'+@SearchKeyword+'%' 
					OR p.jns_nt_msk like '%'+@SearchKeyword+'%' 
					OR p.jns_nt_kel like '%'+@SearchKeyword+'%' 
					OR p.no_nt_kel like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}