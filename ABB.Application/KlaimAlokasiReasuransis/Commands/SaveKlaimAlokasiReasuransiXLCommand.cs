using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class SaveKlaimAlokasiReasuransiXLCommand : IRequest
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_kontr { get; set; }
        public decimal nilai_kl { get; set; }
        public decimal? nilai_reinst { get; set; }
    }

    public class SaveKlaimAlokasiReasuransiXLCommandHandler : IRequestHandler<SaveKlaimAlokasiReasuransiXLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveKlaimAlokasiReasuransiXLCommandHandler> _logger;

        public SaveKlaimAlokasiReasuransiXLCommandHandler(IDbContextPst contextPst,
            ILogger<SaveKlaimAlokasiReasuransiXLCommandHandler> logger)
        {
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKlaimAlokasiReasuransiXLCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailGrupResiko = _contextPst.KlaimAlokasiReasuransiXL.Find(request.kd_cb, request.kd_cob, request.kd_scob, 
                                        request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_kontr);

                if (detailGrupResiko == null)
                {
                    _contextPst.KlaimAlokasiReasuransiXL.Add(new KlaimAlokasiReasuransiXL()
                    {
                        kd_cb = request.kd_cb,
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        kd_thn = request.kd_thn,
                        no_kl = request.no_kl,
                        no_mts = request.no_mts,
                        kd_jns_sor = request.kd_jns_sor,
                        kd_kontr = request.kd_kontr,
                        nilai_kl = request.nilai_kl,
                        nilai_reinst = request.nilai_reinst,
                    });
                }
                else
                {
                    detailGrupResiko.nilai_kl = request.nilai_kl;
                    detailGrupResiko.nilai_reinst = request.nilai_reinst;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}