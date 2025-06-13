using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class AddAsumsiPeriodeCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AddAsumsiPeriodeCommandHandler : IRequestHandler<AddAsumsiPeriodeCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<AddAsumsiPeriodeCommandHandler> _logger;

        public AddAsumsiPeriodeCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<AddAsumsiPeriodeCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddAsumsiPeriodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asumsiPeriode = new AsumsiPeriode()
                {
                    KodeAsumsi = request.KodeAsumsi,
                    KodeProduk = request.KodeProduk,
                    PeriodeProses = request.PeriodeProses.Date
                };

                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                dbContext.AsumsiPeriode.Add(asumsiPeriode);

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