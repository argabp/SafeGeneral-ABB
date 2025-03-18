using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaPerSubCOBs.Queries
{
    public class GetBiayaPerSubCOBsQuery : IRequest<List<BiayaPerSubCOBDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetBiayaPerSubCOBsQueryHandler : IRequestHandler<GetBiayaPerSubCOBsQuery, List<BiayaPerSubCOBDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetBiayaPerSubCOBsQueryHandler> _logger;

        public GetBiayaPerSubCOBsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetBiayaPerSubCOBsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<BiayaPerSubCOBDto>> Handle(GetBiayaPerSubCOBsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<BiayaPerSubCOBDto>(@"SELECT RTRIM(LTRIM(b.kd_mtu)) + RTRIM(LTRIM(b.kd_cob)) + RTRIM(LTRIM(b.kd_scob)) Id ,
                                                                        b.*, m.nm_mtu + '(' + RTRIM(LTRIM(m.symbol)) + ')' nm_mtu,
																		c.nm_cob, s.nm_scob FROM dp03 b 
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