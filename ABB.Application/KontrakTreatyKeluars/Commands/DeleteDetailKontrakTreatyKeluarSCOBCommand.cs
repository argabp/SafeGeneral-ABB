using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Commands
{
    public class DeleteDetailKontrakTreatyKeluarSCOBCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class DeleteDetailKontrakTreatyKeluarSCOBCommandHandler : IRequestHandler<DeleteDetailKontrakTreatyKeluarSCOBCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteDetailKontrakTreatyKeluarSCOBCommandHandler> _logger;

        public DeleteDetailKontrakTreatyKeluarSCOBCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteDetailKontrakTreatyKeluarSCOBCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailKontrakTreatyKeluarSCOBCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarSCOB =
                    _contextPst.DetailKontrakTreatyKeluarSCOB.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_cob, request.kd_scob);

                if (detailKontrakTreatyKeluarSCOB == null)
                {
                    _logger.LogError(
                        "Failed Delete DetailKontrakTreatyKeluarSCOB with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_pps: {kd_tty_pps}, kd_cob: {kd_cob}, kd_scob: {kd_scob}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.kd_cob, request.kd_scob);
                    
                    throw new NotFoundException("Detail Kontrak Treaty Keluar SCOB Not Found");
                }
                
                _contextPst.DetailKontrakTreatyKeluarSCOB.Remove(detailKontrakTreatyKeluarSCOB);
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}