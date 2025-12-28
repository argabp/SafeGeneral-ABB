using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Commands
{
    public class DeleteDetailLookupCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_lookup { get; set; }

        public int no_lookup { get; set; }
    }

    public class DeleteDetailLookupCommandHandler : IRequestHandler<DeleteDetailLookupCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailLookupCommandHandler> _logger;

        public DeleteDetailLookupCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailLookupCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailLookupCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lookupDetail = dbContext.LookupDetail.FirstOrDefault(w => w.kd_lookup == request.kd_lookup
                    && w.no_lookup == request.no_lookup);

                if (lookupDetail != null)
                    dbContext.LookupDetail.Remove(lookupDetail);

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