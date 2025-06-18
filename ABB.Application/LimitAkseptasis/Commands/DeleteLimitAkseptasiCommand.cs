using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class DeleteLimitAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class DeleteLimitAkseptasiCommandHandler : IRequestHandler<DeleteLimitAkseptasiCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteLimitAkseptasiCommandHandler> _logger;

        public DeleteLimitAkseptasiCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteLimitAkseptasiCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLimitAkseptasiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasiDetil = dbContext.LimitAkseptasiDetil.Where(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                var limitAkseptasi = dbContext.LimitAkseptasi.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (limitAkseptasi != null)
                {
                    dbContext.LimitAkseptasiDetil.RemoveRange(limitAkseptasiDetil);
                    dbContext.LimitAkseptasi.Remove(limitAkseptasi);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}