using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Commands
{
    public class AddZonaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }

        public string nm_zona { get; set; }
    }

    public class AddZonaCommandHandler : IRequestHandler<AddZonaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddZonaCommandHandler> _logger;

        public AddZonaCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddZonaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddZonaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var zona = new Zona()
                {
                    kd_zona = request.kd_zona,
                    nm_zona = request.nm_zona
                };

                dbContext.Zona.Add(zona);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}