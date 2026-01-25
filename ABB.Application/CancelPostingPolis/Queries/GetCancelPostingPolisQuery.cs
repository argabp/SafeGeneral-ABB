using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingPolis.Queries
{
    public class GetCancelPostingPolisQuery : IRequest<List<CancelPostingPolisDto>>
    {
        public string DatabaseName { get; set; }

        public string SearchKeyword { get; set; }
    }

    public class GetCancelPostingPolisQueryHandler : IRequestHandler<GetCancelPostingPolisQuery, List<CancelPostingPolisDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCancelPostingPolisQueryHandler> _logger;

        public GetCancelPostingPolisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCancelPostingPolisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CancelPostingPolisDto>> Handle(GetCancelPostingPolisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CancelPostingPolisDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
                    FROM v_uw08e p
                        INNER JOIN rf01 cb
                        ON p.kd_cb = cb.kd_cb
                    INNER JOIN rf04 cob
                    ON p.kd_cob = cob.kd_cob
                    INNER JOIN rf05 scob
                    ON p.kd_cob = scob.kd_cob
                    AND p.kd_scob = scob.kd_scob
                    WHERE p.flag_posting = 'Y' AND (p.no_pol_ttg like '%'+@SearchKeyword+'%' 
                    OR p.tgl_mul_ptg like '%'+@SearchKeyword+'%' 
                    OR p.tgl_akh_ptg like '%'+@SearchKeyword+'%' 
                    OR p.tgl_closing like '%'+@SearchKeyword+'%' 
                    OR p.no_updt like '%'+@SearchKeyword+'%' 
                    OR cb.nm_cb like '%'+@SearchKeyword+'%' 
                    OR cob.nm_cob like '%'+@SearchKeyword+'%' 
                    OR scob.nm_scob like '%'+@SearchKeyword+'%' 
                    OR cb.kd_cb like '%'+@SearchKeyword+'%' 
                    OR cob.kd_cob like '%'+@SearchKeyword+'%' 
                    OR scob.kd_scob like '%'+@SearchKeyword+'%' 
                    OR p.nm_ttg like '%'+@SearchKeyword+'%' 
                    OR @SearchKeyword = '' OR @SearchKeyword IS NULL) AND p.flag_posting = 'Y'", new { request.SearchKeyword })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}