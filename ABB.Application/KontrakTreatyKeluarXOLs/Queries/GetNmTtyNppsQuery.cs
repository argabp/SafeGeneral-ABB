using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class GetNmTtyNppsQuery : IRequest<string>
    {
        public string kd_cob { get; set; }
        
        public string nm_jns_sor { get; set; }
        
        public string npps_layer { get; set; }
        
        public decimal thn_tty_npps { get; set; }
    }

    public class GetNmTtyNppsQueryHandler : IRequestHandler<GetNmTtyNppsQuery, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetNmTtyNppsQueryHandler> _logger;

        public GetNmTtyNppsQueryHandler(IDbConnectionPst connectionPst, 
            ILogger<GetNmTtyNppsQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetNmTtyNppsQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _connectionPst.QueryProc<string>("spe_ri04e_03", new
                {
                    request.kd_cob, request.nm_jns_sor, request.npps_layer, request.thn_tty_npps
                })).FirstOrDefault(), _logger);
        }
    }
}