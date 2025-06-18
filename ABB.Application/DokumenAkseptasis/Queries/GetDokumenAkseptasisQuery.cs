using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class GetDokumenAkseptasisQuery : IRequest<List<DokumenAkseptasiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetDokumenAkseptasisQueryHandler : IRequestHandler<GetDokumenAkseptasisQuery, List<DokumenAkseptasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumenAkseptasisQueryHandler> _logger;

        public GetDokumenAkseptasisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumenAkseptasisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DokumenAkseptasiDto>> Handle(GetDokumenAkseptasisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<DokumenAkseptasiDto>(@"SELECT p.*, cob.nm_cob, scob.nm_scob 
				FROM MS_DokumenAkseptasi p
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_scob = scob.kd_scob
				WHERE (cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_dokumenakseptasi like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();

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