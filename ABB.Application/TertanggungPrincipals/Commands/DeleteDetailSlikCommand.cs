using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class DeleteDetailSlikCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
    }

    public class DeleteDetailSlikCommandHandler : IRequestHandler<DeleteDetailSlikCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailSlikCommandHandler> _logger;

        public DeleteDetailSlikCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailSlikCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailSlikCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailSlik = dbContext.DetailSlik.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_grp_rk == request.kd_grp_rk && w.kd_rk == request.kd_rk);

                if (detailSlik != null)
                    dbContext.DetailSlik.Remove(detailSlik);

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