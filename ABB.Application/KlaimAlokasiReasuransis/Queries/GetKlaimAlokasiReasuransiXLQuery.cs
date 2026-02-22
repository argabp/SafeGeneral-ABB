using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GetKlaimAlokasiReasuransiXLQuery : IRequest<KlaimAlokasiReasuransiXL>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_kontr { get; set; }
    }

    public class GetKlaimAlokasiReasuransiXLQueryHandler : IRequestHandler<GetKlaimAlokasiReasuransiXLQuery, KlaimAlokasiReasuransiXL>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKlaimAlokasiReasuransiXLQueryHandler> _logger;

        public GetKlaimAlokasiReasuransiXLQueryHandler(IDbContextPst dbContextPst, ILogger<GetKlaimAlokasiReasuransiXLQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<KlaimAlokasiReasuransiXL> Handle(GetKlaimAlokasiReasuransiXLQuery request,
            CancellationToken cancellationToken)
        {
            try
            {                
                var klaimAlokasiReasuransiXL = _dbContextPst.KlaimAlokasiReasuransiXL.Find(request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_kontr);

                return klaimAlokasiReasuransiXL;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}