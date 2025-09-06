using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class GetNotaKomisiTambahansQuery : IRequest<List<NotaKomisiTambahanDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetNotaKomisiTambahansQueryHandler : IRequestHandler<GetNotaKomisiTambahansQuery, List<NotaKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNotaKomisiTambahansQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<NotaKomisiTambahanDto>> Handle(GetNotaKomisiTambahansQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<NotaKomisiTambahanDto>(@"SELECT 
    			p.*, cb.nm_cb, cob.nm_cob , scob.nm_scob, pp.tgl_closing, pp.tgl_akh_ptg, pp.tgl_mul_ptg FROM fn05 p
    			    INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
    			    INNER JOIN uw01e pp
    			        ON pp.kd_cb = p.kd_cb
    			        AND pp.kd_cob = p.kd_cob
    			        AND pp.kd_scob = p.kd_scob
    			        AND pp.kd_thn = p.kd_thn
    			        AND pp.no_pol = p.no_pol
    			        AND pp.no_updt = p.no_updt
					WHERE (p.no_pol_ttg like '%'+@SearchKeyword+'%' 
						OR pp.tgl_mul_ptg like '%'+@SearchKeyword+'%' 
						OR pp.tgl_akh_ptg like '%'+@SearchKeyword+'%' 
						OR pp.tgl_closing like '%'+@SearchKeyword+'%' 
						OR p.no_updt like '%'+@SearchKeyword+'%' 
						OR cb.nm_cb like '%'+@SearchKeyword+'%' 
						OR cob.nm_cob like '%'+@SearchKeyword+'%' 
						OR scob.nm_scob like '%'+@SearchKeyword+'%' 
						OR cb.kd_cb like '%'+@SearchKeyword+'%' 
						OR cob.kd_cob like '%'+@SearchKeyword+'%' 
						OR scob.kd_scob like '%'+@SearchKeyword+'%' 
						OR p.nm_ttj like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", 
                new { request.SearchKeyword })).ToList();
        }
    }
}