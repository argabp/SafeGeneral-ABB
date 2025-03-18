using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class DeleteDetailPertanggunganKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public short no_urut { get; set; }
    }

    public class DeleteDetailPertanggunganKendaraanCommandHandler : IRequestHandler<DeleteDetailPertanggunganKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailPertanggunganKendaraanCommandHandler> _logger;

        public DeleteDetailPertanggunganKendaraanCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailPertanggunganKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailPertanggunganKendaraanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailPertanggunganKendaraan = dbContext.DetailPertanggunganKendaraan.FirstOrDefault(
                    w => w.kd_cob == request.kd_cob
                    && w.kd_scob == request.kd_scob
                    && w.kd_jns_ptg == request.kd_jns_ptg
                    && w.no_urut == request.no_urut);

                if (detailPertanggunganKendaraan != null)
                    dbContext.DetailPertanggunganKendaraan.Remove(detailPertanggunganKendaraan);

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