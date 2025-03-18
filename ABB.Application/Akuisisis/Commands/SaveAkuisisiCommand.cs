using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akuisisis.Commands
{
    public class SaveAkuisisiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int kd_thn { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_acq { get; set; }

        public decimal nilai_maks_acq { get; set; }

        public decimal nilai_acq { get; set; }
    }

    public class SaveAkuisisiCommandHandler : IRequestHandler<SaveAkuisisiCommand>
    {
        private readonly ILogger<SaveAkuisisiCommandHandler> _logger;
        private readonly IDbContextFactory _contextFactory;

        public SaveAkuisisiCommandHandler(ILogger<SaveAkuisisiCommandHandler> logger, IDbContextFactory contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(SaveAkuisisiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var akuisisi = dbContext.Akuisisi.FirstOrDefault(w => w.kd_mtu == request.kd_mtu
                                                                      && w.kd_cob == request.kd_cob
                                                                      && w.kd_scob == request.kd_scob
                                                                      && w.kd_thn == request.kd_thn);

                if (akuisisi == null)
                {
                    dbContext.Akuisisi.Add(new Akuisisi()
                    {
                        kd_mtu = request.kd_mtu,
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        kd_thn = request.kd_thn,
                        nilai_acq = request.nilai_acq,
                        nilai_maks_acq = request.nilai_maks_acq,
                        nilai_min_acq = request.nilai_min_acq
                    });
                }
                else
                {
                    akuisisi.nilai_acq = request.nilai_acq;
                    akuisisi.nilai_maks_acq = request.nilai_maks_acq;
                    akuisisi.nilai_min_acq = request.nilai_min_acq;
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