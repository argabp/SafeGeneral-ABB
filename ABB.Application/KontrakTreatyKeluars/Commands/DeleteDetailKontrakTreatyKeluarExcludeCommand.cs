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
    public class DeleteDetailKontrakTreatyKeluarExcludeCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }
    }

    public class DeleteDetailKontrakTreatyKeluarExcludeCommandHandler : IRequestHandler<DeleteDetailKontrakTreatyKeluarExcludeCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteDetailKontrakTreatyKeluarExcludeCommandHandler> _logger;

        public DeleteDetailKontrakTreatyKeluarExcludeCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteDetailKontrakTreatyKeluarExcludeCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailKontrakTreatyKeluarExcludeCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var detailKontrakTreatyKeluarExclude =
                    _contextPst.DetailKontrakTreatyKeluarExclude.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.kd_okup);

                if (detailKontrakTreatyKeluarExclude == null)
                {
                    _logger.LogError(
                        "Failed Delete DetailKontrakTreatyKeluarExclude with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_pps: {kd_tty_pps}, kd_okup: {kd_okup}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.kd_okup);
                    
                    throw new NotFoundException("Detail Kontrak Treaty Keluar Exclude Not Found");
                }
                
                _contextPst.DetailKontrakTreatyKeluarExclude.Remove(detailKontrakTreatyKeluarExclude);
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}