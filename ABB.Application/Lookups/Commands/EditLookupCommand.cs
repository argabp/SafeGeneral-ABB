using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class EditLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_lookup { get; set; }

        public string nm_kategori { get; set; }
    }

    public class EditLookupCommandHandler : IRequestHandler<EditLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLookupCommandHandler> _logger;

        public EditLookupCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLookupCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLookupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookup = dbContext.Lookup.FirstOrDefault(w => w.kd_lookup == request.kd_lookup);

                if (lookup != null)
                {
                    lookup.nm_kategori = request.nm_kategori;
                }

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