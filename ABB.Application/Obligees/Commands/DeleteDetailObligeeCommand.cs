using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Obligees.Commands
{
    public class DeleteDetailObligeeCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
    }

    public class DeleteDetailObligeeCommandHandler : IRequestHandler<DeleteDetailObligeeCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailObligeeCommandHandler> _logger;

        public DeleteDetailObligeeCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailObligeeCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailObligeeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailObligee = dbContext.DetailObligee.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_grp_rk == request.kd_grp_rk && w.kd_rk == request.kd_rk);

                if (detailObligee != null)
                    dbContext.DetailObligee.Remove(detailObligee);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex.InnerException ?? ex;
            }

            return Unit.Value;
        }
    }
}