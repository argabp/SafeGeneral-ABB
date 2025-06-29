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
    }

    public class AddPeriodeProsesCommandHandler : IRequestHandler<AddPeriodeProsesCommand>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<AddPeriodeProsesCommandHandler> _logger;

        public AddPeriodeProsesCommandHandler(IDbContextCSM dbContextCsm,
            ILogger<AddPeriodeProsesCommandHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddPeriodeProsesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var periodeProses = new PeriodeProsesModel()
                {
                    PeriodeProses = request.PeriodeProses,
                    FlagProses = request.FlagProses
                };

                _dbContextCsm.PeriodeProses.Add(periodeProses);

                await _dbContextCsm.SaveChangesAsync(cancellationToken);
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