using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaMaterais.Queries
{
    public class GetBiayaMateraisQuery : IRequest<List<BiayaMateraiDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetBiayaMateraisQueryHandler : IRequestHandler<GetBiayaMateraisQuery, List<BiayaMateraiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetBiayaMateraisQueryHandler> _logger;

        public GetBiayaMateraisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetBiayaMateraisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<BiayaMateraiDto>> Handle(GetBiayaMateraisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<BiayaMateraiDto>(@"SELECT RTRIM(LTRIM(b.kd_mtu)) + CONVERT(varchar, b.nilai_prm_mul) + CONVERT(varchar, b.nilai_prm_mul) Id ,
                                                                        b.*, m.nm_mtu + '(' + RTRIM(LTRIM(m.symbol)) + ')' nm_mtu FROM dp02 b INNER JOIN rf06 m ON b.kd_mtu = m.kd_mtu")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}