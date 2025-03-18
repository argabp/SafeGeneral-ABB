using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Commands
{
    public class SaveDetailZonaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }

        public string kd_kls_konstr { get; set; }

        public string nm_zona_gb { get; set; }

        public string kd_okup { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm { get; set; }
    }

    public class SaveDetailZonaCommandHandler : IRequestHandler<SaveDetailZonaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDetailZonaCommandHandler> _logger;

        public SaveDetailZonaCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDetailZonaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailZonaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailZona = dbContext.DetailZona.FirstOrDefault(w => w.kd_kls_konstr == request.kd_kls_konstr
                                                                          && w.kd_zona == request.kd_zona);

                if (detailZona == null)
                {
                    dbContext.DetailZona.Add(new DetailZona()
                    {
                        kd_kls_konstr = request.kd_kls_konstr,
                        kd_okup = request.kd_okup,
                        pst_rate_prm = request.pst_rate_prm,
                        stn_rate_prm = request.stn_rate_prm,
                        kd_zona = request.kd_zona,
                        nm_zona_gb = request.nm_zona_gb
                    });
                }
                else
                {
                    detailZona.pst_rate_prm = request.pst_rate_prm;
                    detailZona.stn_rate_prm = request.stn_rate_prm;
                    detailZona.kd_okup = request.kd_okup;
                    detailZona.nm_zona_gb = request.nm_zona_gb;
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