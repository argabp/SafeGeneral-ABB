using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKomisiTambahans.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CetakNotaKomisiTambahans.Queries
{
    public class GetCetakNotaKomisiTambahansQuery : IRequest<List<CetakNotaKomisiTambahanDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetCetakNotaKomisiTambahansQueryHandler : IRequestHandler<GetCetakNotaKomisiTambahansQuery, List<CetakNotaKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCetakNotaKomisiTambahansQueryHandler> _logger;

        public GetCetakNotaKomisiTambahansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCetakNotaKomisiTambahansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CetakNotaKomisiTambahanDto>> Handle(GetCetakNotaKomisiTambahansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<CetakNotaKomisiTambahanDto>(@"SELECT 
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
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();

                var sequence = 1;
                foreach (var result in results)
                {
                    result.Id = sequence;
                    sequence++;
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}