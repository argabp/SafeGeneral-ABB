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
    public class GetDetailZonaQuery : IRequest<List<DetailZonaDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }
    }

    public class GetDetailZonaQueryHandler : IRequestHandler<GetDetailZonaQuery, List<DetailZonaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailZonaQueryHandler> _logger;

        public GetDetailZonaQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailZonaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailZonaDto>> Handle(GetDetailZonaQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailZonaDto>("SELECT kd_zona + detailZona.kd_kls_konstr Id, * FROM rf12d detailZona " +
                                                                      "INNER JOIN rf13 konstruksi ON detailZona.kd_kls_konstr = konstruksi.kd_kls_konstr " +
                                                                      "WHERE detailZona.kd_zona = @kd_zona",
                                                        new { request.kd_zona })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}