using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class GetNmTtyNppsQuery : IRequest<string>
    {
        public string kd_cob { get; set; }
        
        public string nm_jns_sor { get; set; }
        
        public decimal thn_tty_pps { get; set; }
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
                (await _connectionPst.QueryProc<string>("spe_ri04e_04", new
                {
                    request.kd_cob, request.nm_jns_sor, request.thn_tty_pps
                })).FirstOrDefault(), _logger);
        }
    }
}