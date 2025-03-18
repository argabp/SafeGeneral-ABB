using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akuisisis.Queries
{
    public class GetAkuisisisQuery : IRequest<List<AkuisisiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetAkuisisisQueryHandler : IRequestHandler<GetAkuisisisQuery, List<AkuisisiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkuisisisQueryHandler> _logger;

        public GetAkuisisisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAkuisisisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<AkuisisiDto>> Handle(GetAkuisisisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<AkuisisiDto>(@"SELECT RTRIM(LTRIM(b.kd_mtu)) + RTRIM(LTRIM(b.kd_cob)) + RTRIM(LTRIM(b.kd_scob)) + CONVERT(varchar,b.kd_thn) Id ,
                                                                        b.*, m.nm_mtu + '(' + RTRIM(LTRIM(m.symbol)) + ')' nm_mtu,
																		c.nm_cob, s.nm_scob FROM rf40 b 
																		INNER JOIN rf06 m ON b.kd_mtu = m.kd_mtu
																		INNER JOIN rf04 c ON b.kd_cob = c.kd_cob
																		INNER JOIN rf05 s on s.kd_scob = b.kd_scob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}