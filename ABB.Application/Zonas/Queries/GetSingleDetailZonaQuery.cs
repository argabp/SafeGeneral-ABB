using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Queries
{
    public class GetSingleDetailZonaQuery : IRequest<DetailZona>
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class GetSingleDetailZonaQueryHandler : IRequestHandler<GetSingleDetailZonaQuery, DetailZona>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<GetSingleDetailZonaQueryHandler> _logger;

        public GetSingleDetailZonaQueryHandler(IDbContextFactory contextFactory, ILogger<GetSingleDetailZonaQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<DetailZona> Handle(GetSingleDetailZonaQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0, cancellationToken);
                
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                return dbContext.DetailZona.FirstOrDefault(w =>
                    w.kd_zona == request.kd_zona && w.kd_kls_konstr == request.kd_kls_konstr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}