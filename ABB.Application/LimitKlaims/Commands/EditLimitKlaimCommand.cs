using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Commands
{
    public class EditLimitKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }
    }

    public class EditLimitKlaimCommandHandler : IRequestHandler<EditLimitKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLimitKlaimCommandHandler> _logger;

        public EditLimitKlaimCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLimitKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLimitKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitKlaim = dbContext.LimitKlaim.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob 
                                             && w.kd_scob == request.kd_scob
                                             && w.thn == request.thn);

                if (limitKlaim != null)
                {
                    limitKlaim.nm_limit = request.nm_limit;
                    
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