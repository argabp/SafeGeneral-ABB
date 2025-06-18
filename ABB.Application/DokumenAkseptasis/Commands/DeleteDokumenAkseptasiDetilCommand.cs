using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Commands
{
    public class DeleteDokumenAkseptasiDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }
    }

    public class DeleteDokumenAkseptasiDetilCommandHandler : IRequestHandler<DeleteDokumenAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDokumenAkseptasiDetilCommandHandler> _logger;

        public DeleteDokumenAkseptasiDetilCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDokumenAkseptasiDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDokumenAkseptasiDetilCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenAkseptasiDetil = dbContext.DokumenAkseptasiDetil.FirstOrDefault(w =>
                    w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_dokumen == request.kd_dokumen);

                if (dokumenAkseptasiDetil != null)
                {
                    dbContext.DokumenAkseptasiDetil.Remove(dokumenAkseptasiDetil);

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