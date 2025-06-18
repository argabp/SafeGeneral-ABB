using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class EditLimitAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_limit { get; set; }
    }

    public class EditLimitAkseptasiCommandHandler : IRequestHandler<EditLimitAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLimitAkseptasiCommandHandler> _logger;

        public EditLimitAkseptasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLimitAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLimitAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasi = dbContext.LimitAkseptasi.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (limitAkseptasi != null)
                {
                    limitAkseptasi.nm_limit = request.nm_limit;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}