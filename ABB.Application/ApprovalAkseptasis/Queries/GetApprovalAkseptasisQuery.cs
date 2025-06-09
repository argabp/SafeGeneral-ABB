using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalAkseptasis.Queries
{
    public class GetApprovalAkseptasisQuery : IRequest<List<ApprovalAkseptasiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }

        public string KodeCabang { get; set; }
    }

    public class GetApprovalAkseptasisQueryHandler : IRequestHandler<GetApprovalAkseptasisQuery, List<ApprovalAkseptasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetApprovalAkseptasisQueryHandler> _logger;

        public GetApprovalAkseptasisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetApprovalAkseptasisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ApprovalAkseptasiDto>> Handle(GetApprovalAkseptasisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<ApprovalAkseptasiDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM v_TR_Akseptasi p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_scob = scob.kd_scob
				WHERE cb.kd_cb = @KodeCabang AND p.status NOT IN ('New', 'Revised', 'Cancel', 'Approved') AND (p.user_status like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_tertanggung like '%'+@SearchKeyword+'%' 
					OR p.tgl_status like '%'+@SearchKeyword+'%' 
					OR p.nomor_pengajuan like '%'+@SearchKeyword+'%' 
					OR p.status like '%'+@SearchKeyword+'%' 
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