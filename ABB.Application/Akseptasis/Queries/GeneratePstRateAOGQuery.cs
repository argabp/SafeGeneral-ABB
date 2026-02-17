using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstRateAOGQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_casco { get; set; }
        public bool flag_aog { get; set; }
    }

    public class GeneratePstRateAOGQueryHandler : IRequestHandler<GeneratePstRateAOGQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GeneratePstRateAOGQueryHandler> _logger;

        public GeneratePstRateAOGQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GeneratePstRateAOGQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GeneratePstRateAOGQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_03", 
                    new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_casco, request.flag_aog })).FirstOrDefault();
            }, _logger);
        }
    }
}