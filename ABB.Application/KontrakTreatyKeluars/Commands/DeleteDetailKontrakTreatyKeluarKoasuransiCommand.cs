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
    public class DeleteDetailKontrakTreatyKeluarKoasuransiCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public int no_urut { get; set; }
    }

    public class DeleteDetailKontrakTreatyKeluarKoasuransiCommandHandler : IRequestHandler<DeleteDetailKontrakTreatyKeluarKoasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteDetailKontrakTreatyKeluarKoasuransiCommandHandler> _logger;

        public DeleteDetailKontrakTreatyKeluarKoasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteDetailKontrakTreatyKeluarKoasuransiCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailKontrakTreatyKeluarKoasuransiCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var detailKontrakTreatyKeluarKoasuransi =
                    _contextPst.DetailKontrakTreatyKeluarKoasuransi.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.no_urut);

                if (detailKontrakTreatyKeluarKoasuransi == null)
                {
                    _logger.LogError(
                        "Failed Delete DetailKontrakTreatyKeluarKoasuransi with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_pps: {kd_tty_pps}, no_urut: {no_urut}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.no_urut);
                    
                    throw new NotFoundException("Detail Kontrak Treaty Keluar Koasuransi Not Found");
                }
                
                _contextPst.DetailKontrakTreatyKeluarKoasuransi.Remove(detailKontrakTreatyKeluarKoasuransi);
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}