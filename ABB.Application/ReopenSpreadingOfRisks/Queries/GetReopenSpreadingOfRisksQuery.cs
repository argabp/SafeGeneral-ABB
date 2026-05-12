using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Interfaces;
using ABB.Application.ReopenSpreadingOfRisks.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ReopenSpreadingOfRisks.Queries
{
    public class GetReopenSpreadingOfRisksQuery : IRequest<List<ReopenSpreadingOfRiskDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetReopenSpreadingOfRisksQueryHandler : IRequestHandler<GetReopenSpreadingOfRisksQuery, List<ReopenSpreadingOfRiskDto>>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetReopenSpreadingOfRisksQueryHandler> _logger;

        public GetReopenSpreadingOfRisksQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetReopenSpreadingOfRisksQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<List<ReopenSpreadingOfRiskDto>> Handle(GetReopenSpreadingOfRisksQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var rekanans = (await _dbConnectionPst.Query<ReopenSpreadingOfRiskDto>(@"
                        
                        SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(p.kd_cb, p.kd_cob, p.kd_scob, p.kd_thn, p.no_pol, p.no_updt) AS BIGINT) AS Id,
                            p.*,
                            cb.nm_cb,
                            cob.nm_cob,
                            scob.nm_scob,
                            SUBSTRING(p.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(p.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(p.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(p.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(p.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(p.no_pol_ttg, 16, LEN(p.no_pol_ttg)) no_pol_ttg_masked
                        FROM v_ri01e p
                        INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                            AND p.kd_scob = scob.kd_scob
                        WHERE p.flag_closing = 'Y' AND (cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_ttg like '%'+@SearchKeyword+'%' 
					OR p.tgl_closing like '%'+@SearchKeyword+'%' 
					OR p.tgl_mul_ptg like '%'+@SearchKeyword+'%' 
					OR p.tgl_akh_ptg like '%'+@SearchKeyword+'%' 
					OR p.no_pol_ttg like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();

                return rekanans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}