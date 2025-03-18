using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kotas.Commands
{
    public class DeleteKotaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kota { get; set; }
    }

    public class DeleteKotaCommandHandler : IRequestHandler<DeleteKotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKotaCommandHandler> _logger;

        public DeleteKotaCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKotaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKotaCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kota = dbContext.Kota.FirstOrDefault(kota => kota.kd_kota == request.kd_kota);

                if (kota != null)
                    dbContext.Kota.Remove(kota);

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