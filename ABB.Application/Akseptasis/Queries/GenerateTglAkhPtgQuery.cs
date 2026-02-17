using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateTglAkhPtgQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal jk_wkt_main { get; set; }
        public DateTime tgl_akh_ptg { get; set; }
    }

    public class GenerateTglAkhPtgQueryHandler : IRequestHandler<GenerateTglAkhPtgQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateTglAkhPtgQueryHandler> _logger;

        public GenerateTglAkhPtgQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateTglAkhPtgQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateTglAkhPtgQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_92", new { request.jk_wkt_main, request.tgl_akh_ptg })).FirstOrDefault();
            }, _logger);
        }
    }
}