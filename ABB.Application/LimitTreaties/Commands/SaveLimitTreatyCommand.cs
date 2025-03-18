using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitTreaties.Commands
{
    public class SaveLimitTreatyCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_tol { get; set; }

        public string? nm_tol { get; set; }

        public string? kd_sub_grp { get; set; }
    }

    public class SaveLimitTreatyCommandHandler : IRequestHandler<SaveLimitTreatyCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveLimitTreatyCommandHandler> _logger;

        public SaveLimitTreatyCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveLimitTreatyCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveLimitTreatyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitTreaty = dbContext.LimitTreaty.FirstOrDefault(w => w.kd_cob == request.kd_cob
                                                                            && w.kd_tol == request.kd_tol);

                if (limitTreaty == null)
                {
                    dbContext.LimitTreaty.Add(new LimitTreaty()
                    {
                        kd_cob = request.kd_cob,
                        kd_tol = request.kd_tol,
                        nm_tol = request.nm_tol,
                        kd_sub_grp = request.kd_sub_grp
                    });
                }
                else
                {
                    limitTreaty.nm_tol = request.nm_tol;    
                    limitTreaty.kd_sub_grp = request.kd_sub_grp;    
                }

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