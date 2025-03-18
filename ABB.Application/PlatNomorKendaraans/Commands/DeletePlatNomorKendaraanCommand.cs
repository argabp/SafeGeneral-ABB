using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PlatNomorKendaraans.Commands
{
    public class DeletePlatNomorKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }
    }

    public class DeletePlatNomorKendaraanCommandHandler : IRequestHandler<DeletePlatNomorKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeletePlatNomorKendaraanCommandHandler> _logger;

        public DeletePlatNomorKendaraanCommandHandler(IDbContextFactory contextFactory, ILogger<DeletePlatNomorKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePlatNomorKendaraanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailGrupResiko = dbContext.DetailGrupResiko.FirstOrDefault(detailGrupResiko => detailGrupResiko.kd_grp_rsk == request.kd_grp_rsk
                    && detailGrupResiko.kd_rsk == request.kd_rsk);

                if (detailGrupResiko != null)
                    dbContext.DetailGrupResiko.Remove(detailGrupResiko);

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