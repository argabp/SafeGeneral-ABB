using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RiskAndLossProfiles.Queries
{
    public class GetRiskAndLossProfilesQuery : IRequest<List<RiskAndLossProfileDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetRiskAndLossProfilesQueryHandler : IRequestHandler<GetRiskAndLossProfilesQuery, List<RiskAndLossProfileDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetRiskAndLossProfilesQueryHandler> _logger;

        public GetRiskAndLossProfilesQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetRiskAndLossProfilesQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<RiskAndLossProfileDto>> Handle(GetRiskAndLossProfilesQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<RiskAndLossProfileDto>(@"SELECT RTRIM(LTRIM(b.kd_cob)) + CONVERT(varchar, b.nomor) Id ,
                                                                        b.*, m.nm_cob FROM dp16 b INNER JOIN rf04 m ON b.kd_cob = m.kd_cob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}