using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RiskAndLossProfiles.Queries
{
    public class GetRiskAndLossProfileQuery : IRequest<RiskAndLossProfileDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public int nomor { get; set; }
    }

    public class GetRiskAndLossProfileQueryHandler : IRequestHandler<GetRiskAndLossProfileQuery, RiskAndLossProfileDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetRiskAndLossProfileQueryHandler> _logger;

        public GetRiskAndLossProfileQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetRiskAndLossProfileQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<RiskAndLossProfileDto> Handle(GetRiskAndLossProfileQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<RiskAndLossProfileDto>(@"SELECT * FROM dp16 WHERE kd_cob = @kd_cob AND nomor = @nomor", new
                {
                    request.kd_cob, request.nomor
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