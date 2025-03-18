using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitTreaties.Queries
{
    public class GetLimitTreatiesQuery : IRequest<List<LimitTreatyDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetLimitTreatiesQueryHandler : IRequestHandler<GetLimitTreatiesQuery, List<LimitTreatyDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLimitTreatiesQueryHandler> _logger;

        public GetLimitTreatiesQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLimitTreatiesQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LimitTreatyDto>> Handle(GetLimitTreatiesQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LimitTreatyDto>(@"SELECT RTRIM(LTRIM(b.kd_cob)) + RTRIM(LTRIM(b.kd_tol)) Id ,
                                                                        b.*, m.nm_cob FROM rf48 b INNER JOIN rf04 m ON b.kd_cob = m.kd_cob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}