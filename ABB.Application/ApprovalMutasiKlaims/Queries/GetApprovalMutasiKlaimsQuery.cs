using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalMutasiKlaims.Queries
{
    public class GetApprovalMutasiKlaimsQuery : IRequest<List<ApprovalMutasiKlaimDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }

        public string KodeCabang { get; set; }
    }

    public class GetApprovalMutasiKlaimsQueryHandler : IRequestHandler<GetApprovalMutasiKlaimsQuery, List<ApprovalMutasiKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<GetApprovalMutasiKlaimsQueryHandler> _logger;

        public GetApprovalMutasiKlaimsQueryHandler(IDbConnectionFactory connectionFactory, 
            ICurrentUserService userService, ILogger<GetApprovalMutasiKlaimsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
            _logger = logger;
        }

        public async Task<List<ApprovalMutasiKlaimDto>> Handle(GetApprovalMutasiKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<ApprovalMutasiKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM v_tr_klaim p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND p.kd_user_status = @UserId AND p.status IN ('LKP Process', 'LKP Escalated') 
				  AND (p.user_status like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_tertanggung like '%'+@SearchKeyword+'%' 
					OR p.tgl_status like '%'+@SearchKeyword+'%' 
					OR p.nomor_berkas like '%'+@SearchKeyword+'%' 
					OR p.status like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", 
                    new { request.SearchKeyword, request.KodeCabang, _userService.UserId })).ToList();

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