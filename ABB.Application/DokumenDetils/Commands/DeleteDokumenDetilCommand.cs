using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenDetils.Commands
{
    public class DeleteDokumenDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public Int16 kd_dokumen { get; set; }
    }

    public class DeleteDokumenDetilCommandHandler : IRequestHandler<DeleteDokumenDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDokumenDetilCommandHandler> _logger;

        public DeleteDokumenDetilCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDokumenDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDokumenDetilCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenDetil = dbContext.DokumenDetil.FirstOrDefault(w => w.kd_dokumen == request.kd_dokumen);

                if (dokumenDetil != null)
                    dbContext.DokumenDetil.Remove(dokumenDetil);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}