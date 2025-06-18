using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Commands
{
    public class AddDokumenAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_dokumenakseptasi { get; set; }
    }

    public class AddDokumenAkseptasiCommandHandler : IRequestHandler<AddDokumenAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDokumenAkseptasiCommandHandler> _logger;

        public AddDokumenAkseptasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDokumenAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDokumenAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenAkseptasi = new DokumenAkseptasi()
                {
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    nm_dokumenakseptasi = request.nm_dokumenakseptasi,
                };

                dbContext.DokumenAkseptasi.Add(dokumenAkseptasi);

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