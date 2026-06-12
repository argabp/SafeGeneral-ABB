using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetSORsQuery : IRequest<List<SORDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetSORsQueryHandler : IRequestHandler<GetSORsQuery, List<SORDto>>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetSORsQueryHandler> _logger;

        public GetSORsQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetSORsQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<List<SORDto>> Handle(GetSORsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var rekanans = (await _dbConnectionPst.Query<SORDto>(@"
                        
                        SELECT DISTINCT 
                            CAST(uw01e.kd_cb AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_cob AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_scob AS VARCHAR) + '-' + 
                                CAST(uw01e.kd_thn AS VARCHAR) + '-' + 
                                CAST(uw01e.no_pol AS VARCHAR) + '-' + 
                                CAST(uw01e.no_updt AS VARCHAR) AS Id,
                            uw01e.kd_cb,   
                             uw01e.kd_cob,   
                             uw01e.kd_scob,   
                             uw01e.kd_thn,   
                             uw01e.no_pol,   
                             uw01e.no_updt,   
                             uw01e.no_renew,   
                             uw01e.thn_uw,   
                             uw01e.no_endt,   
                             uw01e.nm_ttg,   
                             uw01e.tgl_mul_ptg,   
                             uw01e.tgl_akh_ptg,   
                             uw01e.kd_usr_input,   
                             uw01e.tgl_input,   
                             uw01e.tgl_closing,   
                             ri01e.tgl_closing tgl_closing_reas,  
                             ri01e.flag_closing,   
                             ri01e.no_updt_reas,
                            cb.nm_cb,
                            cob.nm_cob,
                            scob.nm_scob,
                            SUBSTRING(uw01e.no_pol_ttg, 1, 2) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 3, 3) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 6, 2) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 8, 4) + '.' +
                            SUBSTRING(uw01e.no_pol_ttg, 12, 4) + '-' +
                            SUBSTRING(uw01e.no_pol_ttg, 16, LEN(uw01e.no_pol_ttg)) no_pol_ttg_masked
                            FROM uw01e 
                            LEFT JOIN ri01e ON 
                                uw01e.kd_cb = ri01e.kd_cb AND 
                                uw01e.kd_cob = ri01e.kd_cob AND 
                                uw01e.kd_scob = ri01e.kd_scob AND 
                                uw01e.kd_thn = ri01e.kd_thn AND 
                                uw01e.no_pol = ri01e.no_pol AND 
                                uw01e.no_updt = ri01e.no_updt
                            LEFT JOIN rf01 cb ON uw01e.kd_cb = cb.kd_cb
                            LEFT JOIN rf04 cob ON uw01e.kd_cob = cob.kd_cob
                        LEFT JOIN rf05 scob ON uw01e.kd_cob = scob.kd_cob 
                                           AND uw01e.kd_scob = scob.kd_scob 
                        WHERE (cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR ri01e.flag_closing like '%'+@SearchKeyword+'%' 
					OR uw01e.tgl_closing like '%'+@SearchKeyword+'%' 
					OR ri01e.tgl_closing like '%'+@SearchKeyword+'%' 
					OR uw01e.nm_ttg like '%'+@SearchKeyword+'%' 
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