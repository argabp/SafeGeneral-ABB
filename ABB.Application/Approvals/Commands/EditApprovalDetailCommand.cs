using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Approvals.Commands
{
    public class EditApprovalDetailCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_status { get; set; }

        public decimal nilai_limit_awal { get; set; }

        public decimal nilai_limit_akhir { get; set; }

        public string kd_user_sign { get; set; }

        public Int16 sla { get; set; }
    }

    public class EditApprovalDetailCommandHandler : IRequestHandler<EditApprovalDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditApprovalDetailCommandHandler> _logger;

        public EditApprovalDetailCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditApprovalDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditApprovalDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approvalDetail = dbContext.ApprovalDetail.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_status == request.kd_status);

                if (approvalDetail != null)
                {
                    approvalDetail.nilai_limit_awal = request.nilai_limit_awal;
                    approvalDetail.nilai_limit_akhir = request.nilai_limit_akhir;
                    approvalDetail.sla = request.sla;
                    approvalDetail.kd_user_sign = request.kd_user_sign;
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