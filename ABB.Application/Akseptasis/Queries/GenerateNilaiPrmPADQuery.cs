using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiPrmPADQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_pad { get; set; }
        public decimal pst_rate_pad { get; set; }
        public decimal stn_rate_pad { get; set; }
    }

    public class GenerateNilaiPrmPADQueryHandler : IRequestHandler<GenerateNilaiPrmPADQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmPADQueryHandler> _logger;

        public GenerateNilaiPrmPADQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmPADQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiPrmPADQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_13", 
                    new { request.nilai_pad, request.pst_rate_pad, request.stn_rate_pad })).FirstOrDefault();
            }, _logger);
        }
    }
}