using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class EditLimitAkseptasiDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal pst_limit { get; set; }
    }

    public class EditLimitAkseptasiDetilCommandHandler : IRequestHandler<EditLimitAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLimitAkseptasiDetilCommandHandler> _logger;

        public EditLimitAkseptasiDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLimitAkseptasiDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLimitAkseptasiDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasiDetil = dbContext.LimitAkseptasiDetil.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.thn == request.thn
                    && w.kd_user == request.kd_user);

                if (limitAkseptasiDetil != null)
                {
                    limitAkseptasiDetil.pst_limit = request.pst_limit;
                    
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