using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Commands
{
    public class AddApprovalKlaimDetailCommand : IRequest
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

    public class AddApprovalKlaimDetailCommandHandler : IRequestHandler<AddApprovalKlaimDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddApprovalKlaimDetailCommandHandler> _logger;

        public AddApprovalKlaimDetailCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddApprovalKlaimDetailCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddApprovalKlaimDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approvalDetail = new ApprovalKlaimDetail()
                {
                    kd_cb = request.kd_cb,
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    kd_status = request.kd_status,
                    kd_user = request.kd_user,
                    kd_user_sign = request.kd_user_sign,
                    sla = request.sla
                };

                dbContext.ApprovalKlaimDetail.Add(approvalDetail);

                await dbContext.SaveChangesAsync(cancellationToken);
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