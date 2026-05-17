using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasukXOLs.Commands
{
    public class DeleteKontrakTreatyMasukXOLCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }
    }

    public class DeleteKontrakTreatyMasukXOLCommandHandler : IRequestHandler<DeleteKontrakTreatyMasukXOLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKontrakTreatyMasukXOLCommandHandler> _logger;

        public DeleteKontrakTreatyMasukXOLCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteKontrakTreatyMasukXOLCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKontrakTreatyMasukXOLCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyMasuk =
                    _contextPst.KontrakTreatyMasuk.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_msk);

                if (kontrakTreatyMasuk == null)
                {
                    _logger.LogError(
                        "Failed Delete KontrakTreatyMasuk with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_msk: {kd_tty_msk}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_msk);
                    
                    throw new NotFoundException("Kontrak Treaty Masuk Not Found");
                }
                
                _contextPst.KontrakTreatyMasuk.Remove(kontrakTreatyMasuk);

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}