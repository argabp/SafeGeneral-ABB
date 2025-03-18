using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Coverages.Queries
{
    public class GetCoverageQuery : IRequest<List<CoverageDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetCoverageQueryHandler : IRequestHandler<GetCoverageQuery, List<CoverageDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCoverageQueryHandler> _logger;

        public GetCoverageQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCoverageQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<CoverageDto>> Handle(GetCoverageQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<CoverageDto>("SELECT * FROM rf17")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}