using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PlatNomorKendaraans.Queries
{
    public class GetPlatNomorKendaraanQuery : IRequest<PlatNomorKendaraanDto>
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }
    }

    public class GetPlatNomorKendaraanQueryHandler : IRequestHandler<GetPlatNomorKendaraanQuery, PlatNomorKendaraanDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPlatNomorKendaraanQueryHandler> _logger;

        public GetPlatNomorKendaraanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPlatNomorKendaraanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<PlatNomorKendaraanDto> Handle(GetPlatNomorKendaraanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PlatNomorKendaraanDto>(@"SELECT * FROM rf10d WHERE kd_grp_rsk = @kd_grp_rsk AND kd_rsk = @kd_rsk", new
                {
                    request.kd_grp_rsk, request.kd_rsk
                })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}