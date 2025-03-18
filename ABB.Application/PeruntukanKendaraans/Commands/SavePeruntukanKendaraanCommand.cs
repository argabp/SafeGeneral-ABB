using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PeruntukanKendaraans.Commands
{
    public class SavePeruntukanKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_utk { get; set; }
        
        public string nm_utk { get; set; }

        public string nm_utk_ing { get; set; }
    }

    public class SavePeruntukanKendaraanCommandHandler : IRequestHandler<SavePeruntukanKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SavePeruntukanKendaraanCommandHandler> _logger;

        public SavePeruntukanKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<SavePeruntukanKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SavePeruntukanKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var peruntukanKendaraan = dbContext.PeruntukanKendaraan.FirstOrDefault(peruntukanKendaraan =>
                    peruntukanKendaraan.kd_utk == request.kd_utk);

                if (peruntukanKendaraan == null)
                {
                    dbContext.PeruntukanKendaraan.Add(new PeruntukanKendaraan()
                    {
                        kd_utk = request.kd_utk,
                        nm_utk = request.nm_utk,
                        nm_utk_ing = request.nm_utk_ing
                    });
                }
                else
                {
                    peruntukanKendaraan.nm_utk = request.nm_utk;
                    peruntukanKendaraan.nm_utk_ing = request.nm_utk_ing;
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