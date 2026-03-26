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
    public class DeleteDetailKontrakTreatyKeluarCoverageCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cvrg { get; set; }
    }

    public class DeleteDetailKontrakTreatyKeluarCoverageCommandHandler : IRequestHandler<DeleteDetailKontrakTreatyKeluarCoverageCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteDetailKontrakTreatyKeluarCoverageCommandHandler> _logger;

        public DeleteDetailKontrakTreatyKeluarCoverageCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteDetailKontrakTreatyKeluarCoverageCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailKontrakTreatyKeluarCoverageCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var detailKontrakTreatyKeluarCoverage =
                    _contextPst.DetailKontrakTreatyKeluarCoverage.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.kd_cvrg);

                if (detailKontrakTreatyKeluarCoverage == null)
                {
                    _logger.LogError(
                        "Failed Delete DetailKontrakTreatyKeluarCoverage with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_pps: {kd_tty_pps}, kd_cvrg: {kd_cvrg}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_pps, request.kd_cvrg);
                    
                    throw new NotFoundException("Detail Kontrak Treaty Keluar Coverage Not Found");
                }
                
                _contextPst.DetailKontrakTreatyKeluarCoverage.Remove(detailKontrakTreatyKeluarCoverage);
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}