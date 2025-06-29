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
    }

    public class DeletePeriodeProsesCommandHandler : IRequestHandler<DeletePeriodeProsesCommand>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<DeletePeriodeProsesCommandHandler> _logger;

        public DeletePeriodeProsesCommandHandler(IDbContextCSM dbContextCsm, ILogger<DeletePeriodeProsesCommandHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePeriodeProsesCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var periodeProses = _dbContextCsm.PeriodeProses.Find(request.PeriodeProses);

                _dbContextCsm.PeriodeProses.Remove(periodeProses);

                await _dbContextCsm.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}