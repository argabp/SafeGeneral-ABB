using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.DLAKoasuransi.Queries
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
    		RTRIM(LTRIM(p.kd_cb)) + RTRIM(LTRIM(p.kd_cob)) + RTRIM(LTRIM(p.kd_scob)) + RTRIM(LTRIM(p.kd_thn)) + RTRIM(LTRIM(p.no_kl)) + CONVERT(varchar(max), p.no_mts) + RTRIM(LTRIM(p.st_tipe_dla)) + CONVERT(varchar(max), p.no_dla) Id,
    		p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM v_cl09 p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND (p.no_mts like '%'+@SearchKeyword+'%' 
					OR p.no_kl like '%'+@SearchKeyword+'%' 
					OR p.st_tipe_dla like '%'+@SearchKeyword+'%' 
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