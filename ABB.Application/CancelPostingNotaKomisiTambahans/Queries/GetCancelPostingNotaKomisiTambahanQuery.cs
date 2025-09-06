using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKomisiTambahans.Queries
{
    public class GetCancelPostingNotaKomisiTambahanQuery : IRequest<List<CancelPostingNotaKomisiTambahanDto>>
    {
        public string DatabaseName { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetCancelPostingNotaKomisiTambahanQueryHandler : IRequestHandler<GetCancelPostingNotaKomisiTambahanQuery, List<CancelPostingNotaKomisiTambahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCancelPostingNotaKomisiTambahanQueryHandler> _logger;

        public GetCancelPostingNotaKomisiTambahanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCancelPostingNotaKomisiTambahanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CancelPostingNotaKomisiTambahanDto>> Handle(GetCancelPostingNotaKomisiTambahanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CancelPostingNotaKomisiTambahanDto>(@"SELECT 
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
                            OR @SearchKeyword = '' OR @SearchKeyword IS NULL) AND p.flag_posting = 'Y' 
                            AND p.jns_tr = 'P' AND p.jns_nt_msk = '0' AND p.jns_nt_kel IN ('C', 'D')", new { request.SearchKeyword })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}