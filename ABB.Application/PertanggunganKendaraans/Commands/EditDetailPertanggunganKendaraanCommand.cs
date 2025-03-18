using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class EditDetailPertanggunganKendaraanCommand : IRequest
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

    public class EditDetailPertanggunganKendaraanCommandHandler : IRequestHandler<EditDetailPertanggunganKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDetailPertanggunganKendaraanCommandHandler> _logger;

        public EditDetailPertanggunganKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDetailPertanggunganKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDetailPertanggunganKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailPertanggunganKendaraan = dbContext.DetailPertanggunganKendaraan.FirstOrDefault(
                                                    w => w.kd_cob == request.kd_cob
                                                                           && w.kd_scob == request.kd_scob
                                                                           && w.kd_jns_ptg == request.kd_jns_ptg
                                                                           && w.no_urut == request.no_urut);

                if (detailPertanggunganKendaraan != null)
                {
                    detailPertanggunganKendaraan.nilai_prm_tjp = request.nilai_prm_tjp;
                    detailPertanggunganKendaraan.nilai_prm_tjh = request.nilai_prm_tjh;
                    detailPertanggunganKendaraan.pst_rate_tjh = request.pst_rate_tjh;
                    detailPertanggunganKendaraan.stn_rate_tjh = request.stn_rate_tjh;
                    detailPertanggunganKendaraan.nilai_tsi_tjh_mul = request.nilai_tsi_tjh_mul;
                    detailPertanggunganKendaraan.nilai_tsi_tjh_akh = request.nilai_tsi_tjh_akh;
                    detailPertanggunganKendaraan.nilai_tsi_tjp = request.nilai_tsi_tjp;
                    detailPertanggunganKendaraan.pst_rate_pad = request.pst_rate_pad;
                    detailPertanggunganKendaraan.pst_rate_pap = request.pst_rate_pap;
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