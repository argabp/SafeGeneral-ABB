using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaKlaimReasuransis.Commands
{
    public class SaveNotaKlaimReasuransiCommand : IRequest
    {
        
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string? ket_nt { get; set; }

        public string? ket_kwi { get; set; }
    }

    public class SaveNotaKlaimReasuransiCommandHandler : IRequestHandler<SaveNotaKlaimReasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveNotaKlaimReasuransiCommandHandler> _logger;

        public SaveNotaKlaimReasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<SaveNotaKlaimReasuransiCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveNotaKlaimReasuransiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var nota = _contextPst.NotaKlaimReasuransi.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                nota.kd_grp_ttj = request.kd_grp_ttj;
                nota.kd_rk_ttj = request.kd_rk_ttj;
                nota.nm_ttj = request.nm_ttj;
                nota.almt_ttj = request.almt_ttj;
                nota.kt_ttj = request.kt_ttj;
                nota.ket_nt = request.ket_nt;
                nota.ket_kwi = request.ket_kwi;

                await _contextPst.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }, _logger);
        }
    }
}