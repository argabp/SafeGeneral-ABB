using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePstRatePADQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_pad { get; set; }
    }

    public class GeneratePstRatePADQueryHandler : IRequestHandler<GeneratePstRatePADQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GeneratePstRatePADQueryHandler> _logger;

        public GeneratePstRatePADQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GeneratePstRatePADQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GeneratePstRatePADQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_12", 
                    new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_pad })).FirstOrDefault();
            }, _logger);
        }
    }
}