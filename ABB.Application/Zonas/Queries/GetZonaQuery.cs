using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Queries
{
    public class GetZonaQuery : IRequest<List<ZonaDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetZonaQueryHandler : IRequestHandler<GetZonaQuery, List<ZonaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetZonaQueryHandler> _logger;

        public GetZonaQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetZonaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ZonaDto>> Handle(GetZonaQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<ZonaDto>("SELECT * FROM rf12")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}