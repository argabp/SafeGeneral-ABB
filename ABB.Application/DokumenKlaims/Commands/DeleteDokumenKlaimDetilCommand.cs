using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenKlaims.Commands
{
    public class DeleteDokumenKlaimDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }
    }

    public class DeleteDokumenKlaimDetilCommandHandler : IRequestHandler<DeleteDokumenKlaimDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDokumenKlaimDetilCommandHandler> _logger;

        public DeleteDokumenKlaimDetilCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDokumenKlaimDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDokumenKlaimDetilCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenKlaimDetil = dbContext.DokumenKlaimDetil.FirstOrDefault(w =>
                    w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob && w.kd_dokumen == request.kd_dokumen);

                if (dokumenKlaimDetil != null)
                {
                    dbContext.DokumenKlaimDetil.Remove(dokumenKlaimDetil);

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