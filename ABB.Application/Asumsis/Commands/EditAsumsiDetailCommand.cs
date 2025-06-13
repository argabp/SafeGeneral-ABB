using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class EditAsumsiDetailCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }

        public Int16 Thn { get; set; }

        public decimal Persentase { get; set; }
        public string DatabaseName { get; set; }
    }

    public class EditAsumsiDetailCommandHandler : IRequestHandler<EditAsumsiDetailCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<EditAsumsiDetailCommandHandler> _logger;

        public EditAsumsiDetailCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<EditAsumsiDetailCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditAsumsiDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var asumsiDetail = dbContext.AsumsiDetail.FirstOrDefault(w => w.KodeAsumsi == request.KodeAsumsi
                                                                              && w.KodeProduk == request.KodeProduk
                                                                              && w.PeriodeProses == request.PeriodeProses
                                                                              && w.Thn == request.Thn);

                if (asumsiDetail != null)
                    asumsiDetail.Persentase = request.Persentase;

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