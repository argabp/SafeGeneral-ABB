using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Commands
{
    public class EditZonaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }

        public string nm_zona { get; set; }
    }

    public class EditZonaCommandHandler : IRequestHandler<EditZonaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditZonaCommandHandler> _logger;

        public EditZonaCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditZonaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditZonaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var zona = dbContext.Zona.FirstOrDefault(w => w.kd_zona == request.kd_zona);

                if (zona != null)
                    zona.nm_zona = request.nm_zona;

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