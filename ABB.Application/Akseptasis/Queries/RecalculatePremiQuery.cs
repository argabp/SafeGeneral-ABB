using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class RecalculatePremiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class RecalculatePremiQueryHandler : IRequestHandler<RecalculatePremiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<RecalculatePremiQueryHandler> _logger;

        public RecalculatePremiQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<RecalculatePremiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(RecalculatePremiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.QueryProc<(string, string, string)>("spp_uw02e_02", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt
                })).FirstOrDefault();

                return results.Item2;
            }, _logger);
        }
    }
}