using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PlatNomorKendaraans.Queries
{
    public class GetPlatNomorKendaraansQuery : IRequest<List<PlatNomorKendaraanDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetPlatNomorKendaraansQueryHandler : IRequestHandler<GetPlatNomorKendaraansQuery, List<PlatNomorKendaraanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPlatNomorKendaraansQueryHandler> _logger;

        public GetPlatNomorKendaraansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPlatNomorKendaraansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PlatNomorKendaraanDto>> Handle(GetPlatNomorKendaraansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PlatNomorKendaraanDto>(@"SELECT RTRIM(LTRIM(b.kd_grp_rsk)) + RTRIM(LTRIM(b.kd_rsk)) Id ,
                                                                        b.*, m.desk_grp_rsk FROM rf10d b INNER JOIN rf10 m ON b.kd_grp_rsk = m.kd_grp_rsk 
                                                                        WHERE b.kd_grp_rsk = '106'")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}