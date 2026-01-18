using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Commands
{
    public class DeleteApprovalKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class DeleteApprovalKlaimCommandHandler : IRequestHandler<DeleteApprovalKlaimCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteApprovalKlaimCommandHandler> _logger;

        public DeleteApprovalKlaimCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteApprovalKlaimCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteApprovalKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var approvalDetails = dbContext.ApprovalKlaimDetail.Where(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                var approval = dbContext.ApprovalKlaim.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (approval != null)
                {
                    dbContext.ApprovalKlaimDetail.RemoveRange(approvalDetails);
                    dbContext.ApprovalKlaim.Remove(approval);

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