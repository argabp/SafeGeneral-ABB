using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetKodeTolsQuery : IRequest<List<RF48Dto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }
    }

    public class GetKodeTolsQueryHandler : IRequestHandler<GetKodeTolsQuery, List<RF48Dto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeTolsQueryHandler> _logger;

        public GetKodeTolsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeTolsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<RF48Dto>> Handle(GetKodeTolsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<RF48Dto>(
                    @"SELECT RTRIM(LTRIM(kd_tol)) kd_tol, nm_tol, RTRIM(LTRIM(kd_cob)) kd_cob FROM rf48 WHERE kd_cob = @kd_cob", new { request.kd_cob})).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}