using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPremiPaQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_pa { get; set; }
        public decimal pst_rate_pa { get; set; }
    }

    public class GenerateNilaiPremiPaQueryHandler : IRequestHandler<GenerateNilaiPremiPaQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPremiPaQueryHandler> _logger;

        public GenerateNilaiPremiPaQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPremiPaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPremiPaQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_kp02e_09", 
                    new { request.nilai_pa, request.pst_rate_pa })).FirstOrDefault();
            }, _logger);
        }
    }
}