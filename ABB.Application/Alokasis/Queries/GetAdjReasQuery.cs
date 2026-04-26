using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetAdjReasQuery : IRequest<string>
    {
        public decimal pst_share { get; set; }
        public decimal pst_adj_reas { get; set; }
        public byte stn_adj_reas { get; set; }
        public decimal pst_kms { get; set; }
        public decimal nilai_prm_reas { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal pst_rate_prm { get; set; }
        public byte stn_rate_prm { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public Int16 no_rsk { get; set; }
    }

    public class GetAdjReasQueryHandler : IRequestHandler<GetAdjReasQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetAdjReasQueryHandler> _logger;

        public GetAdjReasQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetAdjReasQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetAdjReasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _dbConnectionPst.QueryProc<string>("spe_ri05e_06",
                new
                {
                    request.pst_share, request.pst_adj_reas, request.stn_adj_reas,
                    request.pst_kms, request.nilai_prm_reas, request.nilai_prm,
                    request.pst_rate_prm, request.stn_rate_prm, request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_pol, request.no_updt, request.no_rsk
                })).FirstOrDefault(), _logger);

        }
    }
}