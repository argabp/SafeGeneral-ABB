using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeriodeProses.Commands
{
    public class AddPeriodeProsesCommand : IRequest
    {
        public DateTime PeriodeProses { get; set; }

        public bool FlagProses { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AddPeriodeProsesCommandHandler : IRequestHandler<AddPeriodeProsesCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<AddPeriodeProsesCommandHandler> _logger;

        public AddPeriodeProsesCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<AddPeriodeProsesCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddPeriodeProsesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var periodeProses = new PeriodeProsesModel()
                {
                    PeriodeProses = request.PeriodeProses,
                    FlagProses = request.FlagProses
                };

                dbContext.PeriodeProses.Add(periodeProses);

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