using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasuks.Commands
{
    public class DeleteNotaTreatyMasukCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }
    }

    public class DeleteNotaTreatyMasukCommandHandler : IRequestHandler<DeleteNotaTreatyMasukCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteNotaTreatyMasukCommandHandler> _logger;

        public DeleteNotaTreatyMasukCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteNotaTreatyMasukCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteNotaTreatyMasukCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var notaTreatyMasuk =
                    _contextPst.NotaTreatyMasuk.Find(request.kd_cb, request.kd_thn, request.kd_bln, request.kd_jns_sor,
                        request.kd_tty_msk, request.kd_mtu, request.no_tr);
                
                if (notaTreatyMasuk == null)
                {
                    _logger.LogError(
                        "Failed Delete NotaTreatyMasuk with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_msk: {kd_tty_msk}, kd_tty_msk: {kd_thn}, kd_tty_msk: {kd_bln}, kd_tty_msk: {kd_mtu}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_msk, request.kd_thn, request.kd_bln, request.kd_mtu);
                    
                    throw new NotFoundException("Nota Treaty Masuk Not Found");
                }
                
                _contextPst.NotaTreatyMasuk.Remove(notaTreatyMasuk);

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}