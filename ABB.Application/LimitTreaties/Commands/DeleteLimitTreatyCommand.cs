using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitTreaties.Commands
{
    public class DeleteLimitTreatyCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_tol { get; set; }
    }

    public class DeleteLimitTreatyCommandHandler : IRequestHandler<DeleteLimitTreatyCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteLimitTreatyCommandHandler> _logger;

        public DeleteLimitTreatyCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteLimitTreatyCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLimitTreatyCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitTreaty = dbContext.LimitTreaty.FirstOrDefault(limitTreaty => limitTreaty.kd_cob == request.kd_cob
                    && limitTreaty.kd_tol == request.kd_tol);

                if (limitTreaty != null)
                    dbContext.LimitTreaty.Remove(limitTreaty);

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