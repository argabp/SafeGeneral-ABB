using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PlatNomorKendaraans.Commands
{
    public class SavePlatNomorKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string? kd_ref { get; set; }
        public string? kd_ref1 { get; set; }
    }

    public class SavePlatNomorKendaraanCommandHandler : IRequestHandler<SavePlatNomorKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SavePlatNomorKendaraanCommandHandler> _logger;

        public SavePlatNomorKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<SavePlatNomorKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SavePlatNomorKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailGrupResiko = dbContext.DetailGrupResiko.FirstOrDefault(w => w.kd_grp_rsk == request.kd_grp_rsk
                    && w.kd_rsk == request.kd_rsk);

                if (detailGrupResiko == null)
                {
                    dbContext.DetailGrupResiko.Add(new DetailGrupResiko()
                    {
                        kd_grp_rsk = request.kd_grp_rsk,
                        kd_rsk = request.kd_rsk,
                        desk_rsk = request.desk_rsk,
                        kd_ref = request.kd_ref,
                        kd_ref1 = request.kd_ref1
                    });
                }
                else
                {
                    detailGrupResiko.desk_rsk = request.desk_rsk;
                    detailGrupResiko.kd_ref = request.kd_ref;
                    detailGrupResiko.kd_ref1 = request.kd_ref1;
                }

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