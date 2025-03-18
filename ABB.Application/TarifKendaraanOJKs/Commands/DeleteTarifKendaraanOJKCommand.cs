using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKendaraanOJKs.Commands
{
    public class DeleteTarifKendaraanOJKCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }

        public byte no_kategori { get; set; }
    }

    public class DeleteTarifKendaraanOJKCommandHandler : IRequestHandler<DeleteTarifKendaraanOJKCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteTarifKendaraanOJKCommandHandler> _logger;

        public DeleteTarifKendaraanOJKCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteTarifKendaraanOJKCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTarifKendaraanOJKCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kendaraanOjk = dbContext.KendaraanOJK.FirstOrDefault(w => w.kd_kategori == request.kd_kategori
                                                                              && w.kd_jns_ptg == request.kd_jns_ptg
                                                                              && w.kd_wilayah == request.kd_wilayah
                                                                              && w.no_kategori == request.no_kategori);

                if (kendaraanOjk != null)
                    dbContext.KendaraanOJK.Remove(kendaraanOjk);

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