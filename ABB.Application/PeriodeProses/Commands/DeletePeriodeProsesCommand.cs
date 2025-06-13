using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeriodeProses.Commands
{
    public class DeletePeriodeProsesCommand : IRequest
    {
        public DateTime PeriodeProses { get; set; }

        public bool FlagProses { get; set; }
        public string DatabaseName { get; set; }
    }

    public class DeletePeriodeProsesCommandHandler : IRequestHandler<DeletePeriodeProsesCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeletePeriodeProsesCommandHandler> _logger;

        public DeletePeriodeProsesCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeletePeriodeProsesCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePeriodeProsesCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var periodeProses = dbContext.PeriodeProses.Find(request.PeriodeProses);

                dbContext.PeriodeProses.Remove(periodeProses);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}