using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingMutasiKlaims.Queries
{
    public class GetPostingMutasiKlaimQuery : IRequest<List<PostingMutasiKlaimDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetPostingMutasiKlaimQueryHandler : IRequestHandler<GetPostingMutasiKlaimQuery, List<PostingMutasiKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPostingMutasiKlaimQueryHandler> _logger;

        public GetPostingMutasiKlaimQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPostingMutasiKlaimQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PostingMutasiKlaimDto>> Handle(GetPostingMutasiKlaimQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PostingMutasiKlaimDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
					FROM cl10 p
						INNER JOIN rf01 cb
							ON p.kd_cb = cb.kd_cb
						INNER JOIN rf04 cob
							ON p.kd_cob = cob.kd_cob
						INNER JOIN rf05 scob
							ON p.kd_cob = scob.kd_cob
							AND p.kd_scob = scob.kd_scob
					WHERE p.flag_posting = 'N' AND (p.no_mts like '%'+@SearchKeyword+'%' 
						OR p.tgl_nt like '%'+@SearchKeyword+'%' 
						OR cb.nm_cb like '%'+@SearchKeyword+'%' 
						OR cob.nm_cob like '%'+@SearchKeyword+'%' 
						OR scob.nm_scob like '%'+@SearchKeyword+'%' 
						OR cb.kd_cb like '%'+@SearchKeyword+'%' 
						OR cob.kd_cob like '%'+@SearchKeyword+'%' 
						OR scob.kd_scob like '%'+@SearchKeyword+'%' 
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