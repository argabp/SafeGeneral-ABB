using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetKlaimAlokasiReasuransiQuery : IRequest<KlaimAlokasiReasuransi>
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

    public class GetKlaimAlokasiReasuransiQueryHandler : IRequestHandler<GetKlaimAlokasiReasuransiQuery, KlaimAlokasiReasuransi>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKlaimAlokasiReasuransiQueryHandler> _logger;

        public GetKlaimAlokasiReasuransiQueryHandler(IDbContextPst dbContextPst, ILogger<GetKlaimAlokasiReasuransiQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KlaimAlokasiReasuransi> Handle(GetKlaimAlokasiReasuransiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {                
                var klaimAlokasiReasuransi = _dbContextPst.KlaimAlokasiReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_grp_sor, request.kd_rk_sor);

                return klaimAlokasiReasuransi;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}