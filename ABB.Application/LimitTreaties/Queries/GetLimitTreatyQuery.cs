using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitTreaties.Queries
{
    public class GetLimitTreatyQuery : IRequest<LimitTreatyDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_tol { get; set; }
    }

    public class GetLimitTreatyQueryHandler : IRequestHandler<GetLimitTreatyQuery, LimitTreatyDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLimitTreatyQueryHandler> _logger;

        public GetLimitTreatyQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLimitTreatyQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<LimitTreatyDto> Handle(GetLimitTreatyQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LimitTreatyDto>(@"SELECT * FROM rf48 WHERE kd_cob = @kd_cob AND kd_tol = @kd_tol", new
                {
                    request.kd_cob, request.kd_tol
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