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
    public class DeleteKontrakTreatyKeluarCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }
    }

    public class DeleteKontrakTreatyKeluarCommandHandler : IRequestHandler<DeleteKontrakTreatyKeluarCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKontrakTreatyKeluarCommandHandler> _logger;

        public DeleteKontrakTreatyKeluarCommandHandler(IDbContextPst contextPst,
            ILogger<DeleteKontrakTreatyKeluarCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKontrakTreatyKeluarCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var kontrakTreatyKeluar =
                    _contextPst.KontrakTreatyKeluar.Find(request.kd_cb, request.kd_jns_sor, request.kd_tty_pps);

                if (kontrakTreatyKeluar == null)
                {
                    _logger.LogError(
                        "Failed Delete KontrakTreatyKeluar with kd_cb: {kd_cb}, kd_jns_sor: {kd_jns_sor}, kd_tty_pps: {kd_tty_pps}",
                        request.kd_cb, request.kd_jns_sor, request.kd_tty_pps);
                    
                    throw new NotFoundException("Kontrak Treaty Keluar Not Found");
                }
                
                var detailKontrakTreatyKeluars =
                    _contextPst.DetailKontrakTreatyKeluar.Where(w => w.kd_cb == request.kd_cb 
                                                                        && w.kd_jns_sor == request.kd_jns_sor
                                                                        && w.kd_tty_pps == request.kd_tty_pps);
                
                
                var detailKontrakTreatyKeluarCoverages =
                    _contextPst.DetailKontrakTreatyKeluarCoverage.Where(w => w.kd_cb == request.kd_cb 
                                                                     && w.kd_jns_sor == request.kd_jns_sor
                                                                     && w.kd_tty_pps == request.kd_tty_pps);

                
                var detailKontrakTreatyKeluarExcludes =
                    _contextPst.DetailKontrakTreatyKeluarExclude.Where(w => w.kd_cb == request.kd_cb 
                                                                     && w.kd_jns_sor == request.kd_jns_sor
                                                                     && w.kd_tty_pps == request.kd_tty_pps);

                
                var detailKontrakTreatyKeluarKoasuransis =
                    _contextPst.DetailKontrakTreatyKeluarKoasuransi.Where(w => w.kd_cb == request.kd_cb 
                                                                     && w.kd_jns_sor == request.kd_jns_sor
                                                                     && w.kd_tty_pps == request.kd_tty_pps);

                
                var detailKontrakTreatyKeluarSOBs =
                    _contextPst.DetailKontrakTreatyKeluarSCOB.Where(w => w.kd_cb == request.kd_cb 
                                                                     && w.kd_jns_sor == request.kd_jns_sor
                                                                     && w.kd_tty_pps == request.kd_tty_pps);

                
                var detailKontrakTreatyKeluarTableOfLimits =
                    _contextPst.DetailKontrakTreatyKeluarTableOfLimit.Where(w => w.kd_cb == request.kd_cb 
                                                                     && w.kd_jns_sor == request.kd_jns_sor
                                                                     && w.kd_tty_pps == request.kd_tty_pps);

                _contextPst.KontrakTreatyKeluar.Remove(kontrakTreatyKeluar);
                _contextPst.DetailKontrakTreatyKeluar.RemoveRange(detailKontrakTreatyKeluars);
                _contextPst.DetailKontrakTreatyKeluarCoverage.RemoveRange(detailKontrakTreatyKeluarCoverages);
                _contextPst.DetailKontrakTreatyKeluarExclude.RemoveRange(detailKontrakTreatyKeluarExcludes);
                _contextPst.DetailKontrakTreatyKeluarKoasuransi.RemoveRange(detailKontrakTreatyKeluarKoasuransis);
                _contextPst.DetailKontrakTreatyKeluarSCOB.RemoveRange(detailKontrakTreatyKeluarSOBs);
                _contextPst.DetailKontrakTreatyKeluarTableOfLimit.RemoveRange(detailKontrakTreatyKeluarTableOfLimits);

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}