using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Coverages.Commands
{
    public class SaveCoverageCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cvrg { get; set; }

        public string nm_cvrg { get; set; }
        public string nm_cvrg_ing { get; set; }
    }

    public class SaveCoverageCommandHandler : IRequestHandler<SaveCoverageCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveCoverageCommandHandler> _logger;

        public SaveCoverageCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveCoverageCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveCoverageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var coverage = dbContext.Coverage.FirstOrDefault(coverage => coverage.kd_cvrg == request.kd_cvrg);

                if (coverage == null)
                {
                    dbContext.Coverage.Add(new Coverage()
                    {
                        kd_cvrg = request.kd_cvrg,
                        nm_cvrg = request.nm_cvrg,
                        nm_cvrg_ing = request.nm_cvrg_ing
                    });
                }
                else {
                    coverage.nm_cvrg = request.nm_cvrg;
                    coverage.nm_cvrg_ing = request.nm_cvrg_ing;
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