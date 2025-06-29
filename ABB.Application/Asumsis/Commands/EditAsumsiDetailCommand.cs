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
    }

    public class EditAsumsiDetailCommandHandler : IRequestHandler<EditAsumsiDetailCommand>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<EditAsumsiDetailCommandHandler> _logger;

        public EditAsumsiDetailCommandHandler(IDbContextCSM dbContextCsm,
            ILogger<EditAsumsiDetailCommandHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditAsumsiDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asumsiDetail = _dbContextCsm.AsumsiDetail.FirstOrDefault(w => w.KodeAsumsi == request.KodeAsumsi
                                                                              && w.KodeProduk == request.KodeProduk
                                                                              && w.PeriodeProses == request.PeriodeProses
                                                                              && w.Thn == request.Thn);

                if (asumsiDetail != null)
                    asumsiDetail.Persentase = request.Persentase;

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