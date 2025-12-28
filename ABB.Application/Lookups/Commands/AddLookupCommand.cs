using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class AddLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_lookup { get; set; }

        public string nm_kategori { get; set; }
    }

    public class AddLookupCommandHandler : IRequestHandler<AddLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddLookupCommandHandler> _logger;

        public AddLookupCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddLookupCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddLookupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookup = new Lookup()
                {
                    kd_lookup = request.kd_lookup,
                    nm_kategori = request.nm_kategori
                };

                dbContext.Lookup.Add(lookup);

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