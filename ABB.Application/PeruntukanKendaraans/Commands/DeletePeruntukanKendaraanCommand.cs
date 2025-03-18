using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeruntukanKendaraans.Commands
{
    public class DeletePeruntukanKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_utk { get; set; }
    }

    public class DeletePeruntukanKendaraanCommandHandler : IRequestHandler<DeletePeruntukanKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeletePeruntukanKendaraanCommandHandler> _logger;

        public DeletePeruntukanKendaraanCommandHandler(IDbContextFactory contextFactory, ILogger<DeletePeruntukanKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePeruntukanKendaraanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var peruntukanKendaraan = dbContext.PeruntukanKendaraan.FirstOrDefault(peruntukanKendaraan =>
                    peruntukanKendaraan.kd_utk == request.kd_utk);

                if (peruntukanKendaraan != null)
                    dbContext.PeruntukanKendaraan.Remove(peruntukanKendaraan);

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