using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Commands
{
    public class DeleteDokumenAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class DeleteDokumenAkseptasiCommandHandler : IRequestHandler<DeleteDokumenAkseptasiCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteDokumenAkseptasiCommandHandler> _logger;

        public DeleteDokumenAkseptasiCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteDokumenAkseptasiCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDokumenAkseptasiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var dokumenAkseptasiDetils = dbContext.DokumenAkseptasiDetil.Where(w =>
                    w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                var dokumenAkseptasi = dbContext.DokumenAkseptasi.FirstOrDefault(w =>
                    w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                if (dokumenAkseptasi != null)
                {
                    dbContext.DokumenAkseptasiDetil.RemoveRange(dokumenAkseptasiDetils);
                    dbContext.DokumenAkseptasi.Remove(dokumenAkseptasi);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}