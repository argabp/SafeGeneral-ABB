using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class DeleteKlaimAlokasiReasuransiCommand : IRequest
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_grp_sor { get; set; }
        public string kd_rk_sor { get; set; }
    }

    public class DeleteKlaimAlokasiReasuransiCommandHandler : IRequestHandler<DeleteKlaimAlokasiReasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKlaimAlokasiReasuransiCommandHandler> _logger;

        public DeleteKlaimAlokasiReasuransiCommandHandler(IDbContextPst contextPst, 
            ILogger<DeleteKlaimAlokasiReasuransiCommandHandler> logger)
        {
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKlaimAlokasiReasuransiCommand request,
            CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var klaimAlokasiReasuransi = _contextPst.KlaimAlokasiReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob, 
                                        request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_grp_sor, request.kd_rk_sor);

                if (klaimAlokasiReasuransi != null)
                {

                    _contextPst.KlaimAlokasiReasuransi.Remove(klaimAlokasiReasuransi);
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}