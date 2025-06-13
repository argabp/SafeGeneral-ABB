using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeriodeProses.Commands
{
    public class EditPeriodeProsesCommand : IRequest
    {
        public DateTime PeriodeProses { get; set; }

        public bool FlagProses { get; set; }
        public string DatabaseName { get; set; }
    }

    public class EditPeriodeProsesCommandHandler : IRequestHandler<EditPeriodeProsesCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<EditPeriodeProsesCommandHandler> _logger;

        public EditPeriodeProsesCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<EditPeriodeProsesCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditPeriodeProsesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var periodeProses = dbContext.PeriodeProses.Find(request.PeriodeProses);

                periodeProses.FlagProses = request.FlagProses;

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