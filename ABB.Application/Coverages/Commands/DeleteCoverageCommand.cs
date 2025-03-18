using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Coverages.Commands
{
    public class DeleteCoverageCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cvrg { get; set; }
    }

    public class DeleteCoverageCommandHandler : IRequestHandler<DeleteCoverageCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteCoverageCommandHandler> _logger;

        public DeleteCoverageCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteCoverageCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteCoverageCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var coverage = dbContext.Coverage.FirstOrDefault(coverage => coverage.kd_cvrg == request.kd_cvrg);

                if (coverage != null)
                    dbContext.Coverage.Remove(coverage);

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