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
    public class SaveKecamatanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string nm_kec { get; set; }
    }

    public class SaveKecamatanCommandHandler : IRequestHandler<SaveKecamatanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKecamatanCommandHandler> _logger;

        public SaveKecamatanCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKecamatanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKecamatanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kecamatan = dbContext.Kecamatan.FirstOrDefault(w => w.kd_prop == request.kd_prop
                                                                        && w.kd_kab == request.kd_kab
                                                                        && w.kd_kec == request.kd_kec);

                if (kecamatan == null)
                {
                    dbContext.Kecamatan.Add(new Kecamatan()
                    {
                        kd_prop = request.kd_prop,
                        kd_kab = request.kd_kab,
                        kd_kec = request.kd_kec,
                        nm_kec = request.nm_kec
                    });
                }
                else
                    kecamatan.nm_kec = request.nm_kec;

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