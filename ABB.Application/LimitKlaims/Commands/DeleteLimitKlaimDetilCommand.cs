using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Commands
{
    public class DeleteLimitKlaimDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }
    }

    public class DeleteLimitKlaimDetilCommandHandler : IRequestHandler<DeleteLimitKlaimDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteLimitKlaimDetilCommandHandler> _logger;

        public DeleteLimitKlaimDetilCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteLimitKlaimDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLimitKlaimDetilCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaimDetil = dbContext.LimitKlaimDetail.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.thn == request.thn
                    && w.kd_user == request.kd_user);

                if (limitKlaimDetil != null)
                {
                    dbContext.LimitKlaimDetail.Remove(limitKlaimDetil);

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