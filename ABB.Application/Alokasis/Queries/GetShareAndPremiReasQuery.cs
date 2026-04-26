using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetShareAndPremiReasQuery : IRequest<string>
    {
        public decimal nilai_ttl_ptg_reas { get; set; }
        public string kd_jns_sor { get; set; }
        public decimal nilai_ttl_ptg { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal net_prm { get; set; }
    }

    public class GetShareAndPremiReasQueryHandler : IRequestHandler<GetShareAndPremiReasQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetShareAndPremiReasQueryHandler> _logger;

        public GetShareAndPremiReasQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetShareAndPremiReasQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetShareAndPremiReasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _dbConnectionPst.QueryProc<string>("spe_ri05e_10",
                new
                {
                    request.nilai_ttl_ptg_reas, request.kd_jns_sor, request.nilai_ttl_ptg,
                    request.nilai_prm, request.net_prm
                })).FirstOrDefault(), _logger);
        }
    }
}