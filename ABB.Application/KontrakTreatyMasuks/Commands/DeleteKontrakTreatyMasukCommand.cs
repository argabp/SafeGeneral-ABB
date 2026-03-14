using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasuks.Commands
{
    
    public class DeleteKontrakTreatyMasukCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }
    }

    public class DeleteKontrakTreatyMasukCommandHandler : IRequestHandler<DeleteKontrakTreatyMasukCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKontrakTreatyMasukCommandHandler> _logger;

        public DeleteKontrakTreatyMasukCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteKontrakTreatyMasukCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKontrakTreatyMasukCommand request, CancellationToken cancellationToken)
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