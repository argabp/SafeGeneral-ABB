using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kapals.Commands
{
    public class SaveKapalCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string kd_negara { get; set; }

        public int? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? st_class { get; set; }

        public string? no_reg { get; set; }

        public string? no_imo { get; set; }

        public string? ekuitas { get; set; }
    }

    public class SaveKapalCommandHandler : IRequestHandler<SaveKapalCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKapalCommandHandler> _logger;

        public SaveKapalCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKapalCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKapalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kapal = dbContext.Kapal.FirstOrDefault(kapal => kapal.kd_kapal == request.kd_kapal);

                if (kapal == null)
                {
                    dbContext.Kapal.Add(new Kapal()
                    {
                        kd_kapal = request.kd_kapal,
                        kd_negara = request.kd_negara,
                        ekuitas = request.ekuitas,
                        grt = request.grt,
                        merk_kapal = request.merk_kapal,
                        nm_kapal = request.nm_kapal,
                        no_imo = request.no_imo,
                        no_reg = request.no_reg,
                        st_class = request.st_class,
                        thn_buat = request.thn_buat
                    });
                }
                else
                {
                    kapal.kd_kapal = request.kd_kapal;
                    kapal.kd_negara = request.kd_negara;
                    kapal.ekuitas = request.ekuitas;
                    kapal.grt = request.grt;
                    kapal.merk_kapal = request.merk_kapal;
                    kapal.nm_kapal = request.nm_kapal;
                    kapal.no_imo = request.no_imo;
                    kapal.no_reg = request.no_reg;
                    kapal.st_class = request.st_class;
                    kapal.thn_buat = request.thn_buat;
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