using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Commands
{
    public class DeleteKontrakTreatyKeluarXOLCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }
    }

    public class DeleteKontrakTreatyKeluarXOLCommandHandler : IRequestHandler<DeleteKontrakTreatyKeluarXOLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKontrakTreatyKeluarXOLCommandHandler> _logger;

        public DeleteKontrakTreatyKeluarXOLCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteKontrakTreatyKeluarXOLCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKontrakTreatyKeluarXOLCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyKeluarXOL =
                    _contextPst.KontrakTreatyKeluarXOL.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_npps);

                if (kontrakTreatyKeluarXOL == null)
                {
                    _logger.LogError(
                        "Failed Delete KontrakTreatyKeluarXOL with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_npps: {kd_tty_npps}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_npps);
                    
                    throw new NotFoundException("Kontrak Treaty KeluarXOL Not Found");
                }
                
                var detailKontrakTreatyKeluarXOLs =
                    _contextPst.DetailKontrakTreatyKeluarXOL.Where(w => w.kd_cb == request.kd_cb 
                                                                        && w.kd_jns_sor == request.kd_jns_sor
                                                                        && w.kd_tty_npps == request.kd_tty_npps);
                
                _contextPst.KontrakTreatyKeluarXOL.Remove(kontrakTreatyKeluarXOL);
                _contextPst.DetailKontrakTreatyKeluarXOL.RemoveRange(detailKontrakTreatyKeluarXOLs);

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}