using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenDetils.Commands
{
    public class SaveDokumenDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public Int16 kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }
    }

    public class SaveDokumenDetilCommandHandler : IRequestHandler<SaveDokumenDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDokumenDetilCommandHandler> _logger;

        public SaveDokumenDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDokumenDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDokumenDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenDetil = dbContext.DokumenDetil.FirstOrDefault(w => w.kd_dokumen == request.kd_dokumen);

                if (dokumenDetil == null)
                {
                    dbContext.DokumenDetil.Add(new DokumenDetil()
                    {
                        kd_dokumen = request.kd_dokumen,
                        nm_dokumen = request.nm_dokumen
                    });
                }
                else
                    dokumenDetil.nm_dokumen = request.nm_dokumen;
                
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