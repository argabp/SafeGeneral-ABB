using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingMutasiKlaims.Queries
{
    public class GetCancelPostingMutasiKlaimsQuery : IRequest<List<CancelPostingMutasiKlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetCancelPostingMutasiKlaimsQueryHandler : IRequestHandler<GetCancelPostingMutasiKlaimsQuery, List<CancelPostingMutasiKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCancelPostingMutasiKlaimsQueryHandler> _logger;

        public GetCancelPostingMutasiKlaimsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCancelPostingMutasiKlaimsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CancelPostingMutasiKlaimDto>> Handle(GetCancelPostingMutasiKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CancelPostingMutasiKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
					FROM cl10 p
						INNER JOIN rf01 cb
							ON p.kd_cb = cb.kd_cb
						INNER JOIN rf04 cob
							ON p.kd_cob = cob.kd_cob
						INNER JOIN rf05 scob
							ON p.kd_cob = scob.kd_cob
							AND p.kd_scob = scob.kd_scob
					WHERE p.flag_cancel = 'N' AND p.no_dla = 0 AND p.flag_posting = 'Y' AND (
						cb.kd_cb like '%'+@SearchKeyword+'%' 
						OR cob.kd_cob like '%'+@SearchKeyword+'%' 
						OR scob.kd_scob like '%'+@SearchKeyword+'%' 
						OR p.no_kl like '%'+@SearchKeyword+'%' 
						OR p.no_mts like '%'+@SearchKeyword+'%' 
						OR p.nm_ttj like '%'+@SearchKeyword+'%' 
						OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}