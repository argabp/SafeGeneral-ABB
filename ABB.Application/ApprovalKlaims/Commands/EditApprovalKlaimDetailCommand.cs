using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Commands
{
    public class EditApprovalKlaimDetailCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user_sign { get; set; }

        public Int16 sla { get; set; }
    }

    public class EditApprovalKlaimDetailCommandHandler : IRequestHandler<EditApprovalKlaimDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditApprovalKlaimDetailCommandHandler> _logger;

        public EditApprovalKlaimDetailCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditApprovalKlaimDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditApprovalKlaimDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approvalDetail = dbContext.ApprovalKlaimDetail.FirstOrDefault(w => w.kd_cb == request.kd_cb
                    && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_status == request.kd_status
                    && w.kd_user == request.kd_user && w.kd_user_sign == request.kd_user_sign);

                if (approvalDetail != null)
                {
                    approvalDetail.sla = request.sla;
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