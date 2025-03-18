using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KategoriJenisKendaraans.Commands
{
    public class DeleteKategoriJenisKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }
    }

    public class DeleteKategoriJenisKendaraanCommandHandler : IRequestHandler<DeleteKategoriJenisKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKategoriJenisKendaraanCommandHandler> _logger;

        public DeleteKategoriJenisKendaraanCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKategoriJenisKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKategoriJenisKendaraanCommand request,
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