using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class AddDetailPertanggunganKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public short no_urut { get; set; }

        public decimal? nilai_tsi_tjh_mul { get; set; }

        public decimal? nilai_tsi_tjh_akh { get; set; }

        public decimal? pst_rate_tjh { get; set; }

        public byte? stn_rate_tjh { get; set; }

        public decimal? nilai_prm_tjh { get; set; }

        public decimal? nilai_tsi_tjp { get; set; }

        public decimal? nilai_prm_tjp { get; set; }

        public decimal? pst_rate_pad { get; set; }

        public decimal? pst_rate_pap { get; set; }
    }

    public class AddDetailPertanggunganKendaraanCommandHandler : IRequestHandler<AddDetailPertanggunganKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDetailPertanggunganKendaraanCommandHandler> _logger;

        public AddDetailPertanggunganKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDetailPertanggunganKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDetailPertanggunganKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var noUrut = dbContext.DetailPertanggunganKendaraan.Count(w => w.kd_cob == request.kd_cob &&
                                                                               w.kd_scob == request.kd_scob && w.kd_jns_ptg == request.kd_jns_ptg) + 1;
                
                var detailPertanggunganKendaraan = new DetailPertanggunganKendaraan()
                {
                    kd_cob = request.kd_cob,
                    kd_scob = "00",
                    kd_jns_ptg = request.kd_jns_ptg,
                    no_urut = (short)noUrut,
                    nilai_prm_tjh = request.nilai_prm_tjh,
                    nilai_prm_tjp = request.nilai_prm_tjp,
                    nilai_tsi_tjh_akh = request.nilai_tsi_tjh_akh,
                    nilai_tsi_tjh_mul = request.nilai_tsi_tjh_mul,
                    nilai_tsi_tjp = request.nilai_tsi_tjp,
                    pst_rate_pad = request.pst_rate_pad,
                    pst_rate_pap = request.pst_rate_pap,
                    pst_rate_tjh = request.pst_rate_tjh,
                    stn_rate_tjh = request.stn_rate_tjh
                };

                dbContext.DetailPertanggunganKendaraan.Add(detailPertanggunganKendaraan);

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