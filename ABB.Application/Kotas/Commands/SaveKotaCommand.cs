using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kotas.Commands
{
    public class SaveKotaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kota { get; set; }

        public string nm_kota { get; set; }
    }

    public class SaveKotaCommandHandler : IRequestHandler<SaveKotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKotaCommandHandler> _logger;

        public SaveKotaCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKotaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKotaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kota = dbContext.Kota.FirstOrDefault(w => w.kd_kota == request.kd_kota);

                if (kota == null)
                {
                    dbContext.Kota.Add(new Kota()
                    {
                        kd_kota = request.kd_kota,
                        nm_kota = request.nm_kota
                    });
                }
                else
                    kota.nm_kota = request.nm_kota;
                
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