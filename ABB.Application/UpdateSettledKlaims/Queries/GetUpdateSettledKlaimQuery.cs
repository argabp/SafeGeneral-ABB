using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.UpdateSettledKlaims.Queries
{
    public class GetUpdateSettledKlaimQuery : IRequest<List<UpdateSettledKlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
    }

    public class GetUpdateSettledKlaimQueryHandler : IRequestHandler<GetUpdateSettledKlaimQuery, List<UpdateSettledKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetUpdateSettledKlaimQueryHandler> _logger;

        public GetUpdateSettledKlaimQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetUpdateSettledKlaimQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<UpdateSettledKlaimDto>> Handle(GetUpdateSettledKlaimQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<UpdateSettledKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
					FROM cl01 p
						INNER JOIN rf01 cb
							ON p.kd_cb = cb.kd_cb
						INNER JOIN rf04 cob
							ON p.kd_cob = cob.kd_cob
						INNER JOIN rf05 scob
							ON p.kd_cob = scob.kd_cob
							AND p.kd_scob = scob.kd_scob
					WHERE p.flag_settled = 'N' AND p.kd_cb = @kd_cb AND (p.no_rsk like '%'+@SearchKeyword+'%' 
						OR p.tgl_updt like '%'+@SearchKeyword+'%' 
						OR p.no_updt like '%'+@SearchKeyword+'%' 
						OR cb.nm_cb like '%'+@SearchKeyword+'%' 
						OR cob.nm_cob like '%'+@SearchKeyword+'%' 
						OR scob.nm_scob like '%'+@SearchKeyword+'%' 
						OR cb.kd_cb like '%'+@SearchKeyword+'%' 
						OR cob.kd_cob like '%'+@SearchKeyword+'%' 
						OR scob.kd_scob like '%'+@SearchKeyword+'%' 
						OR p.no_kl like '%'+@SearchKeyword+'%' 
						OR p.no_pol_lama like '%'+@SearchKeyword+'%' 
						OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.kd_cb })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}