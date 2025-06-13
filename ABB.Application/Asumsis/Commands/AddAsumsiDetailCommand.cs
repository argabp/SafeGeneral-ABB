using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class AddAsumsiDetailCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }

        public Int16 Thn { get; set; }

        public decimal Persentase { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AddAsumsiDetailCommandHandler : IRequestHandler<AddAsumsiDetailCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<AddAsumsiDetailCommandHandler> _logger;

        public AddAsumsiDetailCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<AddAsumsiDetailCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddAsumsiDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var asumsiDetail = new AsumsiDetail()
                {
                    KodeAsumsi = request.KodeAsumsi,
                    KodeProduk = request.KodeProduk,
                    PeriodeProses = request.PeriodeProses.Date,
                    Thn = request.Thn,
                    Persentase = request.Persentase
                };

                dbContext.AsumsiDetail.Add(asumsiDetail);

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