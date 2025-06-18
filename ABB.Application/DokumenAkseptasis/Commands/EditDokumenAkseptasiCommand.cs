using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Commands
{
    public class EditDokumenAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_dokumenakseptasi { get; set; }
    }

    public class EditDokumenAkseptasiCommandHandler : IRequestHandler<EditDokumenAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDokumenAkseptasiCommandHandler> _logger;

        public EditDokumenAkseptasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDokumenAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDokumenAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenAkseptasi = dbContext.DokumenAkseptasi.FirstOrDefault(w =>
                    w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (dokumenAkseptasi != null)
                {
                    dokumenAkseptasi.nm_dokumenakseptasi = request.nm_dokumenakseptasi;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

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