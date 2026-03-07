using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaKlaimReasuransis.Queries
{
    public class GenerateNotaKlaimReasuransiDataQuery : IRequest<string>
    {
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
    }

    public class GenerateNotaKlaimReasuransiReasuransiDataQueryHandler : IRequestHandler<GenerateNotaKlaimReasuransiDataQuery, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GenerateNotaKlaimReasuransiReasuransiDataQueryHandler> _logger;

        public GenerateNotaKlaimReasuransiReasuransiDataQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GenerateNotaKlaimReasuransiReasuransiDataQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNotaKlaimReasuransiDataQuery request, CancellationToken cancellationToken)
        {
            
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                return (await _connectionPst.QueryProc<string>("spe_cl07e_01", 
                    new { request.kd_cb, request.kd_grp_rk, request.kd_rk })).FirstOrDefault();
            }, _logger);
        }
    }
}