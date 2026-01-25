using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaims.Queries
{
    public class GetEntriNotaKlaimsQuery : IRequest<List<EntriNotaKlaimDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }

        public string kd_cb { get; set; }
    }

    public class GetEntriNotaKlaimsQueryHandler : IRequestHandler<GetEntriNotaKlaimsQuery, List<EntriNotaKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetEntriNotaKlaimsQueryHandler> _logger;

        public GetEntriNotaKlaimsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetEntriNotaKlaimsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<EntriNotaKlaimDto>> Handle(GetEntriNotaKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<EntriNotaKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob, 
                                   c.no_pol_lama FROM cl10 p 
                        INNER JOIN cl01 c
                            ON c.kd_cb = p.kd_cb
                                AND  c.kd_cob = p.kd_cob
                                AND  c.kd_scob = p.kd_scob
                                AND  c.kd_thn = p.kd_thn
                                AND  c.no_kl = p.no_kl
                        INNER JOIN rf01 cb
                           ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob
                           ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob
                           ON p.kd_cob = scob.kd_cob
                           AND p.kd_scob = scob.kd_scob
                        WHERE p.flag_cancel = 'N' AND p.kd_cb = @kd_cb AND
                              (p.no_nt_msk like '%'+@SearchKeyword+'%' 
					OR p.nm_ttj like '%'+@SearchKeyword+'%' 
					OR p.nilai_nt like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR cob.kd_cob like '%'+@SearchKeyword+'%' 
					OR scob.kd_scob like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.kd_cb })).ToList();

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