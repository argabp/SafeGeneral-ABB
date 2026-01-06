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
    public class SaveDokumenKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_dokumenklaim { get; set; }
    }

    public class SaveDokumenKlaimCommandHandler : IRequestHandler<SaveDokumenKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDokumenKlaimCommandHandler> _logger;

        public SaveDokumenKlaimCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDokumenKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDokumenKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenKlaim = dbContext.DokumenKlaim.FirstOrDefault(dokumenKlaim =>
                    dokumenKlaim.kd_cob == request.kd_cob && request.kd_scob == dokumenKlaim.kd_scob);

                if (dokumenKlaim == null)
                {
                    dbContext.DokumenKlaim.Add(new DokumenKlaim()
                    {
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        nm_dokumenklaim = request.nm_dokumenklaim
                    });
                }
                else
                    dokumenKlaim.nm_dokumenklaim = request.nm_dokumenklaim;

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