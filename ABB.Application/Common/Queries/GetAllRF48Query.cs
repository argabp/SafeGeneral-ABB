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
    public class GetAllRF48Query : IRequest<List<RF48Dto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetAllRF48QueryHandler : IRequestHandler<GetAllRF48Query, List<RF48Dto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAllRF48QueryHandler> _logger;

        public GetAllRF48QueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAllRF48QueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<RF48Dto>> Handle(GetAllRF48Query request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<RF48Dto>(
                    @"SELECT RTRIM(LTRIM(kd_tol)) kd_tol, nm_tol, RTRIM(LTRIM(kd_cob)) kd_cob FROM rf48")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}