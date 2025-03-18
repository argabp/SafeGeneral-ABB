using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class DeleteKelurahanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string kd_kel { get; set; }
    }

    public class DeleteKelurahanCommandHandler : IRequestHandler<DeleteKelurahanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKelurahanCommandHandler> _logger;

        public DeleteKelurahanCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKelurahanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKelurahanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kelurahan = dbContext.Kelurahan.FirstOrDefault(kelurahan => kelurahan.kd_kel == request.kd_kel
                    && kelurahan.kd_kec == request.kd_kec
                    && kelurahan.kd_kab == request.kd_kab
                    && kelurahan.kd_prop == request.kd_prop);

                if (kelurahan != null)
                    dbContext.Kelurahan.Remove(kelurahan);

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