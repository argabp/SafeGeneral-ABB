using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Commands
{
    public class EditApprovalKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_approval { get; set; }
    }

    public class EditApprovalKlaimCommandHandler : IRequestHandler<EditApprovalKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditApprovalKlaimCommandHandler> _logger;

        public EditApprovalKlaimCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditApprovalKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditApprovalKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approval = dbContext.ApprovalKlaim.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (approval != null)
                {
                    approval.nm_approval = request.nm_approval;
                    
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