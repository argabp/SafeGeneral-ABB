using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RiskAndLossProfiles.Commands
{
    public class DeleteRiskAndLossProfileCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public int nomor { get; set; }
    }

    public class DeleteRiskAndLossProfileCommandHandler : IRequestHandler<DeleteRiskAndLossProfileCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteRiskAndLossProfileCommandHandler> _logger;

        public DeleteRiskAndLossProfileCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteRiskAndLossProfileCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRiskAndLossProfileCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var riskAndLossProfile = dbContext.RiskAndLossProfile.FirstOrDefault(riskAndLossProfile => riskAndLossProfile.kd_cob == request.kd_cob
                    && riskAndLossProfile.nomor == request.nomor);

                if (riskAndLossProfile != null)
                    dbContext.RiskAndLossProfile.Remove(riskAndLossProfile);

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