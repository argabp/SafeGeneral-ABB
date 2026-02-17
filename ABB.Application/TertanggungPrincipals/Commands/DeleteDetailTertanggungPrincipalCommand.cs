using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class DeleteDetailTertanggungPrincipalCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
    }

    public class DeleteDetailTertanggungPrincipalCommandHandler : IRequestHandler<DeleteDetailTertanggungPrincipalCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailTertanggungPrincipalCommandHandler> _logger;

        public DeleteDetailTertanggungPrincipalCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailTertanggungPrincipalCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailTertanggungPrincipalCommand request,
            CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailRekanan = dbContext.DetailRekanan.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_grp_rk == request.kd_grp_rk && w.kd_rk == request.kd_rk);

                if (detailRekanan != null)
                    dbContext.DetailRekanan.Remove(detailRekanan);

                await dbContext.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}