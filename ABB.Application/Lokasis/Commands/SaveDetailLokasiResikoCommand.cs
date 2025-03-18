using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveDetailLokasiResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }

        public string kd_lok_rsk { get; set; }

        public string gedung { get; set; }

        public string alamat { get; set; }

        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string kd_kel { get; set; }
    }

    public class SaveDetailLokasiResikoCommandHandler : IRequestHandler<SaveDetailLokasiResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDetailLokasiResikoCommandHandler> _logger;

        public SaveDetailLokasiResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDetailLokasiResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailLokasiResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailLokasiResiko = dbContext.DetailLokasiResiko.FirstOrDefault(w => w.kd_pos == request.kd_pos
                                                                                    && w.kd_lok_rsk == request.kd_lok_rsk);

                if (detailLokasiResiko == null)
                {
                    dbContext.DetailLokasiResiko.Add(new DetailLokasiResiko()
                    {
                        kd_pos = request.kd_pos,
                        kd_lok_rsk = request.kd_lok_rsk,
                        gedung = request.gedung,
                        alamat = request.alamat,
                        kd_prop = request.kd_prop,
                        kd_kab = request.kd_kab,
                        kd_kec = request.kd_kec,
                        kd_kel = request.kd_kel
                    });
                }
                else
                {
                    detailLokasiResiko.gedung = request.gedung;
                    detailLokasiResiko.alamat = request.alamat;
                    detailLokasiResiko.kd_prop = request.kd_prop;
                    detailLokasiResiko.kd_kab = request.kd_kab;
                    detailLokasiResiko.kd_kec = request.kd_kec;
                    detailLokasiResiko.kd_kel = request.kd_kel;
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