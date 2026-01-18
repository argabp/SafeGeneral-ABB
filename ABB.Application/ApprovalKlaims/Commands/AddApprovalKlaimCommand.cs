using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Commands
{
    public class AddApprovalKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_approval { get; set; }
    }

    public class AddApprovalKlaimCommandHandler : IRequestHandler<AddApprovalKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddApprovalKlaimCommandHandler> _logger;

        public AddApprovalKlaimCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddApprovalKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddApprovalKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var approval = new ApprovalKlaim()
                {
                    kd_cb = request.kd_cb,
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    nm_approval = request.nm_approval,
                };

                dbContext.ApprovalKlaim.Add(approval);

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