using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Commands
{
    public class DeleteLimitKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class DeleteLimitKlaimCommandHandler : IRequestHandler<DeleteLimitKlaimCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteLimitKlaimCommandHandler> _logger;

        public DeleteLimitKlaimCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteLimitKlaimCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLimitKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaimDetil = dbContext.LimitKlaimDetail.Where(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.thn == request.thn);

                var limitKlaim = dbContext.LimitKlaim.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.thn == request.thn);

                if (limitKlaim != null)
                {
                    dbContext.LimitKlaimDetail.RemoveRange(limitKlaimDetil);
                    dbContext.LimitKlaim.Remove(limitKlaim);

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