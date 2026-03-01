using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaimTreaties.Commands
{
    public class SaveEntriNotaKlaimTreatyCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_nt { get; set; }

        public decimal nilai_nt { get; set; }

        public DateTime tgl_nt { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }
    }

    public class SaveEntriNotaKlaimTreatyCommandHandler : IRequestHandler<SaveEntriNotaKlaimTreatyCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveEntriNotaKlaimTreatyCommandHandler> _logger;

        public SaveEntriNotaKlaimTreatyCommandHandler(IDbContextPst contextPst,
            ILogger<SaveEntriNotaKlaimTreatyCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveEntriNotaKlaimTreatyCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var nota = _contextPst.NotaKlaimTreaty.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                nota.kd_cob = request.kd_cob;
                nota.kd_mtu = request.kd_mtu;
                nota.kd_grp_pas = request.kd_grp_pas;
                nota.kd_rk_pas = request.kd_rk_pas;
                nota.ket_nt = request.ket_nt;
                nota.nilai_nt = request.nilai_nt;
                nota.tgl_nt = request.tgl_nt;
                nota.flag_cancel = request.flag_cancel;
                nota.flag_posting = request.flag_posting;

                await _contextPst.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }, _logger);
        }
    }
}