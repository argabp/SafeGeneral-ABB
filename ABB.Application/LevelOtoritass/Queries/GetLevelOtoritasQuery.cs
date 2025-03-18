using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LevelOtoritass.Queries
{
    public class GetLevelOtoritasQuery : IRequest<LevelOtoritasDto>
    {
        public string DatabaseName { get; set; }
        public string kd_user { get; set; }

        public string kd_pass { get; set; }

        public string? flag_xol { get; set; }

        public decimal? nilai_xol { get; set; }
    }

    public class GetLevelOtoritasQueryHandler : IRequestHandler<GetLevelOtoritasQuery, LevelOtoritasDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLevelOtoritasQueryHandler> _logger;

        public GetLevelOtoritasQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLevelOtoritasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<LevelOtoritasDto> Handle(GetLevelOtoritasQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LevelOtoritasDto>(@"SELECT * FROM rf41 WHERE kd_user = @kd_user", new
                {
                    request.kd_user
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