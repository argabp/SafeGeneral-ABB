using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Queries
{
    public class GetLimitKlaimsQuery : IRequest<List<LimitKlaimDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }

        public string KodeCabang { get; set; }
    }

    public class GetLimitKlaimsQueryHandler : IRequestHandler<GetLimitKlaimsQuery, List<LimitKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLimitKlaimsQueryHandler> _logger;

        public GetLimitKlaimsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLimitKlaimsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LimitKlaimDto>> Handle(GetLimitKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<LimitKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM MS_LimitKlaim p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND (p.nm_limit like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();

                var sequence = 0;
                foreach (var result in results)
                {
                    sequence++;
                    result.Id = sequence;
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