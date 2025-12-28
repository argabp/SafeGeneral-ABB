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
    public class GetAllLookupDetailsQuery : IRequest<List<LookupDetailDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetAllLookupDetailsQueryHandler : IRequestHandler<GetAllLookupDetailsQuery, List<LookupDetailDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAllLookupDetailsQueryHandler> _logger;

        public GetAllLookupDetailsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAllLookupDetailsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LookupDetailDto>> Handle(GetAllLookupDetailsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<LookupDetailDto>(
                    "SELECT * FROM MS_LookupDetail")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}