using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class SaveKlaimAlokasiReasuransiCommand : IRequest
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
        public decimal pst_share { get; set; }
        public decimal nilai_kl { get; set; }
        public string flag_cash_call { get; set; }
        public string flag_nota { get; set; }
        public string? flag_nt { get; set; }
    }

    public class SaveKlaimAlokasiReasuransiCommandHandler : IRequestHandler<SaveKlaimAlokasiReasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveKlaimAlokasiReasuransiCommandHandler> _logger;

        public SaveKlaimAlokasiReasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<SaveKlaimAlokasiReasuransiCommandHandler> logger)
        {
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKlaimAlokasiReasuransiCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailGrupResiko = _contextPst.KlaimAlokasiReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob, 
                                        request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_grp_sor, 
                                        request.kd_rk_sor);

                if (detailGrupResiko == null)
                {
                    _contextPst.KlaimAlokasiReasuransi.Add(new KlaimAlokasiReasuransi()
                    {
                        kd_cb = request.kd_cb,
                        kd_cob = request.kd_cob,
                        kd_scob = request.kd_scob,
                        kd_thn = request.kd_thn,
                        no_kl = request.no_kl,
                        no_mts = request.no_mts,
                        kd_jns_sor = request.kd_jns_sor,
                        kd_grp_sor = request.kd_grp_sor,
                        kd_rk_sor = request.kd_rk_sor,
                        pst_share = request.pst_share,
                        nilai_kl = request.nilai_kl,
                        flag_cash_call = request.flag_cash_call,
                        flag_nota = request.flag_nota,
                        flag_nt = request.flag_nt
                    });
                }
                else
                {
                    detailGrupResiko.pst_share = request.pst_share;
                    detailGrupResiko.nilai_kl = request.nilai_kl;
                    detailGrupResiko.flag_cash_call = request.flag_cash_call;
                    detailGrupResiko.flag_nota = request.flag_nota;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}