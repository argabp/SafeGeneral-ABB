using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class DeleteLimitAkseptasiDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }

        public decimal nilai_limit_akhir { get; set; }
    }

    public class DeleteLimitAkseptasiDetilCommandHandler : IRequestHandler<DeleteLimitAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteLimitAkseptasiDetilCommandHandler> _logger;

        public DeleteLimitAkseptasiDetilCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteLimitAkseptasiDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLimitAkseptasiDetilCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasiDetil = dbContext.LimitAkseptasiDetil.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.nilai_limit_akhir == request.nilai_limit_akhir
                    && w.kd_user == request.kd_user && w.nilai_limit_awal == request.nilai_limit_awal);

                if (limitAkseptasiDetil != null)
                {
                    dbContext.LimitAkseptasiDetil.Remove(limitAkseptasiDetil);

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