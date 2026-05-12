using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesSpreadingOfRisks.Configs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ProsesSpreadingOfRisks.Queries
{    
    public class GetProsesSpreadingOfRisksQuery : IRequest<List<ProsesSpreadingOfRiskDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetProsesSpreadingOfRisksQueryHandler : IRequestHandler<GetProsesSpreadingOfRisksQuery, List<ProsesSpreadingOfRiskDto>>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetProsesSpreadingOfRisksQueryHandler> _logger;

        public GetProsesSpreadingOfRisksQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetProsesSpreadingOfRisksQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<List<ProsesSpreadingOfRiskDto>> Handle(GetProsesSpreadingOfRisksQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var rekanans = (await _dbConnectionPst.Query<ProsesSpreadingOfRiskDto>(@"SELECT DISTINCT
                            CAST(BINARY_CHECKSUM(uw08e.kd_cb, uw08e.kd_cob, uw08e.kd_scob, uw08e.kd_thn, uw08e.no_pol, uw08e.no_updt) AS BIGINT) AS Id,
                               uw08e.kd_cb,
                               uw08e.kd_cob,
                               uw08e.kd_scob,
                               uw08e.kd_thn,
                               uw08e.kd_bln,
                               uw08e.no_pol,
                               uw08e.no_updt,
                               uw01e.nm_ttg,
                               uw01e.tgl_mul_ptg,
                               uw01e.tgl_akh_ptg,
                               uw01e.tgl_closing,
                                rf01.nm_cb,
                                rf04.nm_cob,
                                rf05.nm_scob,
                            SUBSTRING(uw08e.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(uw08e.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(uw08e.no_pol_ttg, 16, LEN(uw08e.no_pol_ttg)) no_pol_ttg_masked
                        FROM uw01e,
                             uw08e,
                             uw04e,
                             rf01,
                             rf04,
                             rf05
                        WHERE uw01e.kd_cb = uw08e.kd_cb
                              AND uw01e.kd_cob = uw08e.kd_cob
                              AND uw01e.kd_scob = uw08e.kd_scob
                              AND uw01e.kd_thn = uw08e.kd_thn
                              AND uw01e.no_pol = uw08e.no_pol
                              AND uw01e.no_updt = uw08e.no_updt
                              AND uw01e.flag_reas = 'N'
                              AND uw08e.flag_posting = 'Y'
                              AND uw08e.flag_cancel = 'N'
                              AND uw04e.nilai_prm <> 0
                              AND uw01e.kd_cb = uw04e.kd_cb
                              AND uw01e.kd_cob = uw04e.kd_cob
                              AND uw01e.kd_scob = uw04e.kd_scob
                              AND uw01e.kd_thn = uw04e.kd_thn
                              AND uw01e.no_pol = uw04e.no_pol
                              AND uw01e.no_updt = uw04e.no_updt
                              AND uw01e.kd_cb = rf01.kd_cb
                              AND uw01e.kd_cob = rf04.kd_cob
                              AND uw01e.kd_cob = rf05.kd_cob
                              AND uw01e.kd_scob = rf05.kd_scob
                        AND (rf01.nm_cb like '%'+@SearchKeyword+'%' 
					OR rf04.nm_cob like '%'+@SearchKeyword+'%' 
					OR rf05.nm_scob like '%'+@SearchKeyword+'%' 
					OR uw01e.nm_ttg like '%'+@SearchKeyword+'%' 
					OR uw01e.tgl_closing like '%'+@SearchKeyword+'%' 
					OR uw01e.tgl_mul_ptg like '%'+@SearchKeyword+'%' 
					OR uw01e.tgl_akh_ptg like '%'+@SearchKeyword+'%' 
					OR uw01e.no_pol_ttg like '%'+@SearchKeyword+'%' 
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