using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveKelurahanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string kd_kel { get; set; }

        public string nm_kel { get; set; }
    }

    public class SaveKelurahanCommandHandler : IRequestHandler<SaveKelurahanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKelurahanCommandHandler> _logger;

        public SaveKelurahanCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKelurahanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKelurahanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kelurahan = dbContext.Kelurahan.FirstOrDefault(w => w.kd_prop == request.kd_prop
                                                                        && w.kd_kab == request.kd_kab
                                                                        && w.kd_kec == request.kd_kec
                                                                        && w.kd_kel == request.kd_kel);

                if (kelurahan == null)
                {
                    dbContext.Kelurahan.Add(new Kelurahan()
                    {
                        kd_prop = request.kd_prop,
                        kd_kab = request.kd_kab,
                        kd_kec = request.kd_kec,
                        kd_kel = request.kd_kel,
                        nm_kel = request.nm_kel
                    });
                }
                else
                    kelurahan.nm_kel = request.nm_kel;

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