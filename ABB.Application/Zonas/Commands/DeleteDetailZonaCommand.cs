using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Commands
{
    public class DeleteDetailZonaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }

        public string kd_kls_konstr { get; set; }
    }

    public class DeleteDetailZonaCommandHandler : IRequestHandler<DeleteDetailZonaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailZonaCommandHandler> _logger;

        public DeleteDetailZonaCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailZonaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailZonaCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailZona = dbContext.DetailZona.FirstOrDefault(w => w.kd_zona == request.kd_zona
                                                                          && w.kd_kls_konstr == request.kd_kls_konstr);

                if (detailZona != null)
                    dbContext.DetailZona.Remove(detailZona);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}