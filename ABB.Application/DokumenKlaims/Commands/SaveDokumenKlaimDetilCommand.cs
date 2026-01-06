using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenKlaims.Commands
{
    public class SaveDokumenKlaimDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public short kd_dokumen { get; set; }

        public bool flag_wajib { get; set; }
    }

    public class SaveDokumenKlaimDetilCommandHandler : IRequestHandler<SaveDokumenKlaimDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDokumenKlaimDetilCommandHandler> _logger;

        public SaveDokumenKlaimDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDokumenKlaimDetilCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDokumenKlaimDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenKlaimDetil = dbContext.DokumenKlaimDetil.FirstOrDefault(dokumenKlaimDetil =>
                                                                dokumenKlaimDetil.kd_cob == request.kd_cob
                                                                && request.kd_scob == dokumenKlaimDetil.kd_scob
                                                                && request.kd_dokumen == dokumenKlaimDetil.kd_dokumen);

                if (dokumenKlaimDetil == null)
                {
                    dbContext.DokumenKlaimDetil.Add(new DokumenKlaimDetil()
                    {
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        kd_dokumen = request.kd_dokumen,
                        flag_wajib = request.flag_wajib
                    });
                }
                else
                    dokumenKlaimDetil.flag_wajib = request.flag_wajib;

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