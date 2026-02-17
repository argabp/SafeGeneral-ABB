using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GeneratePrmPhkQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_ptg_hh { get; set; }
        public decimal pst_rate_aog { get; set; }
        public int stn_rate_aog { get; set; }
    }

    public class GeneratePrmPhkQueryHandler : IRequestHandler<GeneratePrmPhkQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GeneratePrmPhkQueryHandler> _logger;

        public GeneratePrmPhkQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GeneratePrmPhkQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GeneratePrmPhkQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_59", 
                    new { request.nilai_ptg_hh, request.pst_rate_aog, request.stn_rate_aog })).FirstOrDefault();
            }, _logger);
        }
    }
}