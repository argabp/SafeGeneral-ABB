using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyMasuks.Queries
{
    public class GetKeteranganTreatyQuery : IRequest<string>
    {
        public string kd_cob { get; set; }
        
        public string nm_jns_sor { get; set; }
        
        public decimal thn_uw { get; set; }
    }

    public class GetKeteranganTreatyQueryHandler : IRequestHandler<GetKeteranganTreatyQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetKeteranganTreatyQueryHandler> _logger;

        public GetKeteranganTreatyQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetKeteranganTreatyQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetKeteranganTreatyQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _dbConnectionPst.QueryProc<string>("spe_ri04e_05",
                new
                {
                    request.kd_cob, request.nm_jns_sor, request.thn_uw
                })).FirstOrDefault(), _logger);
        }
    }
}