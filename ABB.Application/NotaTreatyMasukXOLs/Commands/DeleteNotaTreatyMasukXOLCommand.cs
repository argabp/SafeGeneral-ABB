using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasukXOLs.Commands
{
    public class DeleteNotaTreatyMasukXOLCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }
    }

    public class DeleteNotaTreatyMasukXOLCommandHandler : IRequestHandler<DeleteNotaTreatyMasukXOLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteNotaTreatyMasukXOLCommandHandler> _logger;

        public DeleteNotaTreatyMasukXOLCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteNotaTreatyMasukXOLCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNotaTreatyMasukXOLCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var transaksiTreatyMasuk =
                    _contextPst.TransaksiTreatyMasuk.Find(request.kd_cb, request.kd_thn, request.kd_bln, request.kd_jns_sor,
                        request.kd_tty_msk, request.kd_mtu, request.no_tr);
                
                if (transaksiTreatyMasuk == null)
                {
                    _logger.LogError(
                        "Failed Delete TransaksiTreatyMasuk with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_msk: {kd_tty_msk}, kd_tty_msk: {kd_thn}, kd_tty_msk: {kd_bln}, kd_tty_msk: {kd_mtu}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_msk, request.kd_thn, request.kd_bln, request.kd_mtu);
                    
                    throw new NotFoundException("Nota Treaty Masuk XOL Not Found");
                }
                
                _contextPst.TransaksiTreatyMasuk.Remove(transaksiTreatyMasuk);

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}