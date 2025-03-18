using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenKlaims.Queries
{
    public class GetDokumenKlaimsQuery : IRequest<List<DokumenKlaimDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetDokumenKlaimsQueryHandler : IRequestHandler<GetDokumenKlaimsQuery, List<DokumenKlaimDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumenKlaimsQueryHandler> _logger;

        public GetDokumenKlaimsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumenKlaimsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DokumenKlaimDto>> Handle(GetDokumenKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DokumenKlaimDto>(@"
                    SELECT d.kd_cob + d.kd_dok Id,
	                    d.kd_cob, c.nm_cob, d.kd_dok,
	                    d.nm_dok FROM dp20 d
	                    INNER JOIN rf04 c
		                    ON d.kd_cob = c.kd_cob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}