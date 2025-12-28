using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class DeleteLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_lookup { get; set; }
    }

    public class DeleteLookupCommandHandler : IRequestHandler<DeleteLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteLookupCommandHandler> _logger;

        public DeleteLookupCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteLookupCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLookupCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookupDetails = dbContext.LookupDetail.Where(w => w.kd_lookup == request.kd_lookup).ToList();
                var lookup = dbContext.Lookup.FirstOrDefault(w => w.kd_lookup == request.kd_lookup);

                if (lookup != null)
                {
                    dbContext.LookupDetail.RemoveRange(lookupDetails);
                    dbContext.Lookup.Remove(lookup);
                }

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