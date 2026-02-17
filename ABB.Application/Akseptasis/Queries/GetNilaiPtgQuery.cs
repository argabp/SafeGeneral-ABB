using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetNilaiPtgQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_share { get; set; }
        public decimal nilai_ttl_ptg100 { get; set; }
    }

    public class GetNilaiPtgQueryHandler : IRequestHandler<GetNilaiPtgQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetNilaiPtgQueryHandler> _logger;

        public GetNilaiPtgQueryHandler(IDbConnectionFactory connectionFactory,
             ILogger<GetNilaiPtgQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetNilaiPtgQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_55", new { request.pst_share, request.nilai_ttl_ptg100 })).FirstOrDefault();
            }, _logger);
        }
    }
}