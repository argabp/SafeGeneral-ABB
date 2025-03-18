using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class EditPertanggunganKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public string? desk { get; set; }

        public Int16? jml_hari { get; set; }

        public string? ket_klasula { get; set; }

        public string? flag_tjh { get; set; }

        public string? flag_rscc { get; set; }

        public string? flag_banjir { get; set; }

        public string? flag_accessories { get; set; }

        public string? flag_lain_lain01 { get; set; }
        
        public string? flag_lain_lain02 { get; set; }
    }

    public class EditPertanggunganKendaraanCommandHandler : IRequestHandler<EditPertanggunganKendaraanCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditPertanggunganKendaraanCommandHandler> _logger;

        public EditPertanggunganKendaraanCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditPertanggunganKendaraanCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditPertanggunganKendaraanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var pertanggunganKendaraan =
                    dbContext.PertanggunganKendaraan.FirstOrDefault(
                        w => w.kd_cob == request.kd_cob
                                                && w.kd_jns_ptg == request.kd_jns_ptg
                                                && w.kd_scob == request.kd_scob);

                if (pertanggunganKendaraan != null)
                {
                    pertanggunganKendaraan.desk = request.desk;
                    pertanggunganKendaraan.jml_hari = request.jml_hari;
                    pertanggunganKendaraan.ket_klasula = request.ket_klasula;
                    pertanggunganKendaraan.flag_tjh = request.flag_tjh;
                    pertanggunganKendaraan.flag_rscc = request.flag_rscc;
                    pertanggunganKendaraan.flag_banjir = request.flag_banjir;
                    pertanggunganKendaraan.flag_accessories = request.flag_accessories;
                    pertanggunganKendaraan.flag_lain_lain01 = request.flag_lain_lain01;
                    pertanggunganKendaraan.flag_lain_lain02 = request.flag_lain_lain02;
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