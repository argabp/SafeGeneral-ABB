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
    public class GenerateNilaiPrmAndPstRateTJHKendQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_kend { get; set; }
        public string kd_wilayah { get; set; }
        public string kd_jns_ptg { get; set; }
        public decimal nilai_tjh { get; set; }
    }

    public class GenerateNilaiPrmAndPstRateTJHKendQueryHandler : IRequestHandler<GenerateNilaiPrmAndPstRateTJHKendQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiPrmAndPstRateTJHKendQueryHandler> _logger;

        public GenerateNilaiPrmAndPstRateTJHKendQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiPrmAndPstRateTJHKendQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GenerateNilaiPrmAndPstRateTJHKendQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_06", 
                    new { request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg, request.nilai_tjh })).ToList();
            }, _logger);
        }
    }
}