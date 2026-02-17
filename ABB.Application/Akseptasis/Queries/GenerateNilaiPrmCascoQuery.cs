using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmCascoQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_guna { get; set; }
        public decimal nilai_casco { get; set; }
        public decimal pst_rate_prm { get; set; }
        public decimal stn_rate_prm { get; set; }
        public decimal nilai_tjh { get; set; }
    }

    public class GenerateNilaiPrmCascoQueryHandler : IRequestHandler<GenerateNilaiPrmCascoQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmCascoQueryHandler> _logger;

        public GenerateNilaiPrmCascoQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmCascoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPrmCascoQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_23", 
                    new { request.kd_guna, request.nilai_casco, request.pst_rate_prm, request.stn_rate_prm, request.nilai_tjh })).FirstOrDefault();
            }, _logger);
        }
    }
}