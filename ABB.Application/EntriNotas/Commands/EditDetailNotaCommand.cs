using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Commands
{
    public class EditDetailNotaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public DateTime tgl_ang { get; set; }

        public DateTime tgl_jth_tempo { get; set; }

        public decimal pst_ang { get; set; }

        public decimal nilai_ang { get; set; }

        public byte no_ang { get; set; }
    }

    public class EditDetailNotaCommandHandler : IRequestHandler<EditDetailNotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDetailNotaCommandHandler> _logger;

        public EditDetailNotaCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDetailNotaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDetailNotaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailNota = dbContext.DetailNota.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.jns_tr == request.jns_tr &&
                    w.jns_nt_msk == request.jns_nt_msk && w.kd_thn == request.kd_thn &&
                    w.kd_bln == request.kd_bln && w.no_nt_msk == request.no_nt_msk &&
                    w.jns_nt_kel == request.jns_nt_kel && w.no_nt_kel == request.no_nt_kel &&
                    w.no_ang == request.no_ang);

                if (detailNota != null)
                {
                    detailNota.tgl_ang = request.tgl_ang;
                    detailNota.tgl_jth_tempo = request.tgl_jth_tempo;
                    detailNota.pst_ang = request.pst_ang;
                    detailNota.nilai_ang = request.nilai_ang;
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