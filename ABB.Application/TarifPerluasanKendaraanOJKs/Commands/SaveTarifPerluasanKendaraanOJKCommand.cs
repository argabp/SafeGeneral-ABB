using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifPerluasanKendaraanOJKs.Commands
{
    public class SaveTarifPerluasanKendaraanOJKCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }

        public short no_kategori { get; set; }

        public decimal nilai_ptg_mul { get; set; }
        public decimal nilai_ptg_akh { get; set; }
        public byte stn_rate_prm { get; set; }
        public decimal pst_rate_prm_min { get; set; }
        public decimal pst_rate_prm_max { get; set; }
    }

    public class SaveTarifPerluasanKendaraanOJKCommandHandler : IRequestHandler<SaveTarifPerluasanKendaraanOJKCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveTarifPerluasanKendaraanOJKCommandHandler> _logger;

        public SaveTarifPerluasanKendaraanOJKCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveTarifPerluasanKendaraanOJKCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveTarifPerluasanKendaraanOJKCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kendaraanOjk = dbContext.KendaraanOJK.FirstOrDefault(w => w.kd_kategori == request.kd_kategori
                                                                              && w.kd_jns_ptg == request.kd_jns_ptg
                                                                              && w.kd_wilayah == request.kd_wilayah
                                                                              && w.no_kategori == request.no_kategori);

                if (kendaraanOjk == null)
                {
                    dbContext.KendaraanOJK.Add(new KendaraanOJK()
                    {
                        kd_kategori = request.kd_kategori,
                        kd_jns_ptg = request.kd_jns_ptg,
                        kd_wilayah = request.kd_wilayah,
                        no_kategori = request.no_kategori,
                        nilai_ptg_mul = request.nilai_ptg_mul,
                        nilai_ptg_akh = request.nilai_ptg_akh,
                        stn_rate_prm = request.stn_rate_prm,
                        pst_rate_prm_min = request.pst_rate_prm_min,
                        pst_rate_prm_max = request.pst_rate_prm_max
                    });
                }
                else
                {
                    kendaraanOjk.nilai_ptg_mul = request.nilai_ptg_mul;
                    kendaraanOjk.nilai_ptg_akh = request.nilai_ptg_akh;
                    kendaraanOjk.stn_rate_prm = request.stn_rate_prm;
                    kendaraanOjk.pst_rate_prm_min = request.pst_rate_prm_min;
                    kendaraanOjk.pst_rate_prm_max = request.pst_rate_prm_max;
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