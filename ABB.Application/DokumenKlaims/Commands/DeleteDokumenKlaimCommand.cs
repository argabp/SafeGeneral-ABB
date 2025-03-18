using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenKlaims.Commands
{
    public class DeleteDokumenKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_dok { get; set; }
    }

    public class DeleteDokumenKlaimCommandHandler : IRequestHandler<DeleteDokumenKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDokumenKlaimCommandHandler> _logger;

        public DeleteDokumenKlaimCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDokumenKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDokumenKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenKlaim = dbContext.DokumenKlaim.FirstOrDefault(dokumenKlaim =>
                    dokumenKlaim.kd_dok == request.kd_dok && dokumenKlaim.kd_cob == request.kd_cob);

                if (dokumenKlaim != null)
                    dbContext.DokumenKlaim.Remove(dokumenKlaim);

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