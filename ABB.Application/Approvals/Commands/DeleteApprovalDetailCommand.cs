using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Approvals.Commands
{
    public class DeleteApprovalDetailCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user { get; set; }

        public string kd_user_sign { get; set; }
    }

    public class DeleteApprovalDetailCommandHandler : IRequestHandler<DeleteApprovalDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteApprovalDetailCommandHandler> _logger;

        public DeleteApprovalDetailCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteApprovalDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteApprovalDetailCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approvalDetail = dbContext.ApprovalDetail.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_status == request.kd_status
                    && w.kd_user == request.kd_user && w.kd_user_sign == request.kd_user_sign);

                if (approvalDetail != null)
                {
                    dbContext.ApprovalDetail.Remove(approvalDetail);

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