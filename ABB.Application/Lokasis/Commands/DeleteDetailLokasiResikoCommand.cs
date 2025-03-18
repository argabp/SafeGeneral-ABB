using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class DeleteDetailLokasiResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }
        public string kd_lok_rsk { get; set; }
    }

    public class DeleteDetailLokasiResikoCommandHandler : IRequestHandler<DeleteDetailLokasiResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailLokasiResikoCommandHandler> _logger;

        public DeleteDetailLokasiResikoCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailLokasiResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailLokasiResikoCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailLokasiResiko = dbContext.DetailLokasiResiko.FirstOrDefault(detailLokasiResiko =>
                                                detailLokasiResiko.kd_pos == request.kd_pos
                                                && detailLokasiResiko.kd_lok_rsk == request.kd_lok_rsk);

                if (detailLokasiResiko != null)
                    dbContext.DetailLokasiResiko.Remove(detailLokasiResiko);

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