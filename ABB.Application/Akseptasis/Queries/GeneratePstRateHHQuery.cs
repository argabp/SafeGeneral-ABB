using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstRateHHQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_casco { get; set; }
        public bool flag_hh { get; set; }
    }

    public class GeneratePstRateHHQueryHandler : IRequestHandler<GeneratePstRateHHQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GeneratePstRateHHQueryHandler> _logger;

        public GeneratePstRateHHQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GeneratePstRateHHQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GeneratePstRateHHQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_02", 
                    new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_casco, request.flag_hh })).FirstOrDefault();
            }, _logger);
        }
    }
}